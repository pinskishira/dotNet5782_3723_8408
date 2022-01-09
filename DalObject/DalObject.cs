using System;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
        internal static DalObject Instance { get { return instance.Value; } }
        private static readonly Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject());
        static DalObject() { }//static ctor to ensure instance init is done just before first usage
        private DalObject()
        {
            DataSource.Initialize();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] electricityUse()
        {
            double[] elecUse = new double[5];
            elecUse[0] = DataSource.Config.BatteryConsumptionPowerUsageEmpty;
            elecUse[1] = DataSource.Config.BatteryConsumptionLightWeight;
            elecUse[2] = DataSource.Config.BatteryConsumptionMediumWeight;
            elecUse[3] = DataSource.Config.BatteryConsumptionHeavyWeight;
            elecUse[4] = DataSource.Config.DroneChargingRatePH;
            return elecUse;
        }
    }
}