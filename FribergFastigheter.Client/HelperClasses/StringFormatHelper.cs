namespace FribergFastigheter.Client.HelperClasses
{
    /// <summary>
    /// Helper class to format strings.
    /// </summary>
    public static class StringFormatHelper
    {
        #region Methods

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

        #endregion

    }
}
