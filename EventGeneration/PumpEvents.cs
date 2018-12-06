
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System.Text;
using Microsoft.Azure.EventHubs;
using System;

namespace EventGeneration
{
    public static class PumpEvents
    {
        const string sampleline = "{0},{0},{1},22888,333666,1000,1,9,90296,POST,5G,5G,FALSE,30,6,3,C,{2},145660,24646,B,AC,0,8870514,EUR,1389,19,0,16,0,19,I,TRUE,33,0,0,12,0527834015,0548334096,{1},{2},490154229596238,36901562,1000,0,0,US,US,2897,8127562_10000,DUB,0,0,0,DUB,1000,564878895,0,0,0,5G,1,1,1005,0,S,NA,1,4G,0,0,0,{1},0,0,{0},{0},20,1000014622,A,10";
        [FunctionName("PumpEvents")]
        public static void Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [EventHub("rated_v2", Connection = "EVENT_HUB_CONN")]  ICollector<EventData> outputEventHubMessage,
            ILogger log)
        {
            DateTime Dtime = DateTime.Now;
            long time = Dtime.Ticks;
            long callTime = time - 30000;
            long endCallTime = time - 25000;

            // parse query parameter
            string bulksize = req.Query["bulksize"];
            bulksize = (bulksize != null) ? bulksize : "500";
            int bsize = int.Parse(bulksize);

            log.LogInformation($"EventPump:: Called with {bsize} as bulk size");
            
            EventData eventData = new EventData(Encoding.UTF8.GetBytes(string.Format(sampleline,time, callTime, endCallTime)));
            // creating bulk message container with the requested number of messages
            for (int i = 0; i < bsize; i++)
            {
                outputEventHubMessage.Add(eventData);
            }

            log.LogInformation($"EventPump:: finished pumping {bsize} events");            
        }

    }
}
