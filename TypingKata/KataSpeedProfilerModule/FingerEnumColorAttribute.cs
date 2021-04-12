using System;
using System.Drawing;

namespace KataSpeedProfilerModule {
    
    /// <summary>
    /// Class that allows for custom color attribute.
    /// </summary>
    public class FingerEnumColorAttribute : Attribute {

        /// <summary>
        /// The color.
        /// </summary>
        public Color AttributeColor { get; }

        /// <summary>
        /// Instantiate new FingerEnumColorAttribute.
        /// </summary>
        /// <param name="attributeColor"></param>
        public FingerEnumColorAttribute(KnownColor attributeColor) {
            AttributeColor = Color.FromKnownColor(attributeColor);
        }
    }
}