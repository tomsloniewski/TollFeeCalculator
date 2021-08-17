using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TollFeeCalculator.Calculator.Vehicles;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services
{
    public interface ITollPayService
    {
        Task<string> GetTollPay(IAsyncEnumerable<TollPay> tollPayItems, string plate);
        int GetTollFee(IVehicle vehicle, DateTime[] dates);
        int GetTollFee(DateTime date, IVehicle vehicle);
        bool IsTollFreeDate(DateTime date);
        bool IsTollFreeVehicle(IVehicle vehicle);
    }
}
