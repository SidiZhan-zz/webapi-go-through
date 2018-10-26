using System;

namespace WebApplication.Services
{
    public class TimeProvider : ITimeProvider
    {
        public string GenerateTime()
        {
            return DateTime.UtcNow.ToShortTimeString();
        }
    }

    public interface ITimeProvider
    {
        string GenerateTime();
    }
}