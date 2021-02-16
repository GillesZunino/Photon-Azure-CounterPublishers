// -----------------------------------------------------------------------------------
// Copyright 2021, Gilles Zunino
// -----------------------------------------------------------------------------------

using System;
using System.Globalization;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace CounterPublisher.Azure.ApplicationInsights
{
    public class CloudTelemetryInitializer : ITelemetryInitializer
    {
        private AgentSettings AgentSettings { get; set; }

        public CloudTelemetryInitializer(AgentSettings agentSettings, )
        {
            AgentSettings = agentSettings;
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleInstance = Environment.MachineName;
            telemetry.Context.Cloud.RoleName = string.Format(CultureInfo.InvariantCulture, AgentSettings.SenderId, Environment.MachineName);
        }
    }
}
