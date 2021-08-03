using NUnit.Framework;
using TollFeeCalculator.Calculator.Helpers;

namespace TollFeeCalculator
{
    [TestFixture]
    class HelpersTests
    {
        [TestCase("Car")]
        [TestCase("Motorbike")]
        public void VehicleHandlerTest(string vehicleType)
        {
            Assert.AreEqual(vehicleType, VehicleCreator.GenerateVehicle(vehicleType).GetType());
        }
    }
}
