using System;
using System.Collections.Generic;
using System.Linq;

using BlApi;
using BO;
using static BO.Enum;

using DalApi;

namespace BL
{
    /// <summary>
    /// Constructor BL that defines the fields.
    /// </summary>
    public sealed partial class BL : Ibl
    {
        internal static BL Instance { get; } = new BL();
        static BL() { }
        static Random rand;
        IDal dal;
        List<DroneToList> BlDrones = new List<DroneToList>();//new list of drones
        double PowerUsageEmpty, BatteryConsumptionLightWeight, BatteryConsumptionMediumWeight, BatteryConsumptionHeavyWeight, DroneChargingRatePH;
        private BL()
        {
            dal = DalApi.DLFactory.GetDL();
            rand = new Random();
            PowerUsageEmpty = dal.electricityUse()[0];//When the drone is empty
            BatteryConsumptionLightWeight = dal.electricityUse()[1];//amount of battery used per km for light weight
            BatteryConsumptionMediumWeight = dal.electricityUse()[2];//amount of battery used per km for medium weight
            BatteryConsumptionHeavyWeight = dal.electricityUse()[3];//amount of battery used per km for heavy weight
            DroneChargingRatePH = dal.electricityUse()[4];//Charging per minutes
            dal.GetAllDrones().CopyPropertiesToIEnumerable(BlDrones);//converting list of dal to BL
            foreach (var indexOfDrones in BlDrones)//going through converted list of drones in the BL
            {
                try
                {
                    //if parcel was paired but not delivered
                    DO.Parcel parcel = dal.GetAllParcels().First(item => item.DroneId == indexOfDrones.Id && item.Delivered == null);
                    indexOfDrones.ParcelIdInTransfer = parcel.Id;
                    indexOfDrones.DroneStatus = DroneStatuses.Delivery;//updating status to be in delivery
                    if (parcel.Scheduled != null && parcel.PickedUp == null)//if parcel was paired but not picked up
                    {
                        DO.Customer sender = dal.FindCustomer(parcel.SenderId);//finding the customer usind id number
                        DO.Station closestStation = smallestDistance(sender.Longitude, sender.Latitude);//returning samllest distance between the sender of the parcel and the stations 
                        indexOfDrones.CurrentLocation = new Location();
                        indexOfDrones.CurrentLocation.Longitude = closestStation.Longitude;//updating the loaction of the drone 
                        indexOfDrones.CurrentLocation.Latitude = closestStation.Latitude;
                        //calculates the usage of battery of the drone according to the distance it travelled and the weight of its parcel
                        double batteryConsumption = BatteryConsumption(indexOfDrones, parcel) + Distance.Haversine
                            (indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude, sender.Longitude, sender.Latitude) * PowerUsageEmpty;
                        indexOfDrones.Battery = rand.Next((int)batteryConsumption, 101);//random selection between battery consumption found and full charge
                    }
                    if (parcel.PickedUp != null)//if parcel was picked up but not delivered 
                    {
                        DO.Customer tempCustomer = dal.FindCustomer(parcel.SenderId);//finding the sender in customers
                        indexOfDrones.CurrentLocation = new Location();
                        indexOfDrones.CurrentLocation.Longitude = tempCustomer.Longitude;//updating the location  of the drone
                        indexOfDrones.CurrentLocation.Latitude = tempCustomer.Latitude;
                        //using random selection to calculate battery using distance the drone traveled and the parcel it collected and full charge
                        indexOfDrones.Battery = rand.Next(BatteryConsumption(indexOfDrones, parcel), 101);
                    }
                }
                catch (InvalidOperationException)
                {
                    if (indexOfDrones.DroneStatus != DroneStatuses.Delivery)//if the drone is not performing a delivery
                        indexOfDrones.DroneStatus = (DroneStatuses)rand.Next(0, 2);//his status will be found using random selection  
                    if (indexOfDrones.DroneStatus == DroneStatuses.Maintenance)//if the drone is in maintanance
                    {
                        List<DO.Station> tempStations = dal.GetAllStations().ToList();//temporary array with all the stations
                        int idStation = rand.Next(0, tempStations.Count());//finding a random index from the array of stations
                        DO.Station station = tempStations[idStation];//placing the index returned into the stations list 
                        indexOfDrones.CurrentLocation = new Location();
                        indexOfDrones.CurrentLocation.Longitude = station.Longitude;//updating the location of the drone
                        indexOfDrones.CurrentLocation.Latitude = station.Latitude;
                        indexOfDrones.Battery = rand.Next(0, 21);//battery will be between 0 and 20 using random selection
                        DO.DroneCharge droneCharge = new DO.DroneCharge();
                        droneCharge.DroneId = indexOfDrones.Id;
                        droneCharge.StationId = station.Id;
                        droneCharge.TimeDroneInCharging = DateTime.Now;
                        dal.AddDroneCharge(droneCharge);
                    }
                    if (indexOfDrones.DroneStatus == DroneStatuses.Available)//if the drone is available for delivery
                    {
                        List<DO.Parcel> deliveredParcels = (List<DO.Parcel>)dal.GetAllParcels(indexOfParcels => indexOfParcels.Delivered != null);
                        int idCustomer = deliveredParcels[rand.Next(0, deliveredParcels.Count())].TargetId;//finding a random index from the new list of deliveredParcels
                        DO.Customer customer = dal.FindCustomer(idCustomer);//finding the customer with the index found with random selection
                        indexOfDrones.CurrentLocation = new Location();
                        indexOfDrones.CurrentLocation.Longitude = customer.Longitude;//updating the location of the drone
                        indexOfDrones.CurrentLocation.Latitude = customer.Latitude;
                        //finding the closest station to the current location of the drone
                        DO.Station smallestDistanceStation = smallestDistance(indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude);
                        //finding the distance from closest station and the drones current location
                        double distanceFromStation = Distance.Haversine
                            (indexOfDrones.CurrentLocation.Longitude, indexOfDrones.CurrentLocation.Latitude, smallestDistanceStation.Longitude, smallestDistanceStation.Latitude);
                        //calculating battery using the distance it will travel and the amount of battery used per km
                        indexOfDrones.Battery = rand.Next((int)(distanceFromStation * PowerUsageEmpty), 101);
                    }
                }
            }
        }

        public int BatteryConsumption(DroneToList droneToList, DO.Parcel parcel)
        {
            DO.Customer target = dal.FindCustomer(parcel.TargetId);//finding the target customer
            DO.Customer sender = dal.FindCustomer(parcel.SenderId);//finding the sender customer
            //finding distance between the sender and the target 
            double distanceFromTarget = Distance.Haversine(sender.Longitude, sender.Latitude, target.Longitude, target.Latitude);
            //finding the station closest to the target to perform delivery
            DO.Station smallestStation = smallestDistance(target.Longitude, target.Latitude);
            //finding the distance between the closest station to target and the target destination
            double distanceFromStation = Distance.Haversine(target.Longitude, target.Latitude, smallestStation.Longitude, smallestStation.Latitude);
            //calculates distance by multiplying by its weight and the amount of battery it uses per km.
            //return (int)(distanceFromTarget * Weight(droneToList.MaxWeight) + distanceFromStation * PowerUsageEmpty);
            return (int)Math.Ceiling(distanceFromTarget * Weight((WeightCategories)parcel.Weight) + distanceFromStation * PowerUsageEmpty);
        }

        public double Weight(WeightCategories maxWeight)
        {
            if (maxWeight == WeightCategories.Easy)
                return BatteryConsumptionLightWeight;
            if (maxWeight ==  WeightCategories.Medium)
                return BatteryConsumptionMediumWeight;
            else
                return BatteryConsumptionHeavyWeight;
        }
        //public double Weight(WeightCategories maxWeight) => maxWeight switch
        //{
        //    WeightCategories.Easy => BatteryConsumptionLightWeight,
        //    WeightCategories.Medium => BatteryConsumptionMediumWeight,
        //    WeightCategories.Heavy => BatteryConsumptionHeavyWeight,
        //    _ => throw new ArgumentException()
        //};

        public DO.Station smallestDistance(double longitude, double latitude)
        {
            double minDistance = double.PositiveInfinity;//starting with an unlimited value
            DO.Station station = new DO.Station();
            double tempDistance = -1;
            foreach (var indexOfStations in dal.GetAllStations())//goes through all the stations 
            {
                //calculating the distance between the sender and the station
                tempDistance = Distance.Haversine(indexOfStations.Longitude, indexOfStations.Latitude, longitude, latitude);
                if (tempDistance < minDistance)//compares which distance is smaller
                {
                    minDistance = tempDistance;
                    station = indexOfStations;
                }
            }
            return station;//returns closest station to sender
        }

        public Location CopyLocation(double longitude, double latitude)
        {
            Location currentLocation = new Location();
            currentLocation.Longitude = longitude;
            currentLocation.Latitude = latitude;
            return currentLocation;
        }
    }
}
