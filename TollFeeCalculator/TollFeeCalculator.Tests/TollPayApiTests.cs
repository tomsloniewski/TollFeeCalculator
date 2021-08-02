using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TollFeeCalculator.Models;

namespace TollFeeCalculator
{
    [TestFixture]
    class TollPayApiTests
    {
        private TestServer _server;

        [OneTimeSetUp]
        public void SetUp()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
        }

        [TearDown]
        public void TearDown()
        {
            // Clears up the DB context
            using (var scope = _server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<TollPayContext>();
                var items = dbContext.TollPayItems;
                var itemsCount = items.Take(items.ToList().Count);
                dbContext.TollPayItems.RemoveRange(itemsCount);

                try
                {
                    dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                };
            }
        }

        [Test]
        public async Task TollPayApiPostTest()
        {
            // Check if the response status is OK and if the DB Context gets updated
            string json = "{\"Id\":1,\"Plate\":\"K123ABC\",\"Type\":\"Car\",\"Date\":\"2021-01-04T06:15:00.00Z\"}";
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = _server.CreateClient())
            {
                using (var scope = _server.Host.Services.CreateScope())
                {
                    var response = await client.PostAsync("/api/TollPay/", content);
                    Assert.IsTrue(response.IsSuccessStatusCode);

                    var dbContext = scope.ServiceProvider.GetService<TollPayContext>();
                    Assert.AreEqual(1, dbContext.TollPayItems.ToList().Count);
                }
            }
        }

        [Test]
        public async Task TollPayApiGetNoDataTest()
        {
            // Check if proper prompt is displayed when there is no matching data entry
            using (HttpClient client = _server.CreateClient())
            {
                var response = await client.GetAsync("/api/TollPay/test");
                string responseContent = await response.Content.ReadAsStringAsync();
                Assert.AreEqual("No data found", responseContent);
            }
        }

        [Test]
        public async Task TollPayApiGetTest()
        {
            TollPay tollPayItem = new TollPay()
            {
                Id = 1,
                Plate = "KABC123",
                Timestamp = new DateTime(2021,1,4,6,15,0),
                Type = "Car"
            };

            // Check if proper value is returned when there is a matching entry
            using (var scope = _server.Host.Services.CreateScope())
            {
                using (HttpClient client = _server.CreateClient())
                {
                    // Updating the db context with dummy data
                    var dbContext = scope.ServiceProvider.GetService<TollPayContext>();
                    dbContext.TollPayItems.Add(tollPayItem);

                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    };

                    // GET response
                    var response = await client.GetAsync("/api/TollPay/KABC123");
                    string responseContent = await response.Content.ReadAsStringAsync();
                    Assert.AreEqual("8", responseContent);
                }
            }
        }
    }
}
