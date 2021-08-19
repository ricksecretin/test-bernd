using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Test.Bernd.Features.ParkingLots.GetParkingLots
{
    public class GetParkingLotsQueryHandler : IRequestHandler<GetParkingLotsQuery, GetParkingLotsQueryResult>
    {
        public async Task<GetParkingLotsQueryResult> Handle(
            GetParkingLotsQuery request,
            CancellationToken cancellationToken)
        {
            return new GetParkingLotsQueryResult();
        }
    }
}