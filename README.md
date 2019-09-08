# Introduction
Photon is a game networking engine and multiplayer platform developed and licensed by Exit Games. According to the [company's web site](https://www.photonengine.com/), Photon is used by various developers and studios, including Disney, Ubisoft and Oculus.

This repository enables Photon's CounterPublisher infrastructure to publish server performance and statistics to Azure. 

**NOTE**: Exit Games Photon is not free software. At the time of writting, Exit Games offers an evaluation version of Photon Self Hosted Server limited to 20 simultaneous users.

# Installation

1. Clone this repository at the root directory of your Photon Server SDK. The directory structure should be as follows:

    ```
        <root>
            | Photon
            |     | build
            |     | deploy
            |     | doc
            |     | lib
            |     | src-server
            |     | Photon-Azure-CounterPublishers
            |     |     | Photon.CounterPublisher.Azure.sln
            |     |     | Photon.CounterPublisher.Azure
            |     |     | CounterPublisher.dll.config
            |     |     | LICENSE
            |     |     | README.md
    ```
2. Using Visual Studio, open "Photon-Azure-CounterPublishers\Photon.CounterPublisher.Azure.sln".
3. Build the solution. All plugin files will be located under Photon.CounterPublisher.Azure\bin\{Debug|Release}. "Release" should be preferred for maximuim performances.

# Configuration
You will need an Application Insights resource in an active Azure Subscription. Instruction to create a new Application Insight resource can be bound [here](https://docs.microsoft.com/en-us/azure/azure-monitor/app/create-new-resource).

1. Copy the Application Insight Instrumentation Key from the Azure portal - This will ressemble a GUID (for example 8323fb13-32aa-46af-b467-8355cf4f8f98). We refer to as this 
2. Copy the content of Photon.CounterPublisher.Azure\bin\Debug to:
* "deploy\CounterPublisher\bin"
* "deploy\Loadbalancing\GameServer\bin"
* "deploy\Loadbalancing\Master\bin"

3. Configure CounterPublisher, LoadBalancing (Master) and  LoadBalancing (Game) to use the plugin to pubish server performances and statistics in the following files:

* "deploy\CounterPublisher\bin\CounterPublisher.dll.config"
* "deploy\Loadbalancing\GameServer\bin\Photon.LoadBalancing.dll.config"
* "deploy\Loadbalancing\Master\bin\Photon.LoadBalancing.dll.config"

These files will already have a <Photon> <CounterPublisher .../> </Photon> section. 

Replace {#InstrumentationKey#} with the instrumentation key obtained during step 1 
Replace {#Region#}.{#Cluster#}.{0} with any string representative of the Photon deployment. The '{0}' is replaced by the server name at runtime.

```xml
  <Photon>
    <CounterPublisher
            senderType="CounterPublisher.Azure.ApplicationInsights.AgentSettings, CounterPublisher.Azure"
            enabled="true"
            addDefaultAppCounter="true"
            updateInterval="10">
        <Sender protocol="CounterPublisher.Azure.ApplicationInsights.AgentWriter, CounterPublisher.Azure"

            initialDelay="10"
            sendInterval="10"
            maxQueueLength="120"
            maxRetryCount="-1"

            senderId="{#Region#}.{#Cluster#}.{0}"
            instrumentationKey = "{#InstrumentationKey#}" />
    </CounterPublisher>
  </Photon>
```

# Azure Application Insights
TODO: Explain how to consume metrics

# TODO
* Should we auto namespace or not?
* Should we add dimensions to metrics?
* Add Metrics (Log Analytics) as an alternative
* Add Agent Id too - not yet communicated
* Pass agent name / id as part of docker variables