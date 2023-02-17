using Newtonsoft.Json;

namespace HuaweiSolar.Models.Huawei
{
    public class DevRealKpiStrInvDataItemMap : BaseDevRealKpiDataItemMap
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
        public double? pv9_u { get; set; } // PV9 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv10_u { get; set; } // PV10 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv11_u {get;set;} // PV11 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv12_u { get; set; } // PV12 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv13_u { get; set; } // PV13 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv14_u { get; set; } // PV14 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv15_u { get; set; } // PV15 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv16_u { get; set; } // PV16 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv17_u { get; set; } // PV17 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv18_u { get; set; } // PV18 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv19_u { get; set; } // PV19 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv20_u { get; set; } // PV20 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv21_u { get; set; } // PV21 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv22_u { get; set; } // PV22 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv23_u { get; set; } // PV23 input voltage (V)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv24_u { get; set; } // PV24 input voltage (V)

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
        public double? pv9_i { get; set; } // PV9 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv10_i { get; set; } // PV10 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv11_i {get;set;} // PV11 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv12_i { get; set; } // PV12 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv13_i { get; set; } // PV13 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv14_i { get; set; } // PV14 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv15_i { get; set; } // PV15 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv16_i { get; set; } // PV16 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv17_i { get; set; } // PV17 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv18_i { get; set; } // PV18 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv19_i { get; set; } // PV19 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv20_i { get; set; } // PV20 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv21_i { get; set; } // PV21 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv22_i { get; set; } // PV22 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv23_i { get; set; } // PV23 input current (A)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? pv24_i { get; set; } // PV24 input current (A)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? total_cap { get; set; } // Total yield (kWh)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public long? open_time { get; set; } // Inverter startup time (EPOCH ms)
        
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public long? close_time { get; set; } // Inverter startup time (EPOCH ms)

        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_total_cap { get; set; } // Total DC input energy (kWh)


        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_1_cap { get; set; } // MPPT 1 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_2_cap { get; set; } // MPPT 2 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_3_cap { get; set; } // MPPT 3 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_4_cap { get; set; } // MPPT 4 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_5_cap { get; set; } // MPPT 5 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_6_cap { get; set; } // MPPT 6 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_7_cap { get; set; } // MPPT 7 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_8_cap { get; set; } // MPPT 8 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_9_cap { get; set; } // MPPT 9 DC total yield (kWh)
        [JsonConverter(typeof(HuaweiInvalidNAValueConverter))]
        public double? mppt_10_cap { get; set; } // MPPT 10 DC total yield (kWh)

        public int run_state { get; set; } // State (0: Disconnected 1: Connected)
    }
}