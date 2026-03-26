using System;
using System.Text;

namespace nanoFramework.HomeAssistant.MqttDiscovery.Items
{
    public class Option : HomeAssistantItem
    {
        private readonly string optionName;
        private readonly string[] options;

        internal Option(HomeAssistant homeAssistant, string optionName, string[] options, string initialState) : base(homeAssistant, initialState)
        {
            this.homeAssistant = homeAssistant;
            this.optionName = optionName;
            this.options = options;
        }

        public override string GetDiscoveryTopic() => $"homeassistant/select/{homeAssistant.DeviceName.Replace(" ", "-")}/{optionName.Replace(" ", "-")}/config";
        public override string GetCommandTopic() => $"nanoframework/select/{homeAssistant.DeviceName.Replace(" ", "-")}/{optionName.Replace(" ", "-")}/set";
        public override string GetStateTopic() => $"nanoframework/select/{homeAssistant.DeviceName.Replace(" ", "-")}s/{optionName.Replace(" ", "-")}/state";

        public override string ToDiscoveryMessage()
        {
            return "{"
                + "\"name\": \"" + optionName + " switch\","
                + "\"unique_id\": \"" + homeAssistant.DeviceName.Replace(" ", "-") + "-" + optionName.Replace(" ", "-") + "-option\","
                + "\"state_topic\": \"" + GetStateTopic() + "\","
                + "\"command_topic\": \"" + GetCommandTopic() + "\","
                + "\"options\": [ \"" + StringExtentionMethods.Join("\", \"", options) + "\" ],"
                + "\"availability_topic\": \"" + homeAssistant.GetAvailabilityTopic() + "\","
                + "\"device\": { \"identifiers\": [ \"" + homeAssistant.DeviceName + "\" ], \"name\": \"" + homeAssistant.DeviceName + "\" }"
                + "}";
        }
    }
}