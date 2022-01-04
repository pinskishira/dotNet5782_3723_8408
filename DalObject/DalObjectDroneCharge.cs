using System.Collections.Generic;
using System;

using DO;
using System.Linq;

namespace Dal
{
    partial class DalObject
    {
        public void AddDroneCharge(DroneCharge newDroneCharge)
        {
            if (DataSource.DroneCharges.Exists(item => item.DroneId == newDroneCharge.DroneId))//checks if drone charge exists
                throw new ItemExistsException("The drone already exists.\n");
            DataSource.DroneCharges.Add(newDroneCharge);
        }

        public IEnumerable<DroneCharge> GetAllDroneCharges(Predicate<DroneCharge> predicate = null)
        {
            return from itemDroneCharges in DataSource.DroneCharges
                   where predicate == null ? true : predicate(itemDroneCharges)
                   select itemDroneCharges;
        }

        public DroneCharge GetDroneCharge(int id)
        {
            try
            {
                return DataSource.DroneCharges.First(item => item.DroneId == id);
            }
            catch (InvalidOperationException)
            {
                throw new ItemExistsException("The drone does not exists.\n");
            }
            
        }
    }
}
