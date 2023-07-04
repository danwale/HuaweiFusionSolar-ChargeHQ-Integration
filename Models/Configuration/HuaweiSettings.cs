namespace HuaweiSolar.Models.Configuration
{
    public class HuaweiSettings
    {
        // This is the domain used for the Huawei FusionSolar API, you should pick the one where your data is being sent by your inverter.
        // In Australia this is often https://intl.fusionsolar.huawei.com/ but it could also be https://sg5.fusionsolar.huawei.com/ from what I've seen.
        public string BaseURI
        {
            get; set;
        }

        // This is the username for the system account Huawei provided or the username provided by the installer for the Northbound API Account they created
        public string Username 
        {
            get; set;
        }

        // This is the systemCode that Huawei provided or the password provided by the installer for the Northbound API Account they created
        public string Password 
        {
            get; set;
        }

        // Also referred to as the Plant Name in FusionSolar, they renamed it at some stage but they mean the same thing
        public string StationName 
        {
            get; set;
        }

        // This is the poll rate in minutes, Huawei ask for this to be 5 minutes so it's hardcoded to this
        public int PollRate
        {
            get
            {
                return 5;
            }
        }

        // This will default to true, but if a grid meter is present and a power sensor is not present this can turn on/off the data from the grid meter being queried and used.
        // Note: If a Power Sensor device is present it will be used in favour of the Grid Meter unless UsePowerSensorData is set to false.
        public bool UseGridMeterData
        {
            get; set;
        } = true;

        // This will default to true, but if a power sensor is present this can turn on/off the data from the power sensor being queried and used.
        public bool UsePowerSensorData
        {
            get; set;
        } = true;

        // This will default to true, but if a battery is present this can turn on/off the data from the battery being queried and used.
        public bool UseBatteryData
        {
            get; set;
        } = true;
    }
}