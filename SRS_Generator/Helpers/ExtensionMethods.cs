using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SRS_Generator.Helpers
{
    public static class ExtensionMethods
    {
        public static string ToBold(this string str)
        {
            str = $"**{str}**";
            return str;
        }

        public static string GetDescription(this Enum genericEnum)
        {
            Type genericEnumType = genericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var descriptionAttribute = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((descriptionAttribute != null && descriptionAttribute.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)descriptionAttribute.ElementAt(0)).Description;
                }
            }
            return genericEnum.ToString();
        }
    }
}
