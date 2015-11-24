using System;

namespace Feefo
{
    public interface IFeefoSettings
    {
        string Logon { get; }

        string MerchantIdentifier { get; }

        Uri BaseUri { get; }
    }
}