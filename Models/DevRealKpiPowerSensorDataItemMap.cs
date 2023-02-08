using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DevRealKpiPowerSensorDataItemMap : BaseDevRealKpiDataItemMap
    {
        public long meter_status {get;set;} = -1; // 0: Offline, 1: normal (-1 means no data returned)

        public double meter_u { get; set; } // Phase A voltage (AC output)
        public double meter_i { get; set; } // Phase A current of grid (IA)

        public double active_power { get; set; } // Active Power Note: in Watts - this is the excess solar
        public double reactive_power { get; set; } // Reactive Power (Var)

        public double power_factor { get; set; } // Power Factor
        public double grid_frequency { get; set; } // Grid Frequency (Hz)

        public double active_cap { get; set; } // Active energy (positive active energy) (kWh)
        public double reverse_active_cap { get; set; } // Negative active energy  (kWh)

        public long run_state { get; set; } // State 0: disconnected, 1: connected

        public double ab_u { get; set; } // A-B line voltage of grid (V)
        public double bc_u { get; set; } // B-C line voltage of grid (V)
        public double ca_u { get; set; } // C-A line voltage of grid (V)
        public double b_u { get; set; } // Phase B voltage (AC output)
        public double c_u { get; set; } // Phase C voltage (AC output)
        public double b_i { get; set; } // Phase B current of grid (IB)
        public double c_i { get; set; } // Phase C current of grid (IC)

        public double forward_reactive_cap { get; set; } // Positive reactive energy (kWh)
        public double reverse_reactive_cap { get; set; } // Negative reactive energy (kWh)

        public double active_power_a { get; set; } // Active power PA (kW)
        public double active_power_b { get; set; } // Active power PB (kW)
        public double active_power_c { get; set; } // Active power PC (kW)

        public double reactive_power_a { get; set; } // Reactive power QA (kVar)
        public double reactive_power_b { get; set; } // Reactive power QB (kVar)
        public double reactive_power_c { get; set; } // Reactive power QC (kVar)

        public double total_apparent_power { get; set; } // Total apparent power (kVA)

        public double reverse_active_peak { get; set; } // Negative active energy (peak) (kWh)
        public double reverse_active_power { get; set; } // Negative active energy (shoulder) (kWh)
        public double reverse_active_valley { get; set; } // Negative active energy (off-peak) (kWh)
        public double reverse_active_top { get; set; } // Negative active energy (sharp) (kWh)

        public double positive_active_peak { get; set; } // Positive active energy (peak) (kWh)
        public double positive_active_power { get; set; } // Positive active energy (shoulder) (kWh)
        public double positive_active_valley { get; set; } // Positive active energy (off-peak) (kWh)
        public double positive_active_top { get; set; } // Positive active energy (sharp) (kWh)

        public double reverse_reactive_peak { get; set; } // Negative reactive energy (peak) (kVar)
        public double reverse_reactive_power { get; set; } // Negative reactive energy (shoulder) (kVar)
        public double reverse_reactive_valley { get; set; } // Negative reactive energy (off-peak) (kVar)
        public double reverse_reactive_top { get; set; } // Negative reactive energy (sharp) (kVar)

        public double positive_reactive_peak { get; set; } // Positive reactive energy (peak) (kVar)
        public double positive_reactive_power { get; set; } // Positive reactive energy (shoulder) (kVar)
        public double positive_reactive_valley { get; set; } // Positive reactive energy (off-peak) (kVar)
        public double positive_reactive_top { get; set; } // Positive reactive energy (sharp) (kVar)
    }
}