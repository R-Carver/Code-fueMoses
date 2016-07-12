using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Quartz
{
    public class Triggers
    {

        public static SimpleTriggerImpl GetTriggerAtDate(DateTime date)
        {
            SimpleTriggerImpl trigger = new SimpleTriggerImpl
            (
                Triggers.RandomString(), //Random name because it has to be unique for a trigger type
                null,
                date,
                null,
                0,
                TimeSpan.Zero
            );

            return trigger;
        }
        public static SimpleTriggerImpl GetTriggerWithSecondOffset(int seconds)
        {
            SimpleTriggerImpl trigger = new SimpleTriggerImpl
            (
                Triggers.RandomString(), //Random name because it has to be unique for a trigger type
                null,
                DateTime.UtcNow.AddSeconds(seconds),
                null,
                0,
                TimeSpan.Zero
            );

            return trigger;
        }

        private static Random random = new Random();
        private static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}