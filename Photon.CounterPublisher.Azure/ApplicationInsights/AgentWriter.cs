// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using ExitGames.Diagnostics.Monitoring;
using ExitGames.Diagnostics.Monitoring.Protocol;

namespace CounterPublisher.Azure.ApplicationInsights
{
    public class AgentWriter : ICounterSampleWriter
    {
        private bool disposed = false;

        private readonly AgentSettings applicationInsightsAgentSettings;
        private CounterSampleSenderBase counterSampleSender;


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

            // TODO: Initialize ApplicationInsights
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
                // TODO: Publish to ApplicationInsights with Server Channel
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