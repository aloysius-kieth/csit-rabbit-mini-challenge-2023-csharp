using Microsoft.AspNetCore.Mvc;
using RabbitApi.Models;
using RabbitApi.Services;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace RabbitApi.Controllers {
    [ApiController]
    public class FlightController : ControllerBase {
        private readonly MongoDBService _mongoDBService;

        public FlightController(MongoDBService mongoDBService) {
            _mongoDBService = mongoDBService;
        }

        //[HttpGet]
        //public async Task<List<Flight>> Get() {
        //    return await _mongoDBService.GetAllFlights();
        //}

        [HttpGet]
        [Route("/flight")]
        public async Task<ActionResult<List<Flight>>> GetCheapestFlights([FromUri] string departureDate, [FromUri] string returnDate, [FromUri] string destination) {
            bool valid = true;
            if (string.IsNullOrEmpty(departureDate) || !Utils.Utils.IsIsoDateFormat(departureDate)) {
                valid = false;
            }
            if (string.IsNullOrEmpty(returnDate) || !Utils.Utils.IsIsoDateFormat(departureDate)) {
                valid = false;
            }
            if (string.IsNullOrEmpty(destination)) {
                valid = false;
            }

            if (valid) {
                try {
                    // Get departure flights
                    var departureFlights = await _mongoDBService.GetDepartureFlights(departureDate, destination);
                    // Get return flights
                    var returnFlights = await _mongoDBService.GetReturnFlights(returnDate, destination);

                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                    for (int i = 0; i < departureFlights.Count; i++) {
                        Flight d = departureFlights[i];
                        Flight r = returnFlights[i];

                        Dictionary<string, object> item = new Dictionary<string, object>
                        {
                            { "City", destination },
                            { "Departure Date", departureDate },
                            { "Departure Airline", d.airline },
                            { "Departure Price", d.price },
                            { "Return Date", returnDate },
                            { "Return Airline", r.airline },
                            { "Return Price", r.price }
                        };

                        result.Add(item);
                    }

                    return Ok(result);
                } catch (Exception ex) {
                    return BadRequest(ex.Message);
                }

            } else {
                return BadRequest("Bad input");
            }
        }

        [Route("/hotel")]
        public async Task<ActionResult<List<Hotel>>> GetCheapestHotels([FromUri] string checkInDate, [FromUri] string checkOutDate, [FromUri] string destination) {
            bool valid = true;
            if (string.IsNullOrEmpty(checkInDate) || !Utils.Utils.IsIsoDateFormat(checkInDate)) {
                valid = false;
            }
            if (string.IsNullOrEmpty(checkOutDate) || !Utils.Utils.IsIsoDateFormat(checkOutDate)) {
                valid = false;
            }
            if (string.IsNullOrEmpty(destination)) {
                valid = false;
            }

            if (valid) {
                try {
                    // Get hotels based on check in date, check out date and destination
                    var hotels = await _mongoDBService.GetCheapestHotels(checkInDate, checkOutDate, destination);

                    List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();

                    var uniqueHotelNames = hotels.Select(x => x.hotelName).Distinct().ToList();

                    foreach (var hotelName in uniqueHotelNames) {
                        var price = hotels.Where(x => x.hotelName == hotelName);
                        int combinedPrice = price.Sum(x => x.price);

                        Dictionary<string, object> item = new Dictionary<string, object>
{
                            { "City", destination },
                            { "Check In Date", checkInDate },
                            { "Check Out Date", checkOutDate },
                            { "Hotel", hotelName },
                            { "Price", combinedPrice },
                        };

                        result.Add(item);
                    }

                    result = result.OrderBy(i => i["Price"]).ToList();

                    return Ok(result);
                } catch (Exception ex) {
                    return BadRequest(ex.Message);
                }

            } else {
                return BadRequest("Bad input");
            }

        }
    }
}
