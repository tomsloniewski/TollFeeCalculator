using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Helpers;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TollPayController : ControllerBase
    {
        private readonly TollPayContext _context;
        private readonly TollCalculator _calculator;

        public TollPayController(TollPayContext context)
        {
            _context = context;
            _calculator = new TollCalculator();
        }

        // GET: api/TollPayItems/KR98K12
        [HttpGet("{plate}")]
        public async Task<ActionResult<string>> GetTollPayItem(string plate)
        {
            IAsyncEnumerable<TollPay> asyncEnumerable = _context.TollPayItems.AsAsyncEnumerable();
            List<DateTime> timestamps = new List<DateTime>();
            string carType = "";
            Vehicle vehicle;

            await foreach (var tollPayItem in asyncEnumerable)
            {
                if (carType.Length == 0) carType = tollPayItem.Type;
                if (tollPayItem.Plate == plate)
                {
                    timestamps.Add(tollPayItem.Timestamp);
                }
            }

            vehicle = VehicleHandler.GenerateVehicle(carType);
            if (timestamps.Count > 0)
            {
                return _calculator.GetTollFee(vehicle, timestamps.ToArray()).ToString();
            }
            else
            {
                return "No data found";
            }
        }

        // POST: api/TollPayItems
        [HttpPost]
        public async Task<ActionResult<TollPay>> PostTollPayItem(TollPay tollPayItem)
        {
            _context.TollPayItems.Add(tollPayItem);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(PostTollPayItem), new { id = tollPayItem.Id }, tollPayItem);
        }
    }
}
