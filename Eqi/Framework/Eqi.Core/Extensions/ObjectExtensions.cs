using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eqi.Core
{
    /// <summary>
    /// Object extension function.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Converts current object to an enum. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum.</typeparam>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Enum value.</returns>
        public static TEnum ToEnum<TEnum>(this object me, TEnum defaultValue) where TEnum : struct
        {
            TEnum result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                Type myType = me.GetType();
                Type destinationType = typeof(TEnum);

                if (myType.IsEnum && (myType != destinationType))
                {
                    me = me.ToString();
                }

                if (me is int)
                {
                    if (Enum.IsDefined(typeof(TEnum), me))
                    {
                        result = (TEnum)me;
                    }
                }
                else if (!Enum.TryParse(me.ToString(), out result))
                {
                    result = defaultValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to an enum. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <typeparam name="TEnum">Type of enum.</typeparam>
        /// <param name="me">Current object.</param>
        /// <returns>Enum value.</returns>
        public static TEnum ToEnum<TEnum>(this object me) where TEnum : struct
        {
            return me.ToEnum(default(TEnum));
        }
    }
}
