using System;
using System.Text;

namespace nanoFramework.HomeAssistant.MqttDiscovery.Items
{
    public class Switch : HomeAssistantItem
    {
        internal Switch(HomeAssistant homeAssistant, string switchName, string initialState)
            : base(homeAssistant, initialState)
        {
            this.switchName = switchName;
        }

        public override string GetDiscoveryTopic() => $"homeassistant/switch/{homeAssistant.DeviceName.Replace(" ", "-")}/{switchName.Replace(" ", "-")}/config";
        public override string GetCommandTopic() => $"nanoframework/switch/{homeAssistant.DeviceName.Replace(" ", "-")}/{switchName.Replace(" ", "-")}/set";
        public override string GetStateTopic() => $"nanoframework/switch/{homeAssistant.DeviceName.Replace(" ", "-")}/{switchName.Replace(" ", "-")}/state";

        public override string ToDiscoveryMessage()
        {
            return "{"
                + "\"name\": \"" + switchName + "\","
                + "\"unique_id\": \"" + homeAssistant.DeviceName.Replace(" ", "-") + "-" + switchName.Replace(" ", "-") + "-switch\","
                + "\"state_topic\": \"" + GetStateTopic() + "\","
                + "\"command_topic\": \"" + GetCommandTopic() + "\","
                + "\"availability_topic\": \"" + homeAssistant.GetAvailabilityTopic() + "\","
                + "\"device\": { \"identifiers\": [ \"" + homeAssistant.DeviceName + "\" ], \"name\": \"" + homeAssistant.DeviceName + "\" }"
                + "}";
        }

        private readonly string switchName;
    }
}