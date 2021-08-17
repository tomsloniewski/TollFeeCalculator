using NUnit.Framework;
using System;
using TollFeeCalculator.Calculator.Vehicles;
using TollFeeCalculator.Services;

namespace TollFeeCalculator
{
    [TestFixture]
    public class TollPayServiceTests
    {
        private ITollPayService _service;

        [OneTimeSetUp]
        public void Setup()
        {
            _service = new TollPayService();
        }

        [Test]
        public void GetTollFeeFreeVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            IVehicle freeVehicle = new Motorbike();
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(0, _service.GetTollFee(paidDate, freeVehicle));
        }

        [Test]
        public void GetTollFeePaidVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            IVehicle paidVehicle = new Car();
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(8, _service.GetTollFee(paidDate, paidVehicle));
        }

        [Test]
        public void GetTollFeeNullVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            IVehicle nullVehicle = null;
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(8, _service.GetTollFee(paidDate, nullVehicle));
        }

        [Test]
        public void GetTollFeeFreeWeekendTest()
        {
            DateTime freeDate1 = new DateTime(2021, 1, 2, 6, 15, 0); // SATURDAY JANUARY 2 2021 06:15
            DateTime freeDate2 = new DateTime(2021, 1, 3, 6, 15, 0); // SUNDAY JANUARY 3 2021 06:15
            IVehicle paidVehicle = new Car();

            Assert.AreEqual(0, _service.GetTollFee(freeDate1, paidVehicle));
            Assert.AreEqual(0, _service.GetTollFee(freeDate2, paidVehicle));
        }

        [TestCase(5, 59)]
        [TestCase(18, 30)]
        public void GetTollFeePaidHoursEquivalencePartitionsNegativeTest(int hour, int minutes)
        {
            DateTime freeHoursDate = new DateTime(2021, 1, 4, hour, minutes, 0); // MONDAY JANUARY 4 2021

            IVehicle paidVehicle = new Car();

            Assert.AreEqual(0, _service.GetTollFee(freeHoursDate, paidVehicle));
        }

        [TestCase(6, 0, 8)]
        [TestCase(6, 29, 8)]
        [TestCase(6, 30, 13)]
        [TestCase(6, 59, 13)]
        [TestCase(7, 0, 18)]
        [TestCase(7, 59, 18)]
        [TestCase(8, 0, 13)]
        [TestCase(8, 29, 13)]
        [TestCase(8, 30, 8)]
        [TestCase(14, 59, 8)]
        [TestCase(15, 0, 13)]
        [TestCase(15, 29, 13)]
        [TestCase(15, 30, 18)]
        [TestCase(16, 59, 18)]
        [TestCase(17, 0, 13)]
        [TestCase(17, 59, 13)]
        [TestCase(18, 0, 8)]
        [TestCase(18, 29, 8)]
        public void GetTollFeePaidHoursEquivalencePartitionsPositiveTest(int hour, int minutes, int fee)
        {
            IVehicle paidVehicle = new Car();
            // ALL DATES ON JANUARY 4 2021 (MONDAY)
            DateTime date = new DateTime(2021, 1, 4, hour, minutes, 0);

            Assert.AreEqual(fee, _service.GetTollFee(date, paidVehicle));
        }

        [Test]
        public void GetTollFeeMultiDatesUnder60Test()
        {
            IVehicle paidVehicle = new Car();
            DateTime[] paidDates =
            {
                new DateTime(2021, 1, 4, 6, 0, 0), // fee: 8
                new DateTime(2021, 1, 4, 6, 15, 0), // fee: 8 - 8 + 8 = 8
                new DateTime(2021, 1, 4, 8, 20, 0), // fee: 8 + 13 = 21
                new DateTime(2021, 1, 4, 9, 30, 0), // fee: 21 + 8 = 29
                new DateTime(2021, 1, 4, 15, 0, 0), // fee: 29 + 13 = 42
                new DateTime(2021, 1, 4, 18, 0, 0), // fee: 42 + 8 = 50
            };

            Assert.AreEqual(50, _service.GetTollFee(paidVehicle, paidDates));
        }

        [Test]
        public void GetTollFeeMultiDatesOver60Test()
        {
            IVehicle paidVehicle = new Car();
            DateTime[] paidDates =
            {
                new DateTime(2021, 1, 4, 6, 0, 0), // fee: 8
                new DateTime(2021, 1, 4, 6, 15, 0), // fee: 8 - 8 + 8 = 8
                new DateTime(2021, 1, 4, 8, 20, 0), // fee: 8 + 13 = 21
                new DateTime(2021, 1, 4, 9, 30, 0), // fee: 21 + 8 = 29
                new DateTime(2021, 1, 4, 15, 0, 0), // fee: 29 + 13 = 42
                new DateTime(2021, 1, 4, 16, 45, 0), // fee: 42 + 18 = 60
                new DateTime(2021, 1, 4, 18, 0, 0) // fee: 60 + 8 = 68 -> max 60
            };

            Assert.AreEqual(60, _service.GetTollFee(paidVehicle, paidDates));
        }

        [TestCase(2013, 1, 1)]
        [TestCase(2013, 3, 28)]
        [TestCase(2013, 3, 29)]
        [TestCase(2013, 4, 1)]
        [TestCase(2013, 4, 30)]
        [TestCase(2013, 5, 1)]
        [TestCase(2013, 5, 8)]
        [TestCase(2013, 5, 9)]
        [TestCase(2013, 6, 5)]
        [TestCase(2013, 6, 6)]
        [TestCase(2013, 6, 21)]
        [TestCase(2013, 7, 15)]
        [TestCase(2013, 7, 18)]
        [TestCase(2013, 11, 1)]
        [TestCase(2013, 12, 24)]
        [TestCase(2013, 12, 25)]
        [TestCase(2013, 12, 26)]
        [TestCase(2013, 12, 31)]
        public void IsTollFeeDateDatesPositiveTest(int year, int month, int day)
        {
            DateTime freeDate = new DateTime(year, month, day, 6, 15, 0);

            Assert.IsTrue(_service.IsTollFreeDate(freeDate));
        }

        [TestCase(2021, 6, 19)] // Saturday
        [TestCase(2021, 6, 20)] // Sunday
        public void IsTollFreeDateWeekendPositiveTest(int year, int month, int day)
        {
            DateTime freeDate = new DateTime(year, month, day, 6, 15, 0);

            Assert.IsTrue(_service.IsTollFreeDate(freeDate));
        }

        [TestCase(2021, 1, 4)] // Monday
        public void IsTollFreeDateNegativeTest(int year, int month, int day)
        {
            DateTime paidDate = new DateTime(year, month, day, 6, 15, 0);

            Assert.IsFalse(_service.IsTollFreeDate(paidDate));
        }
    }
}
