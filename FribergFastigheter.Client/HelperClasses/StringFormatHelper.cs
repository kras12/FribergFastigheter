namespace FribergFastigheter.Client.HelperClasses
{
    /// <summary>
    /// Helper class to format strings.
    /// </summary>
    public static class StringFormatHelper
    {
        #region Methods

        /// <summary>
        /// Formats a number as a meter squared string.
        /// </summary>
        /// <param name="area">The area to format.</param>
        /// <returns>A formatted <see cref="string"/>.</returns>
        public static string FormatMeterSquared(double area)
        {
            return $"{area} m²";
        }

        /// <summary>
        /// Formats the number as a price string.
        /// </summary>
        /// <param name="price">The price to format.</param>
        /// <param name="includeDecimals">True to format the price with 2 decimals.</param>
        /// <returns>A formatted <see cref="string"/>.</returns>
        /// <!-- Author: Jimmie -->
        /// <!-- Co Authors: -->
        public static string FormatPrice(decimal price, bool includeDecimals = false)
        {
            if (includeDecimals)
            {
                // price with 2 decimals, thousands separator, and currency.
                return $"{price:C2}";
            }
            else
            {
                // price with 0 decimals, thousands separator, and currency.
                return $"{price:C0}";
            }            
        }

        /// <summary>
        /// Returns a truncated text if i exceeds the character limit.
        /// </summary>
        /// <param name="text">The text to format.</param>
        /// <param name="characterLimit">The max number of characters to return.</param>
        /// <returns>A formatted <see cref="string"/>.</returns>
        public static string TruncateLongTexts(string text, int characterLimit)
        {
            if (text.Length <= characterLimit)
            {
                return text;
            }
            else
            {
                return $"{text.Substring(0, Math.Min(text.Length, characterLimit - 5))}(...)";
            }            
        }

        #endregion

    }
}
