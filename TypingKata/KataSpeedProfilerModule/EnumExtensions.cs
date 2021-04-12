using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace KataSpeedProfilerModule {

    /// <summary>
    /// Collection of Enum Extensions.
    /// </summary>
    public static class EnumExtensions {

        /// <summary>
        /// Get the color value from an enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Color mapped to enum, or default if not found.</returns>
        public static Color GetColorValue(this Enum value) {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes =
                fieldInfo.GetCustomAttributes(typeof(FingerEnumColorAttribute), false) as FingerEnumColorAttribute[];
            return attributes?.Length > 0 ? attributes[0].AttributeColor : default(Color);
        }

        /// <summary>
        /// Get the description from an enum.
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <param name="e">Enum to get descriptor of.</param>
        /// <returns>Description of the enum, or null if it is not found.</returns>
        public static string GetDescription<T>(this T e) where T : IConvertible {
            if (e is Enum) {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values) {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture)) {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null) {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }
}

