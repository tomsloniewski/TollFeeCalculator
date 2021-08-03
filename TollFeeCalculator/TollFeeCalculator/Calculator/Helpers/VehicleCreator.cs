using System;
using TollFeeCalculator.Calculator.Vehicles;

namespace TollFeeCalculator.Calculator.Helpers
{
    public class VehicleCreator
    {
        /**
         * Initializes the vehicle object
         * 
         * @param vehicleType - the name of initialized vehicle object
         * @return - the vehicle object
         */
        public static IVehicle GenerateVehicle(string vehicleType)
        {
            switch (vehicleType)
            {
                // TODO: Fill with other types when implemented
                case "Motorbike":
                    return new Motorbike();
                case "Tractor":
                    throw new NotImplementedException("This vehicle was not yet implemented");
                case "Emergency":
                    throw new NotImplementedException("This vehicle was not yet implemented");
                case "Diplomat":
                    throw new NotImplementedException("This vehicle was not yet implemented");
                case "Foreign":
                    throw new NotImplementedException("This vehicle was not yet implemented");
                case "Military":
                    throw new NotImplementedException("This vehicle was not yet implemented");
                default:
                    return new Car();
            }
        }
    }
}
