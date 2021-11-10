using System;
using DAL;
using DalObject;
using IDAL;
using IBL;
using System.Collections.Generic;
using BL;
using DAL.IDAL.DO;
    
namespace BL
{
    public class BL
    {
        public BL()
        {
            IDal dalObject = new DalObject.DalObject();
            double[] elecUse = dalObject.electricityUse();
            double Available = elecUse[0];
            double LightWeight = elecUse[01];
            double MediumWeight = elecUse[2];
            double HeavyWeight = elecUse[3];
            double DroneLoadingRate = elecUse[4];
        }
    }
}
