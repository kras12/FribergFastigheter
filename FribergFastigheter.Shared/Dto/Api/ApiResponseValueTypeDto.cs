using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergFastigheter.Shared.Dto.Api
{
    /// <summary>
    /// Stores a value type. Designed to be used with class <see cref="ApiResponseDto{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseValueTypeDto<T> where T : struct, IFormattable
    {
        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">The value for a successful response.</param>
        public ApiResponseValueTypeDto(T value)
        {
            Value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The value for a successful response.
        /// </summary>
        public T Value { get; protected set; }

        #endregion
    }
}
