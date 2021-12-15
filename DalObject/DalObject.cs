using DalApi;

namespace Dal
{
    partial class DalObject :IDal
    {
        internal static DalObject Instance { get; } = new DalObject();
        static DalObject() { }
        private DalObject()
        {
            DataSource.Initialize();
        }

        public double[] electricityUse()
        {
            double []elecUse = new double[5];
            elecUse[0] = DataSource.Config.BatteryConsumptionPowerUsageEmpty;
            elecUse[1] = DataSource.Config.BatteryConsumptionLightWeight;
            elecUse[2] = DataSource.Config.BatteryConsumptionMediumWeight;
            elecUse[3] = DataSource.Config.BatteryConsumptionHeavyWeight;
            elecUse[4] = DataSource.Config.DroneChargingRatePH;
            return elecUse;
        }
    }
}