using System;
using TaskManagement.Shared.Responses;

namespace TaskManagement.Shared.Helpers
{
	public class ResponseBuilder
	{
            public static Response<T> Build<T>(T? data, string message, bool status)
            {
                return new Response<T>(message, data, status);
            }

            public static Response<T> Success<T>(T? data, string message, bool status)

            {
                return Build(data, message, status);
            }

            public static Response<T> Failed<T>(string message, bool status = false)
            {
                return Build<T>(default, message, status);
            }

        }
    }


