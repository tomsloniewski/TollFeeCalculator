namespace TollFeeCalculator.Calculator.Vehicles
{
    public class Motorbike : IVehicle, IFreeVehicle
    {
        string IVehicle.GetType()
        {
            return "Motorbike";
        }
    }
}
