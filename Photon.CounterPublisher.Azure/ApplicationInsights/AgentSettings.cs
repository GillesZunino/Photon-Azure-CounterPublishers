// -----------------------------------------------------------------------------------
// Copyright 2019, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Configuration;
using ExitGames.Diagnostics.Configuration;

namespace CounterPublisher.Azure.ApplicationInsights
{
    public class AgentSettings : CounterSampleSenderSettings
    {
        private static readonly string InstrumentationKeyAttributeName = "instrumentationKey";
        private static readonly string EndpointAttributeName = "endpoint";
        private static readonly string NamespaceAttributeName = "namespace";


        [ConfigurationProperty("endpoint", IsRequired = false)]
        public new Uri Endpoint
        {
            get { return (Uri)this[EndpointAttributeName]; }
            set { this[EndpointAttributeName] = value; }
        }

        [ConfigurationProperty("instrumentationKey", IsRequired = true)]
        public string InstrumentationKey
        {
            get { return (string)this[InstrumentationKeyAttributeName]; }
            set { this[InstrumentationKeyAttributeName] = value; }
        }

        [ConfigurationProperty("namespace", IsRequired = false, DefaultValue ="Photon")]
        public string NamespacePrefix
        {
            get { return (string)this[NamespaceAttributeName]; }
            set { this[NamespaceAttributeName] = value; }
        }

        public AgentSettings()
        {
        }

        public AgentSettings(CounterSampleSenderSettings settings) : base(settings)
        {
        }
    }
}
