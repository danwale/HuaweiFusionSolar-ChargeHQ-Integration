using Newtonsoft.Json;

namespace HuaweiSolar.Models.Huawei
{
    public class DevRealKpiBattDataItemMap : BaseDevRealKpiDataItemMap
    {
        public double battery_status { get; set; } = -1; // Battery State, -1 indicates bad data from the API
        // 0: Offline, 1: Standby, 2: Running, 3: Faulty, 5: Hibernating

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? max_charge_power { get; set; } // Maximum Charging Power (W)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? max_discharge_power { get; set; } // Maximum Discharging Power (W)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ch_discharge_power { get; set; } // Charge/Discharge Power (W)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? busbar_u { get; set; } // Battery Voltage (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? battery_soc { get; set; } // Battery SOC (%)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? battery_soj { get; set; } // Battery SOH (None)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ch_discharge_model { get; set; } // Charge/Discharge mode 
        // 0: none, 1: forced charge/discharge, 2: time-of-use price, 3: fixed charge/discharge, 4: automatic charge/discharge

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? charge_cap { get; set; } // Charged Energy (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? discharge_cap { get; set; } // Discharged Energy (kWh)

        public int run_state { get; set; } // State (0: Disconnected 1: Connected)
    }
}