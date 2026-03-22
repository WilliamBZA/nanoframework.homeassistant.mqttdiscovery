using nanoFramework.M2Mqtt;
using nanoFramework.M2Mqtt.Messages;
using nanoFramework.HomeAssistant.MqttDiscovery.Items;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace nanoFramework.HomeAssistant.MqttDiscovery
{
    public class HomeAssistant : IDisposable
    {
        public HomeAssistant(string deviceName, string brokerIp, int port = 1883, string username = "", string password = "")
        {
            DeviceName = deviceName;
            this.brokerIp = brokerIp;
            this.port = port;
            this.username = username;
            this.password = password;

            items = new ArrayList();

            AddLastUpdatedItem();
        }

        private void AddLastUpdatedItem()
        {
            var sensor = AddSensor("Last Updated", "", DateTime.UtcNow.ToString("o"), DeviceClass.Timestamp);

            lastUpdatedTimer = new Timer((state) =>
            {
                ValidateConnection();
                sensor.UpdateValue(DateTime.UtcNow.ToString("o"));
            }, null, 0, 60000);
        }

        public string DeviceName { get; private set; }

        public void Connect()
        {
            MqttReasonCode retCode;

            do
            {
                client = new MqttClient(brokerIp, port, false, null, null, MqttSslProtocols.None);

                client.MqttMsgPublishReceived += (sender, e) =>
                {
                    var message = Encoding.UTF8.GetString(e.Message, 0, e.Message.Length);
                    Console.WriteLine($"Received message on topic {e.Topic}: {message}");

                    foreach (HomeAssistantItem item in items)
                    {
                        if (item.GetCommandTopic() == e.Topic)
                        {
                            item.SetState(message);
                        }
                    }
                };

                client.ConnectionClosed += (sender, e) =>
                {
                    Console.WriteLine("MQTT connection closed. Reconnecting...");
                    Connect();
                };

                retCode = client.Connect(DeviceName, username, password);
            } while (retCode != MqttReasonCode.Success);

            Console.WriteLine("Connected to MQTT Broker");

            PublishAutoDiscovery();
        }

        public void PublishAutoDiscovery()
        {
            foreach (HomeAssistantItem item in items)
            {
                if (!string.IsNullOrEmpty(item.GetCommandTopic()))
                {
                    client.Subscribe(new[] { item.GetCommandTopic() }, new[] { MqttQoSLevel.AtLeastOnce });
                }

                var topic = item.GetDiscoveryTopic();
                var message = item.ToDiscoveryMessage();

                Console.WriteLine($"Publishing to '{topic}': {message}");

                client.Publish(topic, Encoding.UTF8.GetBytes(message), null, null, MqttQoSLevel.AtMostOnce, true);
                Console.WriteLine($"Published Auto Discovery for {topic}");
            }

            Thread.Sleep(10);
            foreach (HomeAssistantItem item in items)
            {
                var topic = item.GetAvailabilityTopic();
                client.Publish(topic, Encoding.UTF8.GetBytes("online"), null, null, MqttQoSLevel.AtMostOnce, true);
                Console.WriteLine($"Publishing to topic '{topic}' with value: 'online'");

                client.Publish(item.GetStateTopic(), Encoding.UTF8.GetBytes(item.GetState()), null, null, MqttQoSLevel.AtLeastOnce, true);
            }
        }

        public void Dispose()
        {
            client?.Dispose();
        }

        internal void StateChanged(HomeAssistantItem item, string state)
        {
            ValidateConnection();

            Console.WriteLine($"Publishing to topic '{item.GetStateTopic()}' with value: '{state}'");
            client.Publish(item.GetStateTopic(), Encoding.UTF8.GetBytes(state), null, null, MqttQoSLevel.AtMostOnce, true);
        }

        public Option AddOption(string name, string[] options, string initialState)
        {
            var option = new Option(this, name, options, initialState);
            items.Add(option);

            return option;
        }

        public Switch AddSwitch(string name, string initialState)
        {
            var @switch = new Switch(this, name, initialState);
            items.Add(@switch);

            return @switch;
        }

        public Sensor AddSensor(string sensorName, string unitOfMeasurement, string initialState, DeviceClass deviceClass)
        {
            var sensor = new Sensor(this, sensorName, unitOfMeasurement, initialState, deviceClass);
            items.Add(sensor);

            return sensor;
        }

        public TextItem AddTextItem(string label, string initialState)
        {
            var textItem = new TextItem(this, label, initialState);
            items.Add(textItem);

            return textItem;
        }

        private void ValidateConnection()
        {
            if (client.IsConnected)
            {
                return;
            }

            Connect();
        }

        MqttClient client;

        private string brokerIp;
        private int port;
        private string username;
        private string password;

        private ArrayList items;
        private Timer lastUpdatedTimer;
    }
}