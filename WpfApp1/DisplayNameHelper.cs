namespace WpfApp1
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    internal class DisplayNameHelper
    {
        public static string GetPropertyDisplayName(object descriptor, out Type type)
        {
            type = null;
            if (descriptor is PropertyDescriptor pd)
            {
                type = pd.PropertyType;

                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
                return IsNullOrDefault(displayName) ? string.Empty : displayName?.DisplayName;
            }

            var pi = descriptor as PropertyInfo;

            if (pi == null)
                return string.Empty;

            // Check for DisplayName attribute and set the column header accordingly
            var attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            foreach (var displayName in attributes.Select(t => t as DisplayNameAttribute)
                         .TakeWhile(displayName => !IsNullOrDefault(displayName)))
            {
                return displayName?.DisplayName;
            }

            return string.Empty;

            bool IsNullOrDefault(DisplayNameAttribute displayName)
            {
                return displayName == null || displayName == DisplayNameAttribute.Default;
            }
        }
    }
}