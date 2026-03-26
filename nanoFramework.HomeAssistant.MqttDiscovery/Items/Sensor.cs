using System;
using System.Text;

namespace nanoFramework.HomeAssistant.MqttDiscovery.Items
{
    public class Sensor : HomeAssistantItem
    {
        internal Sensor(HomeAssistant homeAssistant, string sensorName, string unitOfMeasurement, string initialState, DeviceClass deviceClass = DeviceClass.None)
            : base(homeAssistant, initialState)
        {
            this.sensorName = sensorName;
            this.unitOfMeasurement = unitOfMeasurement;
            this.deviceClass = deviceClass;
        }

        public override string GetDiscoveryTopic() => $"homeassistant/sensor/{homeAssistant.DeviceName.Replace(" ", "-")}/{sensorName.Replace(" ", "-")}/config";
        public override string GetCommandTopic() => "";
        public override string GetStateTopic() => $"nanoframework/sensor/{homeAssistant.DeviceName.Replace(" ", "-")}/{sensorName.Replace(" ", "-")}/state";

        public override string ToDiscoveryMessage()
        {
            var discoveryPayload = "{"
                + "\"name\": \"" + sensorName + "\","
                + "\"unique_id\": \"" + homeAssistant.DeviceName.Replace(" ", "-") + "-" + sensorName.Replace(" ", "-") + "-sensor\","
                + "\"state_topic\": \"" + GetStateTopic() + "\","
                + "\"availability_topic\": \"" + homeAssistant.GetAvailabilityTopic() + "\",";

            if (!string.IsNullOrEmpty(unitOfMeasurement))
            {
                discoveryPayload += "\"unit_of_measurement\": \"" + unitOfMeasurement + "\",";
            }

            if (deviceClass != DeviceClass.None)
            {
                discoveryPayload += "\"device_class\": \"" + Sensor.ToDeviceClassString(deviceClass) + "\",";
            }

            discoveryPayload += "\"device\": { \"identifiers\": [ \"" + homeAssistant.DeviceName + "\" ], \"name\": \"" + homeAssistant.DeviceName + "\" }}";

            return discoveryPayload;
        }

        public static string ToDeviceClassString(DeviceClass e)
        {
            switch (e)
            {
                case DeviceClass.Absolute_Humidity: return "absolute_humidity";
                case DeviceClass.Apparent_Power: return "apparent_power";
                case DeviceClass.Aqi: return "aqi";
                case DeviceClass.Area: return "area";
                case DeviceClass.Atmospheric_Pressure: return "atmospheric_pressure";
                case DeviceClass.Battery: return "battery";
                case DeviceClass.Blood_Glucose_Concentration: return "blood_glucose_concentration";
                case DeviceClass.Carbon_Dioxide: return "carbon_dioxide";
                case DeviceClass.Carbon_Monoxide: return "carbon_monoxide";
                case DeviceClass.Current: return "current";
                case DeviceClass.Data_Rate: return "data_rate";
                case DeviceClass.Data_Size: return "data_size";
                case DeviceClass.Date: return "date";
                case DeviceClass.Distance: return "distance";
                case DeviceClass.Duration: return "duration";
                case DeviceClass.Energy: return "energy";
                case DeviceClass.Energy_Distance: return "energy_distance";
                case DeviceClass.Energy_Storage: return "energy_storage";
                case DeviceClass.Enum: return "enum";
                case DeviceClass.Frequency: return "frequency";
                case DeviceClass.Gas: return "gas";
                case DeviceClass.Humidity: return "humidity";
                case DeviceClass.Illuminance: return "illuminance";
                case DeviceClass.Irradiance: return "irradiance";
                case DeviceClass.Moisture: return "moisture";
                case DeviceClass.Monetary: return "monetary";
                case DeviceClass.Nitrogen_Dioxide: return "nitrogen_dioxide";
                case DeviceClass.Nitrogen_Monoxide: return "nitrogen_monoxide";
                case DeviceClass.Nitrous_Oxide: return "nitrous_oxide";
                case DeviceClass.Ozone: return "ozone";
                case DeviceClass.PH: return "ph";
                case DeviceClass.PM1: return "pm1";
                case DeviceClass.PM25: return "pm25";
                case DeviceClass.PM10: return "pm10";
                case DeviceClass.Power_Factor: return "power_factor";
                case DeviceClass.Power: return "power";
                case DeviceClass.Precipitation: return "precipitation";
                case DeviceClass.Precipitation_Intensity: return "precipitation_intensity";
                case DeviceClass.Pressure: return "pressure";
                case DeviceClass.Reactive_Energy: return "reactive_energy";
                case DeviceClass.Reactive_Power: return "reactive_power";
                case DeviceClass.Signal_Strength: return "signal_strength";
                case DeviceClass.Sound_Pressure: return "sound_pressure";
                case DeviceClass.Speed: return "speed";
                case DeviceClass.Sulphour_Dioxide: return "sulphour_dioxide";
                case DeviceClass.Temperature: return "temperature";
                case DeviceClass.Timestamp: return "timestamp";
                case DeviceClass.Volatile_Organic_Compounds: return "volatile_organic_compounds";
                case DeviceClass.Volatile_Organic_Compounds_Parts: return "volatile_organic_compounds_parts";
                case DeviceClass.Voltage: return "voltage";
                case DeviceClass.Volume: return "volume";
                case DeviceClass.Volume_Flow_Rate: return "volume_flow_rate";
                case DeviceClass.Volume_Storage: return "volume_storage";
                case DeviceClass.Water: return "water";
                case DeviceClass.Weight: return "weight";
                case DeviceClass.Wind_Direction: return "wind_direction";
                case DeviceClass.Wind_Speed: return "wind_speed";
                default: return "";
            }
        }

        public void UpdateValue(string value)
        {
            homeAssistant.StateChanged(this, value);
        }

        string sensorName;
        string unitOfMeasurement;
        DeviceClass deviceClass;
    }

    public enum DeviceClass
    {
        None,
        Absolute_Humidity,
        Apparent_Power,
        Aqi,
        Area,
        Atmospheric_Pressure,
        Battery,
        Blood_Glucose_Concentration,
        Carbon_Dioxide,
        Carbon_Monoxide,
        Current,
        Data_Rate,
        Data_Size,
        Date,
        Distance,
        Duration,
        Energy,
        Energy_Distance,
        Energy_Storage,
        Enum,
        Frequency,
        Gas,
        Humidity,
        Illuminance,
        Irradiance,
        Moisture,
        Monetary,
        Nitrogen_Dioxide,
        Nitrogen_Monoxide,
        Nitrous_Oxide,
        Ozone,
        PH,
        PM1,
        PM25,
        PM10,
        Power_Factor,
        Power,
        Precipitation,
        Precipitation_Intensity,
        Pressure,
        Reactive_Energy,
        Reactive_Power,
        Signal_Strength,
        Sound_Pressure,
        Speed,
        Sulphour_Dioxide,
        Temperature,
        Timestamp,
        Volatile_Organic_Compounds,
        Volatile_Organic_Compounds_Parts,
        Voltage,
        Volume,
        Volume_Flow_Rate,
        Volume_Storage,
        Water,
        Weight,
        Wind_Direction,
        Wind_Speed
    }
}