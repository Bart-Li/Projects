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
        /// Converts current object to string. If current object is null or cannot be converted to string, defaultvalue is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>String value.</returns>

        public static string ToString(this object me, string defaultValue = default(string))
        {
            string result = defaultValue;
            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    result = me as string;
                }
                else
                {
                    result = me.ToString();
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to an char. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Int value.</returns>
        public static char ToChar(this object me, char defaultValue = default(char))
        {
            char result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!char.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToChar(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to an int. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultvalue">The default value.</param>
        /// <returns>Int value.</returns>
        public static int ToInt(this object me, int defaultValue = default(int))
        {
            int result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!int.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToInt32(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to a long. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultvalue">The default value.</param>
        /// <returns>Long value.</returns>
        public static long ToLong(this object me, long defaultValue = default(long))
        {
            long result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!long.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToInt64(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to a decimal. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Decimal value.</returns>
        public static decimal ToDecimal(this object me, decimal defaultValue = default(decimal))
        {
            decimal result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!decimal.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToDecimal(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to a float. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Long value.</returns>
        public static float ToFloat(this object me, float defaultValue = default(float))
        {
            float result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!float.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToSingle(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to a double. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Long value.</returns>
        public static double ToDouble(this object me, double defaultValue = default(double))
        {
            double result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is string)
                {
                    if (!double.TryParse((string)me, out result))
                    {
                        result = defaultValue;
                    }
                }
                else if (me is IConvertible)
                {
                    result = Convert.ToDouble(me);
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to string, then parse string to a DateTime. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>DateTime value.</returns>
        public static DateTime ToDateTime(this object me, DateTime defaultValue = default(DateTime))
        {
            DateTime result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is DateTime)
                {
                    result = (DateTime)me;
                }
                else if (!DateTime.TryParse(me.ToString(), out result))
                {
                    result = defaultValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Converts current object to a bool. If current object is null or cannot be converted to the target type, default value is returned.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Bool value.</returns>
        public static bool ToBool(this object me, bool defaultValue = false)
        {
            bool result = defaultValue;

            if (me != null && me != DBNull.Value)
            {
                if (me is bool)
                {
                    result = (bool)me;
                }
                else if (me.IsNumeric())
                {
                    result = me.ToDecimal() != decimal.Zero ? true : defaultValue;
                }
                else if (me is string)
                {
                    if (!bool.TryParse(me.ToString(), out result))
                    {
                        result = defaultValue;
                    }
                }
                else
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Check whether current object is numeric.
        /// </summary>
        /// <param name="me">Current object.</param>
        /// <returns>Whether current object is numeric.</returns>
        public static bool IsNumeric(this object me)
        {
            if (!(me is byte ||
                 me is short ||
                 me is int ||
                 me is long ||
                 me is sbyte ||
                 me is ushort ||
                 me is uint ||
                 me is ulong ||
                 me is decimal ||
                 me is double ||
                 me is float))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

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
