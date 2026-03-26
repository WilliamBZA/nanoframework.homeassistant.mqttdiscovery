using System;
using System.Text;

namespace nanoFramework.HomeAssistant.MqttDiscovery.Items
{
    public class TextItem : HomeAssistantItem
    {
        internal TextItem(HomeAssistant homeAssistant, string label, string initialState)
            : base(homeAssistant, initialState)
        {
            this.label = label;
        }

        public override string GetDiscoveryTopic() => $"homeassistant/text/{homeAssistant.DeviceName.Replace(" ", "-")}/{label.Replace(" ", "-")}/config";
        public override string GetCommandTopic() => $"nanoframework/text/{homeAssistant.DeviceName.Replace(" ", "-")}/{label.Replace(" ", "-")}/set";
        public override string GetStateTopic() => $"nanoframework/text/{homeAssistant.DeviceName.Replace(" ", "-")}/{label.Replace(" ", "-")}/state";

        public override string ToDiscoveryMessage()
        {
            return "{"
                + "\"name\": \"" + label + "\","
                + "\"unique_id\": \"" + homeAssistant.DeviceName.Replace(" ", "-") + "-" + label.Replace(" ", "-") + "-sensor\","
                + "\"state_topic\": \"" + GetStateTopic() + "\","
                + "\"command_topic\": \"" + GetCommandTopic() + "\","
                + "\"availability_topic\": \"" + homeAssistant.GetAvailabilityTopic() + "\","
                + "\"device\": { \"identifiers\": [ \"" + homeAssistant.DeviceName + "\" ], \"name\": \"" + homeAssistant.DeviceName + "\" }}";
        }

        string label;
    }
}