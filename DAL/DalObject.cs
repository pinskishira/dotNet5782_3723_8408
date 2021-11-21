using System;
using IDAL.DO;
using System.Collections.Generic;
//using DAL.IDAL.DO;
using DalObject;
using IDAL;

namespace DalObject
{
    public partial class DalObject :IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

        /// <summary>
        /// Defines an array that holds the data of the amount of battery used per km.
        /// </summary>
        /// <returns>Array of data</returns>
        public double[] electricityUse()
        {
            double []elecUse = new double[5];
            elecUse[0] = DataSource.Config.PowerUsageEmpty;
            elecUse[1] = DataSource.Config.LightWeight;
            elecUse[2] = DataSource.Config.MediumWeight;
            elecUse[3] = DataSource.Config.HeavyWeight;
            elecUse[4] = DataSource.Config.DroneChargingRatePH;
            return elecUse;
        }
    }
}