// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Text;
using ExitGames.Diagnostics.Counter;
using ExitGames.Diagnostics.Monitoring;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace CounterPublisher.Azure.ApplicationInsights
{
    internal class Agent
	{
        private static readonly char[] SplitCharacters = new char[] { '.' };

        private TelemetryClient telemetryClient;

        public void Initialize(AgentSettings agentSettings)
        {
            // TODO: Initialize ApplicationInsights correctly from configuration file
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = agentSettings.InstrumentationKey;
            telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public void Publish(CounterSampleCollection[] packages)
        {
            foreach (CounterSampleCollection package in packages)
            {
                // Split metric into a namespace and a counter anme - This will make it easier to find the counter in Application Insights
                (string metricNamespace, string metricName) = GetMetricDefinition(package.CounterName);

                foreach (CounterSample counterSample in package)
                {
                    // TODO: Investigate the use of Metric Aggregation facilities as opposed to telemetryClient.TrackMetric()
                    //MetricIdentifier metricIdentifer = new MetricIdentifier(metricNamespace, metricId);
                    //Metric metric = telemetryClient.GetMetric(metricIdentifer);
                    //metric.TrackValue();

                    MetricTelemetry metricTelemetry = new MetricTelemetry();
                    metricTelemetry.Name = metricName;

                    if (!string.IsNullOrEmpty(metricNamespace))
                    {
                        metricTelemetry.MetricNamespace = metricNamespace;
                    }
                    
                    metricTelemetry.Sum = counterSample.Value;
                    metricTelemetry.Count = 1;
                    metricTelemetry.Timestamp = counterSample.Timestamp;

                    telemetryClient.TrackMetric(metricTelemetry);
                }
            }
        }

        private static (string metricNamespace, string metricName) GetMetricDefinition(string counterName)
        {
            string[] metricNameSplit = counterName.Split(SplitCharacters, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < metricNameSplit.Length - 1; index++)
            {
                if (index > 0) { stringBuilder.Append('.'); }
                stringBuilder.Append(metricNameSplit[index]);
            }

            return (stringBuilder.ToString(), metricNameSplit[metricNameSplit.Length - 1]);
        }
    }
}
