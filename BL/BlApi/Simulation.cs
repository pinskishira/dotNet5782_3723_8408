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
        BL bl;

        private const double kmh = 3600;//כל קילומטר זה שנייה כי בשעה יש 3600 שניות

        public Simulation(BL _bl, int droneID, Action Progress, Func<bool> stopSim)
        {
            DalApi.IDal Idal = DalApi.DLFactory.GetDL();
            bl = _bl;
            var dal = bl;
            //area for seting puse time
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
                                    droneToList.Battery -= (int)bl.PowerUsageEmpty;
                                    Progress();
                                    distance -= 1;
                                    Thread.Sleep(1500);
                                }
                                droneToList.Battery = tempBattery;//הפונקציה שליחה לטעינה בודקת את המרחק ההתחלתי ולפי זה מחשבת את הסוללה ולכן צריך להחזיר למצב ההתחלתי
                                bl.SendDroneToChargingStation(droneID);
                                Progress();
                            }
                        }
                        break;
                    case DroneStatuses.Maintenance: //
                        bool flag = true;
                        while (droneToList.Battery < 100 && flag)
                        {
                            if (stopSim())
                                flag = false;
                            if (droneToList.Battery + 10 > 100)//בדיקה אם כבר עברנו את ה100%
                                bl.GetAllDrones().First(item => item.Id == droneToList.Id).Battery = 100;
                            else
                                bl.GetAllDrones().First(item => item.Id == droneToList.Id).Battery += 10;
                            Progress();
                            Thread.Sleep(1500);
                        }
                        if (flag == true)
                        {
                            bl.DroneReleaseFromChargingStation(droneID); //שחרור מטעינה ברגע שהרחפן מגיע ל100
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
                            while (distance > 1)
                            {
                                droneToList.Battery -= (int)bl.PowerUsageEmpty;
                                distance -= 1;
                                locationSteps(MyDrone.CurrentLocation, bl.GetCustomer(MyDrone.ParcelInTransfer.Sender.Id).CustomerLocation, MyDrone);
                                droneToList.CurrentLocation = MyDrone.CurrentLocation;
                                Progress();
                                Thread.Sleep(1500);
                            }
                            droneToList.CurrentLocation = droneLocation;
                            droneToList.Battery = tempBattery;
                            bl.UpdateParcelCollectionByDrone(MyDrone.Id);
                            Progress();
                        }
                        else // PickedUp != null
                        {
                            tempBattery = droneToList.Battery;
                            distance = MyDrone.ParcelInTransfer.TransportDistance;
                            while (distance > 1)
                            {
                                switch (MyDrone.ParcelInTransfer.Weight)
                                {
                                    case WeightCategories.Easy:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionLightWeight;
                                        break;
                                    case WeightCategories.Medium:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionMediumWeight;
                                        break;
                                    case WeightCategories.Heavy:
                                        droneToList.Battery -= (int)bl.BatteryConsumptionHeavyWeight;
                                        break;
                                    default:
                                        break;
                                }

                                Progress();
                                distance -= 1;
                                Thread.Sleep(1500);
                            }

                            droneToList.Battery = tempBattery;
                            bl.UpdateParcelDeliveryToCustomer(MyDrone.Id);
                            Progress();
                        }
                        break;
                    default:
                        break;
                }
                //ReportProgressInSimultor();
                Thread.Sleep(1500);
            }


        }
        private void locationSteps(Location locationOfDrone, Location locationOfNextStep, Drone myDrone)
        {
            double droneLatitude = locationOfDrone.Latitude;
            double droneLongitude = locationOfDrone.Longitude;

            double senderLatitude = locationOfNextStep.Latitude;
            double senderLongitude = locationOfNextStep.Longitude;

            if (droneLatitude < senderLatitude)// ++++++
            {
                double step = (senderLatitude - droneLatitude) / myDrone.ParcelInTransfer.TransportDistance;
                myDrone.CurrentLocation.Latitude += step;
            }
            else
            {
                double step = (droneLatitude - senderLatitude) / myDrone.ParcelInTransfer.TransportDistance;
                myDrone.CurrentLocation.Latitude -= step;

            }

            if (droneLongitude < senderLongitude)//+++++++
            {
                double step = (senderLongitude - droneLongitude) / myDrone.ParcelInTransfer.TransportDistance;
                myDrone.CurrentLocation.Longitude += step;
            }
            else
            {
                double step = (droneLongitude - senderLongitude) / myDrone.ParcelInTransfer.TransportDistance;
                myDrone.CurrentLocation.Longitude -= step;

            }
        }
    }
}

