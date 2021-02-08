using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace KataSpeedProfilerModule {
    public class KeyToCharacterConverter : IValueConverter {

        /// <summary>
        /// Convert Key to character.
        /// </summary>
        /// <param name="value">The key value</param>
        /// <param name="targetType">The target type (char).</param>
        /// <param name="parameter">Optional parameter(can be null).</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Converted Character.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value != null) {
                var key = (Key) value;
                string sKey = System.Convert.ToString(key);
                try {
                    var c = sKey[0];
                    return c;
                }
                catch (Exception) {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Convert Character to Key.
        /// </summary>
        /// <param name="value">The Character value.</param>
        /// <param name="targetType">The target type (Key).</param>
        /// <param name="parameter">Optional parameter (can be null).</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return null;

            var c = (char) value;
            var sChar = new string(new[]{char.ToUpper(c)});
            var success = Enum.TryParse<Key>(sChar, false, out var result);

            if (success) {
                return result;
            }

            return null;
        }
    }
}
