using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DevRealKpiPowerSensorDataItemMap : BaseDevRealKpiDataItemMap
    {
        public long meter_status {get;set;} = -1; // 0: Offline, 1: normal (-1 means no data returned)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? meter_u { get; set; } // Phase A voltage (AC output)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? meter_i { get; set; } // Phase A current of grid (IA)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_power { get; set; } // Active Power Note: in Watts - this is the excess solar

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reactive_power { get; set; } // Reactive Power (Var)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? power_factor { get; set; } // Power Factor

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? grid_frequency { get; set; } // Grid Frequency (Hz)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_cap { get; set; } // Active energy (positive active energy) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_active_cap { get; set; } // Negative active energy  (kWh)

        public long run_state { get; set; } // State 0: disconnected, 1: connected

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ab_u { get; set; } // A-B line voltage of grid (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? bc_u { get; set; } // B-C line voltage of grid (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ca_u { get; set; } // C-A line voltage of grid (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? b_u { get; set; } // Phase B voltage (AC output)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? c_u { get; set; } // Phase C voltage (AC output)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? b_i { get; set; } // Phase B current of grid (IB)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? c_i { get; set; } // Phase C current of grid (IC)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? forward_reactive_cap { get; set; } // Positive reactive energy (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_reactive_cap { get; set; } // Negative reactive energy (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_power_a { get; set; } // Active power PA (kW)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_power_b { get; set; } // Active power PB (kW)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_power_c { get; set; } // Active power PC (kW)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reactive_power_a { get; set; } // Reactive power QA (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reactive_power_b { get; set; } // Reactive power QB (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reactive_power_c { get; set; } // Reactive power QC (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? total_apparent_power { get; set; } // Total apparent power (kVA)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_active_peak { get; set; } // Negative active energy (peak) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_active_power { get; set; } // Negative active energy (shoulder) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_active_valley { get; set; } // Negative active energy (off-peak) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_active_top { get; set; } // Negative active energy (sharp) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_active_peak { get; set; } // Positive active energy (peak) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_active_power { get; set; } // Positive active energy (shoulder) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_active_valley { get; set; } // Positive active energy (off-peak) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_active_top { get; set; } // Positive active energy (sharp) (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_reactive_peak { get; set; } // Negative reactive energy (peak) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_reactive_power { get; set; } // Negative reactive energy (shoulder) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_reactive_valley { get; set; } // Negative reactive energy (off-peak) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reverse_reactive_top { get; set; } // Negative reactive energy (sharp) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_reactive_peak { get; set; } // Positive reactive energy (peak) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_reactive_power { get; set; } // Positive reactive energy (shoulder) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_reactive_valley { get; set; } // Positive reactive energy (off-peak) (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? positive_reactive_top { get; set; } // Positive reactive energy (sharp) (kVar)
    }
}