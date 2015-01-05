//-----------------------------------------------------------------------
// <copyright file="Utils.cs" company="Integra.Vision.Engine">
//     Copyright (c) Integra.Vision.Engine. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Integra.Vision.Engine.Core
{
    using System;
    using System.Linq;

    /// <summary>
    /// Internal utility methods.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static int Clamp(this int value, int minimum, int maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value < minimum)
            {
                value = minimum;
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static long Clamp(this long value, long minimum, long maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value < minimum)
            {
                value = minimum;
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static float Clamp(this float value, float minimum, float maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value < minimum)
            {
                value = minimum;
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static double Clamp(this double value, double minimum, double maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value < minimum)
            {
                value = minimum;
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static decimal Clamp(this decimal value, decimal minimum, decimal maximum)
        {
            if (minimum > maximum)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value < minimum)
            {
                value = minimum;
            }
            else if (value > maximum)
            {
                value = maximum;
            }

            return value;
        }

        /// <summary>
        /// Clamps value to [minimum, maximum] range, that is value cannot be less than minimum or greater than maximum.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="value">The value to compare.</param>
        /// <param name="minimum">The lower value in the range.</param>
        /// <param name="maximum">The upper value in the range.</param>
        /// <returns>coerced to given range</returns>
        public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (minimum.CompareTo(maximum) > 0)
            {
                throw new ArgumentException("minimum cannot be greater than maximum", "minimum");
            }

            if (value.CompareTo(minimum) < 0)
            {
                value = minimum;
            }
            else if (value.CompareTo(maximum) > 0)
            {
                value = maximum;
            }

            return value;
        }
        
        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static int Min(int arg, params int[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            return Math.Min(arg, args.Min());
        }

        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static long Min(long arg, params long[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            return Math.Min(arg, args.Min());
        }

        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static float Min(float arg, params float[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            return Math.Min(arg, args.Min());
        }

        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static double Min(double arg, params double[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            return Math.Min(arg, args.Min());
        }

        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static decimal Min(decimal arg, params decimal[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            return Math.Min(arg, args.Min());
        }
        
        /// <summary>
        /// Computes minimum of given args.
        /// </summary>
        /// <typeparam name="T">The type of the values to compare.</typeparam>
        /// <param name="arg">The value to compare.</param>
        /// <param name="args">The values to compare.</param>
        /// <returns>Minimum of given arguments.</returns>
        public static T Min<T>(T arg, params T[] args) where T : IComparable<T>
        {
            if (args.IsNullOrEmpty())
            {
                return arg;
            }

            T min = arg;

            foreach (var item in args)
            {
                if (min.CompareTo(item) > 0)
                {
                    min = item;
                }
            }

            return min;
        }

        /// <summary>
        /// Check if given array is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the values in the array</typeparam>
        /// <param name="array">The array of values</param>
        /// <returns>
        /// Returns true when array given as parameter is equal null or is empty (has Length equal 0); otherwise false.
        /// </returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array.IsNull() || (array.Length == 0);
        }
        
        /// <summary>
        /// Returns <c>true</c> if given object instance is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">Must be reference type.</typeparam>
        /// <param name="instance">Object to test.</param>
        /// <returns>
        /// Returns true when array given as parameter is equal null; otherwise false.
        /// </returns>
        public static bool IsNull<T>(this T instance) where T : class
        {
            return instance == null;
        }
    }
}
