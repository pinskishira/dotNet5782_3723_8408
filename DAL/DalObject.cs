using System;
using IDAL.DO;
using System.Collections.Generic;
//using DAL.IDAL.DO;
using DalObject;
using IDAL;
/// <summary>
/// DalObject defines arrays for the stations, drones, customers, parcels and drone charges and then updates them and fills them with data.
/// It also includes adding functions for all arrays as well as searching functions.
/// </summary>
namespace DalObject
{
    public partial class DalObject :IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }

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