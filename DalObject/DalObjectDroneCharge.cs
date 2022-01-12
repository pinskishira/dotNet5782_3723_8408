using System.Collections.Generic;
using System;

using DO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Dal
{
    partial class DalObject
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == newDroneCharge.DroneId))//checks if drone charge exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            return from itemDroneCharges in DataSource.DroneCharges
                   where predicate == null ? true : predicate(itemDroneCharges)
                   select itemDroneCharges;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int id)
        {
            try
            {
                return DataSource.DroneCharges.First(item => item.DroneId == id);
            }
            catch (InvalidOperationException)
            {
                throw new ItemDoesNotExistException("The drone does not exists.\n");
            }   
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDroneCharge(int id)
        {
            try
            {
                DroneCharge droneCharge = GetAllDroneCharges(item => item.DroneId == id).First();
                DataSource.DroneCharges.Remove(droneCharge);
            }
            catch (InvalidOperationException)
            {
                throw new ItemDoesNotExistException("The drone does not exists.\n");
            }
        }
    }
}
