using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BL.BL;
using static BO.Enum;

namespace BL
{

    class Simulation
    {
        BL bl;
        private const int sleep = 1000;

        public Simulation(BL _bl, int droneID, Action Progress, Func<bool> stopSim)
        {
            DalApi.IDal Idal = DalApi.DLFactory.GetDL();
            bl = _bl;
            double distance;
            int tempBattery;

            DroneToList droneToList = bl.GetAllDrones().First(x => x.Id == droneID);

            while (!stopSim())
            {
                switch (droneToList.DroneStatus)
                {
                    case DroneStatuses.Available:
                        try
                        {
                            bl.UpdateAssignParcelToDrone(droneID);
                            Progress();
                        }
                        catch
                        {
                            if (droneToList.Battery < 100)
                            {
                                tempBattery = droneToList.Battery;
                                DO.Station station = bl.smallestDistanceFromDrone(droneToList.CurrentLocation);
                                distance = Distance.Haversine(droneToList.CurrentLocation.Longitude, droneToList.CurrentLocation.Latitude, station.Longitude, station.Latitude);
                                while (distance > 0)
                                {
                                    droneToList.Battery -= (int)bl.PowerUsageEmpty;//the drone is available
                                    Progress();
                                    distance -= 1;
                                    Thread.Sleep(sleep);
                                }
                                droneToList.Battery = tempBattery;//restarting the battery
                                bl.SendDroneToChargingStation(droneID);//here it will change it to the correct battery.
                                Progress();
                            }
                        }
                        break;
                    case DroneStatuses.Maintenance: 
                        bool flag = true;
                        while (droneToList.Battery < 100 && flag)
                        {
                            if (stopSim())
                                flag = false;
                            if (droneToList.Battery + (int)(bl.DroneChargingRatePH / 100) > 100)
                                bl.GetAllDrones().First(item => item.Id == droneToList.Id).Battery = 100;
                            else
                                bl.GetAllDrones().First(item => item.Id == droneToList.Id).Battery += (int)(bl.DroneChargingRatePH/100);
                            Progress();
                            Thread.Sleep(sleep);
                        }
                        if (flag == true)
                        {
                            bl.DroneReleaseFromChargingStation(droneID);
                            Progress();
                        }
                        break;
                    case DroneStatuses.Delivery:
                        Drone MyDrone = bl.GetDrone(droneID);
                        Parcel parcel = bl.GetParcel(MyDrone.ParcelInTransfer.Id);
                        if (parcel.PickedUp == null)
                        {
                            Customer sender = bl.GetCustomer(parcel.Sender.Id);
                            tempBattery = droneToList.Battery;
                            Location droneLocation = new Location { Longitude = droneToList.CurrentLocation.Longitude, Latitude = droneToList.CurrentLocation.Latitude };
                            distance = Distance.Haversine(droneLocation.Longitude, droneLocation.Latitude, sender.CustomerLocation.Longitude, sender.CustomerLocation.Latitude);
                            double latitude = Math.Abs((bl.GetCustomer(MyDrone.ParcelInTransfer.Sender.Id).CustomerLocation.Latitude - droneToList.CurrentLocation.Latitude) / distance);
                            double longitude = Math.Abs((bl.GetCustomer(MyDrone.ParcelInTransfer.Sender.Id).CustomerLocation.Longitude - droneToList.CurrentLocation.Longitude) / distance);
                            while (distance > 1)
                            {
                                droneToList.Battery -= (int)bl.PowerUsageEmpty;
                                distance -= 1;
                                locationSteps(MyDrone.CurrentLocation, bl.GetCustomer(MyDrone.ParcelInTransfer.Sender.Id).CustomerLocation, MyDrone, longitude, latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                Progress();
                                Thread.Sleep(sleep);
                            }
                            droneToList.CurrentLocation = droneLocation;
                            droneToList.Battery = tempBattery;
                            bl.UpdateParcelCollectionByDrone(MyDrone.Id);
                            Progress();
                        }
                        else // PickedUp != null
                        {
                            tempBattery = droneToList.Battery;
                            distance = MyDrone.ParcelInTransfer.TransportDistance;//the distance betwwen the sender and the resever
                            Location droneLocation = new Location { Longitude = droneToList.CurrentLocation.Longitude, Latitude = droneToList.CurrentLocation.Latitude };
                            double latitude = Math.Abs((bl.GetCustomer(MyDrone.ParcelInTransfer.Target.Id).CustomerLocation.Latitude - droneToList.CurrentLocation.Latitude) / distance);
                            double longitude = Math.Abs((bl.GetCustomer(MyDrone.ParcelInTransfer.Target.Id).CustomerLocation.Longitude - droneToList.CurrentLocation.Longitude) / distance);
                            while (distance > 1)
                            {
                                switch (MyDrone.ParcelInTransfer.Weight)
                                {
                                    case WeightCategories.Easy:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionLightWeight;//light
                                        break;
                                    case WeightCategories.Medium:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionMediumWeight;//medium
                                        break;
                                    case WeightCategories.Heavy:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionHeavyWeight;//heavy
                                        break;
                                    default:
                                        break;
                                }
                                locationSteps(MyDrone.CurrentLocation, bl.GetCustomer(MyDrone.ParcelInTransfer.Target.Id).CustomerLocation, MyDrone, longitude, latitude);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                Progress();
                                distance -= 1;
                                Thread.Sleep(sleep);
                            }
                            droneToList.CurrentLocation = droneLocation;
                            droneToList.Battery = tempBattery;
                            bl.UpdateParcelDeliveryToCustomer(MyDrone.Id);
                            Progress();
                        }
                        break;
                    default:
                        break;
                }
                //ReportProgressInSimultor();
                Thread.Sleep(sleep);
            }


        }
        private void locationSteps(Location locationOfDrone, Location locationOfNextStep, Drone myDrone, double lon, double lat)
        {
            double droneLatitude = locationOfDrone.Latitude;
            double droneLongitude = locationOfDrone.Longitude;

            double nextStepLatitude = locationOfNextStep.Latitude;
            double nextStepLongitude = locationOfNextStep.Longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < nextStepLatitude)
            {
                myDrone.CurrentLocation.Latitude += lat;
            }
            else
            {
                myDrone.CurrentLocation.Latitude -= lat;
            }

            //Calculate the Longitude of the new location.
            if (droneLongitude < nextStepLongitude)
            {
                myDrone.CurrentLocation.Longitude += lon;
            }
            else
            {
                myDrone.CurrentLocation.Longitude -= lon;
            }
        }
    }
}

