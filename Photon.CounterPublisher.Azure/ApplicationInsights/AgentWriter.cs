// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using ExitGames.Diagnostics.Counter;
using ExitGames.Diagnostics.Monitoring;
using ExitGames.Diagnostics.Monitoring.Protocol;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace CounterPublisher.Azure.ApplicationInsights
{
    public class AgentWriter : ICounterSampleWriter
    {
        private static readonly char[] SplitCharacters = new char[] { '.' };

        private bool disposed = false;

        private readonly AgentSettings applicationInsightsAgentSettings;
        private CounterSampleSenderBase counterSampleSender;

        private TelemetryClient telemetryClient;

        public bool Ready { get; protected set; }

        public AgentWriter(AgentSettings settings)
        {
            Ready = true;

            applicationInsightsAgentSettings = settings;
        }

        public void Start(CounterSampleSenderBase sender)
        {
            ThrowIfDisposed();

            if (counterSampleSender != null)
            {
                throw new InvalidOperationException("ApplicationInsights.AgentWriter.Start() has already been called");
            }

            counterSampleSender = sender;

            // TODO: Initialize ApplicationInsights correctly from configuration file
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = applicationInsightsAgentSettings.InstrumentationKey;
            telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public void Publish(CounterSampleCollection[] packages)
        {
            ThrowIfDisposed();

            if (!Ready)
            {
                counterSampleSender.RaiseOnDisconnetedEvent();
            }
            else
            {
                PublishPackages(packages);
            }
        }

        private void PublishPackages(CounterSampleCollection[] packages)
        {
            foreach (CounterSampleCollection package in packages)
            {
                foreach (CounterSample counterSample in package)
                {
                    // TODO: Investigate the use of Metric Aggregation facilities as opposed to telemetryClient.TrackMetric()
                    //MetricIdentifier metricIdentifer = new MetricIdentifier(metricNamespace, metricId);
                    //Metric metric = telemetryClient.GetMetric(metricIdentifer);
                    //metric.TrackValue();

                    MetricTelemetry metricTelemetry = new MetricTelemetry();
                    metricTelemetry.Name = package.CounterName;
                    metricTelemetry.Sum = counterSample.Value;
                    metricTelemetry.Count = 1;
                    metricTelemetry.Timestamp = counterSample.Timestamp;

                    telemetryClient.TrackMetric(metricTelemetry);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (counterSampleSender != null)
                    {
                        counterSampleSender.Dispose();
                        counterSampleSender = null;
                    }
                }

                disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(string.Empty);
            }
        }
    }
}