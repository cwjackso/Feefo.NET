using Feefo.Requests;

namespace Feefo
{
    public interface IQueryStringFactory
    {
        string Create(IFeefoSettings settings, FeedbackRequest feedbackRequest);
    }
}