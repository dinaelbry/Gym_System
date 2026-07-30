using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public class Result
    {
        public bool Succeeded { get; }
        public string? Message { get; }

        private Result(bool succeeded, string? message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public static Result Ok() => new(true, null);
        public static Result Fail(string message) => new(false, message);
        public static Result NotFound(string message) => new(false, message);
    }
}
