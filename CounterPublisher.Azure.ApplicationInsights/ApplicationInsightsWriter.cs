// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using ExitGames.Diagnostics.Monitoring;
using ExitGames.Diagnostics.Monitoring.Protocol;

namespace CounterPublisher.Azure
{
    public class ApplicationInsightsWriter : ICounterSampleWriter
    {
        private bool disposed = false;

        private CounterSampleSenderBase counterSampleSender;


        public bool Ready => throw new NotImplementedException();

        public void Start(CounterSampleSenderBase sender)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("Resource was disposed.");
            }

            if (counterSampleSender != null)
            {
                throw new InvalidOperationException("Already started(), can't call for second time");
            }

            counterSampleSender = sender;

            //if (counterSampleSender.SendInterval < MinSendInterval)
            //{
            //    throw new ArgumentOutOfRangeException("sender", "sender.SendInterval is out of range. Min value is " + MinSendInterval);
            //}

            // TODO: Initialize ApplicationInsights
        }

        public void Publish(CounterSampleCollection[] packages)
        {
            if (disposed)
            {
                throw new ObjectDisposedException("Resource was disposed.");
            }

            if (!Ready)
            {
                counterSampleSender.RaiseOnDisconnetedEvent();
                return;
            }

            // TODO: Publish to ApplicationInsights with Server Channel
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
    }
}