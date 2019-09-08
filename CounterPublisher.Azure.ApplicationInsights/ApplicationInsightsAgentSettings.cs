// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System.Configuration;
using ExitGames.Diagnostics.Configuration;

namespace CounterPublisher.Azure
{
    public class ApplicationInsightsAgentSettings : CounterSampleSenderSettings
    {
        [ConfigurationProperty("Instrumentation Key", IsRequired = true)]
        public string InstrumentationKey
        {
            get { return (string)this["InstrumentationKey"]; }
            set { this["InstrumentationKey"] = value; }
        }
    }
}
