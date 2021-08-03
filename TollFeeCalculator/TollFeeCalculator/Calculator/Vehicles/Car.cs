namespace TollFeeCalculator.Calculator.Vehicles
{
    public class Car : IVehicle
    {
        string IVehicle.GetType()
        {
            return "Car";
        }
    }
}
