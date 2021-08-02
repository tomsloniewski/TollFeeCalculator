namespace TollFeeCalculator.Helpers
{
    public class VehicleHandler
    {
        public static Vehicle GenerateVehicle(string vehicleType)
        {
            switch (vehicleType)
            {
                // TODO: Fill with other types when implemented
                case "Motorbike":
                    return new Motorbike();
                default:
                    return new Car();
            }
        }
    }
}
