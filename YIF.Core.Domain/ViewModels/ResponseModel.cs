using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YIF.Core.Domain.ViewModels
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Object { get; set; }

        public ResponseModel<T> Set(bool isSuccess, string message = null)
        {
            Success = isSuccess;
            Message = message;
            return this;
        }
    }
}
