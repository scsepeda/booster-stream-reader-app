using System.Diagnostics.Contracts;
using System.Net;

namespace Booster.StreamReader.Core.Wrappers
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Gets the error message if the operation failed.
        /// </summary>
        public string Error { get; private set; }

        /// <summary>
        /// Gets the result code of the operation.
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// Gets the unique identifier for the result.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="error">The error message if the operation failed.</param>
        /// <param name="code">The result code of the operation.</param>
        protected Result(bool success, string error, int code)
        {
            Contract.Requires(success || !string.IsNullOrEmpty(error));
            Contract.Requires(!success || !string.IsNullOrEmpty(error));
            Success = success;
            Error = error;
            Code = code;
            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Creates a successful result.
        /// </summary>
        /// <returns>A successful result.</returns>
        public static Result Ok() => new(true, string.Empty, (int)ResultCode.OK);

        /// <summary>
        /// Creates a failed result with the specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>A failed result.</returns>
        public static Result Fail(string message) => new(false, message, (int)ResultCode.Failed);
    }

    /// <summary>
    /// Represents the result of an operation with a value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class Result<T> : Result
    {
        private T _value;

        /// <summary>
        /// Gets the value of the result if the operation was successful.
        /// </summary>
        public T Value
        {
            get
            {
                Contract.Requires(Success);
                return _value;
            }
            private set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="success">Indicates whether the operation was successful.</param>
        /// <param name="error">The error message if the operation failed.</param>
        /// <param name="value">The value of the result.</param>
        /// <param name="code">The result code of the operation.</param>
        public Result(bool success, string error, T value, int code) : base(success, error, code)
        {
            Contract.Requires(value != null || !success);
            Value = value;
        }
    }

    /// <summary>
    /// Defines the result codes for operations.
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// Indicates that the operation was successful.
        /// </summary>
        OK = HttpStatusCode.OK,

        /// <summary>
        /// Indicates that the operation failed.
        /// </summary>
        Failed = HttpStatusCode.InternalServerError
    }
}
