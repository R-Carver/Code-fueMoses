using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Quartz
{
    public class Scheduler
    {
        private static IScheduler scheduler = null;
        public static IScheduler Instance()
        {   
            if (scheduler == null)
            {
                scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();
                scheduler.Clear();
            }
            
            return scheduler;
        }
    }
}