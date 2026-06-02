using System.Collections.Concurrent;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;

    // userId/IP -> (count, timestamp)
    private static readonly ConcurrentDictionary<string, (int Count, DateTime Time)> _requests = new();

    public RateLimitMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // 👇 نجيب المستخدم (JWT uid)
        var userId = context.User?.FindFirst("uid")?.Value
                     ?? context.Connection.RemoteIpAddress?.ToString();

        if (userId == null)
        {
            await _next(context);
            return;
        }

        var now = DateTime.UtcNow;

        var entry = _requests.GetOrAdd(userId, _ => (0, now));

        // لو داخل نفس الدقيقة
        if ((now - entry.Time).TotalMinutes < 1)
        {
            if (entry.Count >= 5)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many requests (limit = 5 per minute)");
                return;
            }

            _requests[userId] = (entry.Count + 1, entry.Time);
        }
        else
        {
            // reset
            _requests[userId] = (1, now);
        }

        await _next(context);
    }
}