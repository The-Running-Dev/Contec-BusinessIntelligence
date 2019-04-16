using System.Collections.Generic;

namespace Contec.Framework.Data.Types
{
    public class Interval
    {
        public static Dictionary<IntervalType, double> Conversions =
            new Dictionary<IntervalType, double>() {
                {IntervalType.MilliSeconds, 0.001},
                {IntervalType.Seconds, 1},
                {IntervalType.Minutes, 60},
                {IntervalType.Hours, 3600},
                {IntervalType.Days, 86400},
                {IntervalType.Weeks, 604800}
            };

        public static Interval MinInterval = new Interval(0, IntervalType.MilliSeconds);
        public static Interval MaxInterval = new Interval(long.MaxValue, IntervalType.MilliSeconds);
        public static IntervalType DefaultIntervalType = IntervalType.Seconds;
        public static Interval DefaultInterval = MinInterval;

        public Interval(long value, IntervalType type = IntervalType.Seconds)
        {
            Value = value;
            Type = type;
        }

        public long Value { get; set; }
        public IntervalType Type { get; set; }

        public Interval ConvertTo(IntervalType newType)
        {
            //-------------------------------------------------------------------------------------
            // Return the current value if its already in the request units.
            if (newType == Type)
                return this;

            //-------------------------------------------------------------------------------------
            // Get the number of seconds represented by the current value.
            var seconds = (long) (Value*Conversions[Type]);

            //-------------------------------------------------------------------------------------
            // Convert the number of seconds into the requested interval type.
            var newValue = (long) (seconds/Conversions[newType]);

            return new Interval(newValue, newType);
        }
    }
}