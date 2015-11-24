using System.Threading;
using System.Threading.Tasks;
using Feefo.Requests;
using Feefo.Responses;

namespace Feefo
{
    public interface IFeefoClient
    {
        Task<FeefoClientResponse> GetFeedbackAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<FeefoClientResponse> GetFeedbackAsync(FeedbackRequest feedbackRequest, CancellationToken cancellationToken = default(CancellationToken));
    }
}