# Introduction
Photon is a game networking engine and multiplayer platform developed and licensed by Exit Games. According to the [company's web site](https://www.photonengine.com/), Photon is used by various developers and studios, including Disney, Ubisoft and Oculus.

This repository enables Photon's CounterPublisher infrastructure to publish server performance and statistics to Azure. 

**NOTE**: Exit Games Photon is not free software. At the time of writting, Exit Games offers an evaluation version of Photon Self Hosted Server limited to 20 simultaneous users.

# Installation

1. Clone this repository at the root of your Photon Server SDK. The directory structure should be as follows:

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
2. Open `Photon.CounterPublisher.Azure.sln` in Visual Studio,
3. Build the solution. The output will be located under `Photon.CounterPublisher.Azure\bin\{Debug|Release}`,
4. Copy the content of `Photon.CounterPublisher.Azure\bin\{Debug|Release}` to the following directories:
   * `deploy\CounterPublisher\bin`
   * `deploy\Loadbalancing\GameServer\bin`
   * `deploy\Loadbalancing\Master\bin`

# Configuration
Before performance counters can be published, Photon needs to be configured to use the plugin. The following steps show how to configure the standard LoadBalancing application (Master and GameServer).

1. Create a new Application Insight resource (see [this page](https://docs.microsoft.com/en-us/azure/azure-monitor/app/create-new-resource) for additional details) and copy the instrumentation key,

2. Prepare the configuration section. The following snippet contains two placeholders which need to be replaced:
      * `{#InstrumentationKey#}` with the Application Insights instrumentation key acquired during step 1,
      * `{#Region#}.{#Cluster#}` with a suitable string to identify the Photon server instance. `{0}` will be replaced with the machine name.
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
4. Insert the configuration section prepared in the previous steps in all the following files: 

   * `deploy\CounterPublisher\bin\CounterPublisher.dll.config`
   * `deploy\Loadbalancing\GameServer\bin\Photon.LoadBalancing.dll.config`
   * `deploy\Loadbalancing\Master\bin\Photon.LoadBalancing.dll.config`

    Each configuration file above already contains a `<Photon><CounterPublisher .../></Photon>` which can be relaced or augmented.

5. Start Photon. Metrics should become visible in Application Insights within a few minutes.  

# Azure Application Insights
TODO: Explain how to consume metrics

# TODO

* Describe more precisely addDefaultAppCounter, enabled, updateInterval, ... and all other attributes
* Consider adding dimensions to metrics?
* Add Metrics (Log Analytics) as an alternative to APplication Insights
* Pass sendierId to Application Insights