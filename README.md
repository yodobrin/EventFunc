# EventFunc
## Overview
Simulator which push multiple events to eventhub. Each function would push diffrent type of messages to designated event hub.
The event hub would be used by ADX as the source to specific tables.

The main idea behind this repository is to show case the query capabilities of ADX together with its near real time data ingestion.


## High Level Architecture
![High Level Architecture](https://user-images.githubusercontent.com/37622785/49592639-53ad6900-f97a-11e8-972f-cb27b33031bb.png)

## Main Components

### PumpEvents
The function will send bulk messagesto an event hub. the event hub connection is defined within the application settings of the function app. The name of the eventhub is currently defined as part of the function code.

The function will attempt to pull from the query param *bulksize* and use this value as the number of bulk messages, in the scenario the param was not provided it will send *500* messages.

The current message sent has limited concern in the cardinality of the individual messages, as the aim and focus was to load ADX with multiple concurrent events.

### PumpSocialEvents - WIP
The function would push social events (tweets) to a designated event hub, it will send either according to a *bulksize* parameter passed. If a prameter was not passed it would use a default bulk size which is *500* messages.

### PumpIt - Logic App
The logic app, is configured as a recurring trigger. In the first scenario where the **PumpEvents** function is used the logic app, was configured to be triggered on every second. It then spawn 4-6 concurrent calls to the function ommiting 2000-3000 events per sec.

![Logic App](https://user-images.githubusercontent.com/37622785/50053150-6e30d080-0138-11e9-9f94-a448443a9ed8.png)
