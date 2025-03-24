using System.Diagnostics.Contracts;
using System.Net;

namespace Booster.StreamReader.API.Core.Wrappers
{
    public class Result
    {
        public bool Success { get; private set; }
        public string Error { get; private set; }
        public int Code { get; private set; }
        public string Id { get; private set; }

        protected Result(bool success, string error, int code)
        {
            Contract.Requires(success || !string.IsNullOrEmpty(error));
            Contract.Requires(!success || !string.IsNullOrEmpty(error));
            Success = success;
            Error = error;
            Code = code;
            Id = Guid.NewGuid().ToString();
        }
        public static Result Ok() => new(true, string.Empty, (int)ResultCode.OK);
        public static Result Fail(string message) => new(false, message, (int)ResultCode.Failed);
    }

    public class Result<T> : Result
    {
        private T _value;
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
        public Result(bool success, string error, T value, int code) : base(success, error, code)
        {
            Contract.Requires(value != null || !success);
            Value = value;
        }
    }

    public enum ResultCode
    {
        OK = HttpStatusCode.OK,
        Failed = HttpStatusCode.InternalServerError
    }
}
