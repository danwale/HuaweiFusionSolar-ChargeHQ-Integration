using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DevRealKpiResInvDataItemMap : BaseDevRealKpiDataItemMap
    {
        public double inverter_state { get; set; } = -1; // Inverter State, -1 indicates bad data from the API

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ab_u { get; set; } // A-B line voltage of grid (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? bc_u { get; set; } // B-C line voltage of grid (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? ca_u { get; set; } // C-A line voltage of grid (V)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? a_u { get; set; } // Phase A voltage (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? b_u { get; set; } // Phase B voltage (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? c_u { get; set; } // Phase C voltage (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? a_i { get; set; } // Phase A current of grid (A))

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? b_i { get; set; } // Phase B current of grid (A))

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? c_i { get; set; } // Phase C current of grid (A))


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? efficiency { get; set; } // Inverter conversion efficiency (manufacturer) (%)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? temperature { get; set; } // Internal temperature (deg C)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? power_factor { get; set; } // Power factor

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? elec_freq { get; set; } // Grid frequency (Hz)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? active_power { get; set; } // Active power (kW) - this is what is being produced now

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? reactive_power { get; set; } //Output reactive power (kVar)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? day_cap { get; set; } // Yield today (kWh)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_power { get; set; } // MPPT Total input power (kW)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv1_u {get;set;} // PV1 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv2_u { get; set; } // PV2 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv3_u { get; set; } // PV3 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv4_u { get; set; } // PV4 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv5_u { get; set; } // PV5 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv6_u { get; set; } // PV6 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv7_u { get; set; } // PV7 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv8_u { get; set; } // PV8 input voltage (V)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv1_i {get;set;} // PV1 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv2_i { get; set; } // PV2 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv3_i { get; set; } // PV3 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv4_i { get; set; } // PV4 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv5_i { get; set; } // PV5 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv6_i { get; set; } // PV6 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv7_i { get; set; } // PV7 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv8_i { get; set; } // PV8 input current (A)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? total_cap { get; set; } // Total yield (kWh)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public long? open_time { get; set; } // Inverter startup time (EPOCH ms)
        
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public long? close_time { get; set; } // Inverter startup time (EPOCH ms)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_1_cap { get; set; } // MPPT 1 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_2_cap { get; set; } // MPPT 2 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_3_cap { get; set; } // MPPT 3 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_4_cap { get; set; } // MPPT 4 DC total yield (kWh)

        public int run_state { get; set; } // State (0: Disconnected 1: Connected)
    }
}