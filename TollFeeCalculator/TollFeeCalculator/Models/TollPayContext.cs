using Microsoft.EntityFrameworkCore;

namespace TollFeeCalculator.Models
{
    public class TollPayContext : DbContext
    {
        public TollPayContext(DbContextOptions<TollPayContext> options) : base(options) { }

        public DbSet<TollPay> TollPayItems { get; set; }
    }
}
