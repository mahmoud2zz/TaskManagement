using System;
using System.Net.NetworkInformation;

namespace TaskManagement.Shared.Responses
{
    public class Response<T>
    {
        public bool Status { set; get; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public Response(string message, T? data, bool status)
        {
            Status = status;
            Message = message;
            Data = data;
        }

        public Response() { }

    }
}

