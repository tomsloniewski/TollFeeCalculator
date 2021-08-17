using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services;

namespace TollFeeCalculator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TollPayController : ControllerBase
    {
        private readonly ITollPayService _tollPayService;
        private readonly TollPayContext _context;

        public TollPayController(TollPayContext context, ITollPayService tollPayService)
        {
            _tollPayService = tollPayService ?? throw new ArgumentNullException(nameof(_tollPayService));
            _context = context ?? throw new ArgumentNullException(nameof(_context));
        }

        // GET: api/TollPayItems/{plate}
        [HttpGet("{plate}")]
        public async Task<ActionResult<string>> GetTollPayItem(string plate)
        {
            IAsyncEnumerable<TollPay> tollPayItems = _context.TollPayItems.AsAsyncEnumerable();
            string tollPayString = await _tollPayService.GetTollPay(tollPayItems, plate);

            return tollPayString;
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
