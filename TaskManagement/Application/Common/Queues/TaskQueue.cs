using System;
namespace TaskManagement.Application.Common.Queues
{
    public static class TaskQueue
    {
        public static Queue<int> Tasks = new();
    }
}

