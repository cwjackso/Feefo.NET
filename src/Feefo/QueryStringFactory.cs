using System.Collections;
using System.Collections.Generic;
using Feefo.Requests;

namespace Feefo
{
    public class QueryStringFactory : IQueryStringFactory
    {
        public string Create(IFeefoSettings settings, FeedbackRequest feedbackRequest)
        {
            var query = "";
            if (!string.IsNullOrEmpty(settings.Logon))
            {
                query = $"?logon={settings.Logon}&json=true";
            }
            else if (!string.IsNullOrEmpty(settings.MerchantIdentifier))
            {
                query = $"?merchantidentifier={settings.MerchantIdentifier}&json=true";
            }
            
            if (feedbackRequest.VendorRef != null)
            {
                query += $"&vendorref={feedbackRequest.VendorRef}";
            }
            
            if (feedbackRequest.Since != Since.None)
            {
                var since = _valueMaps.SinceValueMap[feedbackRequest.Since];

                query += $"&since={since}";
            }

            if (feedbackRequest.Sort != null)
            {
                var sortBy = _valueMaps.SortByValueMap[feedbackRequest.Sort.SortBy];
                var order = feedbackRequest.Sort.Order == Order.Ascending ? "asc" : "desc";

                query += $"&sortby={sortBy}&order={order}";
            }

            if (feedbackRequest.Limit.HasValue)
            {
                query += $"&limit={feedbackRequest.Limit} ";
            } 

            if (feedbackRequest.Mode != Mode.None)
            {
                var mode = _valueMaps.ModeValueMap[feedbackRequest.Mode];
                query += $"&mode={mode}";
            }

            return query;
        }

        private readonly ValueMaps _valueMaps = new ValueMaps();
    }
}