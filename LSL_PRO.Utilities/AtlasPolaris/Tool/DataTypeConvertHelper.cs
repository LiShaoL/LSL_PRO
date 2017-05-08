using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSL_PRO.Utilities.AtlasPolaris.Tool
{
    /// <summary>
    /// Type to type.
    /// </summary>
    public class T2T
    {
        public static int To_Int(object obj)
        {
            int result;
            int.TryParse(obj.ToString(), out result);
            return result;
        }

        public static double To_double(object obj)
        {
            double result;
            double.TryParse(obj.ToString(), out result);
            return result;
        }

        public static decimal To_decimal(object obj)
        {
            decimal result;
            decimal.TryParse(obj.ToString(), out result);
            return result;
        }
        public static float To_float(object obj)
        {
            float result;
            float.TryParse(obj.ToString(), out result);
            return result;
        }
    }
}
