using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station newStation)
        {
            if ((Math.Round(Math.Floor(Math.Log10(newStation.Id))) + 1) != 4)//if id inputted is not 4 digits long
                throw new InvalidInputException("The identification number should be 4 digits long\n");
            if (newStation.Name == "\n")//if nothing was inputted as name for station
                throw new InvalidInputException("You have to enter a valid name, with letters\n");//if longitude isnt between 29.3 and 33.5 and latitude isnt between 33.7 and 36.3
            if (newStation.StationLocation.Longitude < 29.3 || newStation.StationLocation.Longitude > 33.5)
                throw new InvalidInputException("The longitude is not valid, enter a longitude point between 29.3 and 33.5\n");
            if (newStation.StationLocation.Latitude < 33.7 || newStation.StationLocation.Latitude > 36.3)
                throw new InvalidInputException("The Latitude is not valid, enter a Latitude point between 33.7 and 36.3\n");
            if (newStation.AvailableChargeSlots < 0)
                throw new InvalidInputException("The number of charging stations of the station is less than 0\n");
            lock (dal)
            {
                try
                {
                    //converting BL station to dal
                    DO.Station tempStation = new DO.Station();
                    object obj = tempStation;
                    newStation.CopyPropertiesTo(obj);
                    tempStation = (DO.Station)obj;
                    newStation.CopyPropertiesTo(tempStation);
                    tempStation.Longitude = newStation.StationLocation.Longitude;
                    tempStation.Latitude = newStation.StationLocation.Latitude;
                    dal.AddStation(tempStation);//adding to station list in dal
                }
                catch (DO.ItemExistsException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationId)
        {
            lock (dal)
            {
                Station blStation = new Station();
                try
                {
                    DO.Station dalStation = dal.FindStation(stationId);//finding station
                    dalStation.CopyPropertiesTo(blStation);//converting to BL
                    blStation.StationLocation = CopyLocation(dalStation.Longitude, dalStation.Latitude);
                    blStation.DronesInCharging = from item in dal.GetAllDroneCharges(item => item.StationId == stationId)
                                                 let temp = BlDrones.FirstOrDefault(indexDroneToList => indexDroneToList.Id == item.DroneId)
                                                 select new DroneInCharging
                                                 {
                                                     Id = item.DroneId,
                                                     Battery = temp != default ? temp.Battery :
                                                         throw new FailedGetException("The Id number does not exist. \n")
                                                 };
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedGetException("ERROR.\n", ex);
                }
                catch (InvalidOperationException)
                {
                    throw new FailedGetException("The drone does not exist.\n");
                }
                return blStation;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAllStations(Predicate<StationToList> predicate = null)
        {
            lock (dal)
            {
                return (from itemStation in dal.GetAllStations()
                        let tempStation = GetStation(itemStation.Id)//getting station with inputted index
                        select new StationToList
                        {
                            Id = tempStation.Id,
                            Name = tempStation.Name,
                            AvailableChargeSlots = tempStation.AvailableChargeSlots,
                            OccupiedChargeSlots = tempStation.DronesInCharging != null ? tempStation.DronesInCharging.Count() : 0
                        }).Where(item => predicate == null ? true : predicate(item));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int idStation, string newName, int chargeSlots)
        {
            lock (dal)
            {
                try
                {
                    if (chargeSlots < 0)//if invalid number was inputted
                        throw new InvalidInputException("The inputted number of empty charges is incorrect. \n");
                    dal.UpdateStation(idStation, newName, chargeSlots);//sends to update in dal
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new FailedToAddException("ERROR.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(int idStation)
        {
            lock (dal)
            {
                try
                {
                    dal.DeleteStation(idStation);//delete parcel
                }
                catch (DO.ItemDoesNotExistException ex)
                {
                    throw new ItemDoesNotExistException("ERROR.\n", ex);
                }
            }
        }
    }
}
