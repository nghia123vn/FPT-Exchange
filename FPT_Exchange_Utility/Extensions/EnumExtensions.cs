using System.Reflection;
using System.Runtime.Serialization;

namespace FPT_Exchange_Utility.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumMember<T>(this T value) where T : Enum
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo.GetCustomAttributes(typeof(EnumMemberAttribute), false) is EnumMemberAttribute[] attributes && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return value.ToString();
            }
        }

        public static string GetEnumKeyFromMember<T>(string memberValue)
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"{enumType.Name} is not an Enum type.");
            }

            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                EnumMemberAttribute attribute = field.GetCustomAttribute<EnumMemberAttribute>();
                if (attribute != null && attribute.Value == memberValue)
                {
                    return field.Name;
                }
            }

            throw new ArgumentException($"No matching enum key found for member value: {memberValue}");
        }
    }
}
