﻿using System;
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
            double batteryUsage = 0;
            DO.Parcel parcel = default;
            bool pickUp = false;
            Maintenance maintenance = Maintenance.Starting;

            void Deliver(int idDrone)
            {
                parcel = dal.FindParcel(idDrone);
                int weightOfParcel = (int)parcel.Weight + 1;
                batteryUsage = dal.electricityUse()[weightOfParcel];
                pickUp = parcel.PickedUp is not null;
                customer = bl.GetCustomer(pickUp ? parcel.TargetId : parcel.SenderId);
            }

            do
            {
                switch (drone.DroneStatus)
                {
                    case DroneStatuses.Available:
                        if (!TimeSleep())
                            break;
                        parcelId = dal.GetAllParcels(p => p.Scheduled == null
                        && (WeightCategories)p.Weight <= drone.Weight
                        && bl.BatteryConsumption(drone.Id, p) < drone.Battery)
                            .OrderByDescending(x => x.Priority)
                            .ThenByDescending(x => x.Weight)
                            .FirstOrDefault().Id;
                        if (parcelId == default && drone.Battery != 100)
                        {
                            DO.Station tmpStation = bl.smallestDistance(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude);
                            stationId = tmpStation.Id;
                            if (stationId != default)
                            {
                                drone.DroneStatus = DroneStatuses.Maintenance;
                                maintenance = Maintenance.Starting;
                                dal.UpdateSendDroneToChargingStation(droneId, tmpStation.Name);
                                DO.DroneCharge droneCharge = new() { DroneId = drone.Id, StationId = stationId };
                                dal.AddDroneCharge(droneCharge);
                            }
                            if (parcelId != default && drone.Battery != 100)
                            {
                                try
                                {
                                    dal.UpdateAssignParcelToDrone(droneId, parcelId);
                                    drone.ParcelInTransfer.Id = parcelId;
                                    Deliver(parcelId);
                                    drone.DroneStatus = DroneStatuses.Delivery;
                                }
                                catch (DO.ItemDoesNotExistException ex)
                                {
                                    throw new WrongStatusException("Error getting parcel", ex);
                                }
                            }
                        }
                        break;
                    case DroneStatuses.Maintenance:
                        switch (maintenance)
                        {
                            case Maintenance.Starting:
                                lock (bl)
                                {
                                    if (stationId != default)
                                        station = bl.GetStation(stationId);
                                    else
                                        station = bl.GetStation(dal.GetDroneCharge(droneId).StationId);
                                    distance = Distance.Haversine(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude, station.StationLocation.Longitude, station.StationLocation.Latitude);
                                    maintenance = Maintenance.Going;
                                }
                                break;
                            case Maintenance.Going:
                                if (distance < 1)
                                    lock (bl)
                                    {
                                        drone.CurrentLocation = station.StationLocation;
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!TimeSleep())
                                        break;
                                    lock (bl)
                                    {
                                        double del = distance < Step ? distance : Step;
                                        distance -= del;
                                        if (drone.Battery - del * dal.electricityUse()[0] < 0)
                                            drone.Battery = 0;
                                        else
                                            drone.Battery = (int)(drone.Battery - del * dal.electricityUse()[0]);
                                    }

                                }
                                break;
                            case Maintenance.Charging:
                                if (drone.Battery == 100)
                                    lock (bl)
                                    {
                                        drone.DroneStatus = DroneStatuses.Available;
                                        bl.DroneReleaseFromChargingStation(drone.Id);
                                    }
                                else
                                {
                                    if (!TimeSleep())
                                        break;
                                    lock (bl)
                                    {
                                        drone.Battery = (int)Math.Min(100, drone.Battery + dal.electricityUse()[0] * TimeStep);
                                    }
                                }
                                break;
                        }
                        break;

                    case DroneStatuses.Delivery:
                        lock (bl)
                        {
                            try
                            {
                                if (parcelId == default)
                                    Deliver(drone.ParcelInTransfer.Id);
                            }
                            catch (DO.ItemDoesNotExistException ex)
                            {
                                throw new WrongStatusException("Error getting parcel", ex);
                            }
                            distance = Distance.Haversine(drone.CurrentLocation.Longitude, drone.CurrentLocation.Latitude, customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude);
                        }

                        if (distance < 0.01 || drone.Battery == 0.0)
                        {
                            lock (bl)
                            {
                                drone.CurrentLocation = customer.CustomerLocation;
                                if (pickUp)
                                {
                                    dal.UpdateParcelDeliveryToCustomer(parcelId);
                                    drone.DroneStatus = DroneStatuses.Available;
                                }
                                else
                                    dal.UpdateParcelCollectionByDrone(parcelId);
                            }
                        }
                        else
                        {
                            if (!TimeSleep())
                                break;
                            lock (bl)
                            {
                                double diffs = distance < Step ? distance : Step;
                                double quantity = diffs / distance;
                                drone.Battery = (int)Math.Max(0.0, drone.Battery - diffs * dal.electricityUse()[pickUp ? (int)batteryUsage : 0]);
                                double latitude = drone.CurrentLocation.Latitude + (customer.CustomerLocation.Latitude - drone.CurrentLocation.Latitude) * quantity;
                                double longitude = drone.CurrentLocation.Longitude + (customer.CustomerLocation.Longitude - drone.CurrentLocation.Longitude) * quantity;
                                drone.CurrentLocation = new() { Longitude = longitude, Latitude = latitude };
                            }
                        }
                        break;

                    default:
                        throw new WrongStatusException("Error: Drone now not available");
                }
                action();

            } while (!stop());
        }
        private static bool TimeSleep()
        {
            try
            {
                Thread.Sleep(Delay);
            }
            catch (ThreadErrorException)
            {
                return false;
            }

            return true;
        }
    }
}

