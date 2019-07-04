using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace wpf.control
{
    public class Utility
    {
        public const int Default_DPI = 96;
        public static int GetDpiBySystemParameters()
        {
            const System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static;

            var dpiXProperty = typeof(SystemParameters).GetProperty("DpiX", bindingFlags);

            if (dpiXProperty != null)
            {
                return (int)dpiXProperty.GetValue(null, null);
            }
            return 96;

        }

        public static double GetDpiRatio()
        {
            return (double)Default_DPI / (double)GetDpiBySystemParameters();
 

        }
    }
}
