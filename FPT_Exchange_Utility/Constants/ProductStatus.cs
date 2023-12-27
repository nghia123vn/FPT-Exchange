using System.Runtime.Serialization;

namespace FPT_Exchange_Utility.Constants
{
    public enum ProductStatus
    {
        [EnumMember(Value = "8dda5aac-4c23-4abd-a2e1-f5fe677dc198")]
        Active,
        [EnumMember(Value = "feec488e-1bcd-46e3-9514-dd641f3c161a")]
        Inactive,
        [EnumMember(Value = "ac090dda-de3b-484e-b7a8-fbe4d2762ba8")]
        Saled,
        [EnumMember(Value = "75fce374-e1f0-40dd-8214-8dc98125ad2b")]
        Processing,
        [EnumMember(Value = "239041a7-d82a-4968-8547-3e991c2032e2")]
        Confirmed,
        [EnumMember(Value = "2cf8fe4e-f9a2-4cd5-8f6f-db7e046b105a")]
        Canceled,
        [EnumMember(Value = "9ffbbdec-fc82-4851-a239-b60ab8fe8286")]
        Rejected
    }
}
