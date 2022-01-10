using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using static BO.Enum;

namespace BL.BlApi
{
    class Simulation
    {
        enum Maintenance { Starting, Going, Charging }
        private const int Delay = 500;
        private const double SpeedTime = 1;
        private const double TimeStep = Delay / 1000.00;
        private const double Step = SpeedTime / TimeStep;
        public Simulation(BL ibl, int droneId, Action action, Func<bool> stop)
        {
            var bl = ibl;
            var dal = bl.dal;
            Drone drone = bl.GetDrone(droneId);
            int parcelId = 0;
            int stationId = 0;
            Station station = null;
            Customer customer = null;
            double distance = 0.0;
            int batteryUsage = 0;
            DO.Parcel parcel = default;
            bool pickUp = false;
            Maintenance maintenance = Maintenance.Starting;

            void Deliver(int idDrone)
            {
                //parcel = dal.FindParcel(idDrone);
                //batteryUsage = dal.electricityUse()[pa]

            }

            do
            {
                switch (drone.DroneStatus)
                {
                    case DroneStatuses.Available:
                        lock(bl)
                        {
                            parcelId = dal.GetAllParcels(p=> p?.Scheduled == null
                            && (WeightCategories)(p?.Weight) <= drone.Weight
                            && drone.bat
                        }
                        break;
                    case DroneStatuses.Maintenance:
                        switch(maintenance)
                        {
                            case Maintenance.Starting:
                                lock(bl)
                                {
                                    bs=bl.GetStation()
                                    distance=drone
                                }
                                break;
                            case Maintenance.Going:
                                lock (bl)
                                {
                                    distance = drone
                                }
                                break;

                        }

                        break;


                }
            }while()

            //while (stop())
            //{

            //    //try
            //    //{
            //    //    switch (drone.Status)
            //    //    {
            //    //        case DroneStatus.available:
            //    //            bl.ScheduledAParcelToADrone(drone.Id);
            //    //            break;
            //    //        case DroneStatus.inFix:
            //    //            if (drone.Battery == 100)
            //    //                bl.FreeDroneFromeCharger(drone.Id);
            //    //            break;
            //    //        case DroneStatus.delivery:
            //    //            if (drone.ParcelInTransfer.StatusParcel == false)
            //    //                bl.PickUpParcel(drone.Id);
            //    //            else
            //    //                bl.DeliverParcel(drone.Id);
            //    //            break;
            //    //    }
            //    //}
            //    //catch (FailToUpdateException ex)
            //    //{
            //    //    if (drone.Battery != 100)
            //    //        bl.SendDroneToCharging(drone.Id);
            //    //    throw;
            //    //}
            //    }
           
          
     }


}

