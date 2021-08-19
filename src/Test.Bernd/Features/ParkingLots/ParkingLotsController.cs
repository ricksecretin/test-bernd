using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Test.Bernd.Features.ParkingLots.GetParkingLots;

namespace Test.Bernd.Features.ParkingLots
{
    [ApiController]
    [Route("api/parkinglots")]
    public class ParkingLotsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ParkingLotsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetParkingLotsQuery());

            return Ok(result);
        }
    }
}