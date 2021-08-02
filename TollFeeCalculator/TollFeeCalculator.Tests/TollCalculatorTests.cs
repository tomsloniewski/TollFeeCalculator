using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace TollFeeCalculator
{
    [TestFixture]
    public class TollCalculatorTests
    {
        private TollCalculator calculator;

        [OneTimeSetUp]
        public void Setup()
        {
            calculator = new TollCalculator();
        }

        [Test]
        public void GetTollFeeFreeVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            Vehicle freeVehicle = new Motorbike();
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(0, calculator.GetTollFee(paidDate, freeVehicle));
        }

        [Test]
        public void GetTollFeePaidVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            Vehicle paidVehicle = new Car();
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(8, calculator.GetTollFee(paidDate, paidVehicle));
        }

        [Test]
        public void GetTollFeeNullVehicleTest()
        {
            // tests private IsTollFreeVehicle() method
            Vehicle nullVehicle = null;
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY JANUARY 4 2021 06:15
            Assert.AreEqual(8, calculator.GetTollFee(paidDate, nullVehicle));
        }

        [Test]
        public void GetTollFeeFreeWeekendTest()
        {
            DateTime freeDate1 = new DateTime(2021, 1, 2, 6, 15, 0); // SATURDAY JANUARY 2 2021 06:15
            DateTime freeDate2 = new DateTime(2021, 1, 3, 6, 15, 0); // SUNDAY JANUARY 3 2021 06:15
            Vehicle paidVehicle = new Car();

            Assert.AreEqual(0, calculator.GetTollFee(freeDate1, paidVehicle));
            Assert.AreEqual(0, calculator.GetTollFee(freeDate2, paidVehicle));
        }

        [Test]
        public void GetTollFeePaidHoursEquivalencePartitionsNegativeTest()
        {
            DateTime freeHoursDate1 = new DateTime(2021, 1, 4, 5, 59, 0); // MONDAY JANUARY 4 2021 5:59
            DateTime freeHoursDate2 = new DateTime(2021, 1, 4, 18, 30, 0); // MONDAY JANUARY 4 2021 18:30

            Vehicle paidVehicle = new Car();

            Assert.AreEqual(0, calculator.GetTollFee(freeHoursDate1, paidVehicle));
            Assert.AreEqual(0, calculator.GetTollFee(freeHoursDate2, paidVehicle));
        }

        [Test]
        public void GetTollFeePaidHoursEquivalencePartitionsPositiveTest()
        {
            Vehicle paidVehicle = new Car();
            // ALL DATES ON JANUARY 4 2021 (MONDAY)
            Dictionary<DateTime, int> datesAndFees = new Dictionary<DateTime, int>()
            {
                { new DateTime(2021, 1, 4, 6, 0, 0), 8 },
                { new DateTime(2021, 1, 4, 6, 29, 0), 8 },
                { new DateTime(2021, 1, 4, 6, 30, 0), 13 },
                { new DateTime(2021, 1, 4, 6, 59, 0), 13 },
                { new DateTime(2021, 1, 4, 7, 0, 0), 18 },
                { new DateTime(2021, 1, 4, 7, 59, 0), 18 },
                { new DateTime(2021, 1, 4, 8, 0, 0), 13 },
                { new DateTime(2021, 1, 4, 8, 29, 0), 13 },
                { new DateTime(2021, 1, 4, 8, 30, 0), 8 },
                { new DateTime(2021, 1, 4, 14, 59, 0), 8 },
                { new DateTime(2021, 1, 4, 15, 0, 0), 13 },
                { new DateTime(2021, 1, 4, 15, 29, 0), 13 },
                { new DateTime(2021, 1, 4, 15, 30, 0), 18 },
                { new DateTime(2021, 1, 4, 16, 59, 0), 18 },
                { new DateTime(2021, 1, 4, 17, 0, 0), 13 },
                { new DateTime(2021, 1, 4, 17, 59, 0), 13 },
                { new DateTime(2021, 1, 4, 18, 0, 0), 8 },
                { new DateTime(2021, 1, 4, 18, 29, 0), 8 },
            };

            foreach(KeyValuePair<DateTime, int> pair in datesAndFees)
            {
                Assert.AreEqual(pair.Value, calculator.GetTollFee(pair.Key, paidVehicle));
            }
        }

        [Test]
        public void GetTollFeeMultiDatesUnder60Test()
        {
            Vehicle paidVehicle = new Car();
            DateTime[] paidDates =
            {
                new DateTime(2021, 1, 4, 6, 0, 0), // fee: 8
                new DateTime(2021, 1, 4, 6, 15, 0), // fee: 8 - 8 + 8 = 8
                new DateTime(2021, 1, 4, 8, 20, 0), // fee: 8 + 13 = 21
                new DateTime(2021, 1, 4, 9, 30, 0), // fee: 21 + 8 = 29
                new DateTime(2021, 1, 4, 15, 0, 0), // fee: 29 + 13 = 42
                new DateTime(2021, 1, 4, 18, 0, 0), // fee: 42 + 8 = 50
            };

            Assert.AreEqual(50, calculator.GetTollFee(paidVehicle, paidDates));
        }

        [Test]
        public void GetTollFeeMultiDatesOver60Test()
        {
            Vehicle paidVehicle = new Car();
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

            Assert.AreEqual(60, calculator.GetTollFee(paidVehicle, paidDates));
        }

        [Test]
        public void IsTollFeeDateDatesPositiveTest()
        {
            DateTime[] freeDates =
            {
                new DateTime(2013, 1, 1, 6, 15, 0),
                new DateTime(2013, 3, 28, 6, 15, 0),
                new DateTime(2013, 3, 29, 6, 15, 0),
                new DateTime(2013, 4, 1, 6, 15, 0),
                new DateTime(2013, 4, 30, 6, 15, 0),
                new DateTime(2013, 5, 1, 6, 15, 0),
                new DateTime(2013, 5, 8, 6, 15, 0),
                new DateTime(2013, 5, 9, 6, 15, 0),
                new DateTime(2013, 6, 5, 6, 15, 0),
                new DateTime(2013, 6, 6, 6, 15, 0),
                new DateTime(2013, 6, 21, 6, 15, 0),
                new DateTime(2013, 7, 15, 6, 15, 0),
                new DateTime(2013, 7, 18, 6, 15, 0),
                new DateTime(2013, 11, 1, 6, 15, 0),
                new DateTime(2013, 12, 24, 6, 15, 0),
                new DateTime(2013, 12, 25, 6, 15, 0),
                new DateTime(2013, 12, 26, 6, 15, 0),
                new DateTime(2013, 12, 31, 6, 15, 0)
            };

            foreach (DateTime date in freeDates)
            {
                Assert.IsTrue(calculator.IsTollFreeDate(date));
            }
        }

        [Test]
        public void IsTollFreeDateWeekendPositiveTest()
        {
            DateTime[] freeDates =
            {
                new DateTime(2021, 6, 19, 6, 15, 0), // SATURDAY
                new DateTime(2021, 6, 20, 6, 15, 0) // SUNDAY
            };

            foreach (DateTime date in freeDates)
            {
                Assert.IsTrue(calculator.IsTollFreeDate(date));
            }
        }

        [Test]
        public void IsTollFreeDateNegativeTest()
        {
            DateTime paidDate = new DateTime(2021, 1, 4, 6, 15, 0); // MONDAY

            Assert.IsFalse(calculator.IsTollFreeDate(paidDate));
        }
    }
}
