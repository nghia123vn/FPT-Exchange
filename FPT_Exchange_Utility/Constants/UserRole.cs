using System.Runtime.Serialization;

namespace FPT_Exchange_Utility.Constants
{
    public enum UserRole
    {
        [EnumMember(Value = "a530e7c2-9d44-4ce7-82f7-3edd17b57734")]
        Admin,
        [EnumMember(Value = "3d2c1859-4e4f-4cfa-9efb-afbcd6a7a17d")]
        Staff,
        [EnumMember(Value = "61b99726-8e73-4a0c-be55-c7855bb0a0c3")]
        Customer
    }
}
