using NUnit.Framework;
using TollFeeCalculator.Helpers;

namespace TollFeeCalculator
{
    [TestFixture]
    class HelpersTests
    {
        [TestCase("Car")]
        [TestCase("Motorbike")]
        public void VehicleHandlerTest(string vehicleType)
        {
            Assert.AreEqual(vehicleType, VehicleHandler.GenerateVehicle(vehicleType).GetType());
        }
    }
}
