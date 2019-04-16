using System.Linq;
using System.Collections.Generic;

using Contec.Framework.Data.Types;

namespace Contec.Framework.Configuration
{
    public static class ConfigurationExtensions
    {
        private static readonly Dictionary<IntervalType, string> IntervalSuffixes =
            new Dictionary<IntervalType, string>() {
                {IntervalType.MilliSeconds, "InMilliSeconds"},
                {IntervalType.Seconds, "InSeconds"},
                {IntervalType.Minutes, "InMinutes"},
                {IntervalType.Hours, "InHours"},
                {IntervalType.Days, "InDays"},
                {IntervalType.Weeks, "InWeeks"}
            };

        public static Interval GetInterval(this IConfiguration config, string groupName, string settingName, Interval defaultValue = null)
        {
            var intervalType = Interval.DefaultIntervalType;
            var settings = config.Groups[groupName];
            var intervalSettingName = settingName;

            //-------------------------------------------------------------------------------------
            // Find the interval configuration setting that was provided, if any.
            var settingKeys = settings.Keys.ToList();

            if (!settingKeys.Contains(intervalSettingName))
            {
                foreach (var type in IntervalSuffixes.Keys)
                {
                    var tempSetting = string.Format("{0}{1}", settingName, IntervalSuffixes[intervalType]);

                    if (settingKeys.Contains(tempSetting))
                    {
                        intervalSettingName = tempSetting;
                        intervalType = type;
                        break;
                    }
                }
            }

            //-------------------------------------------------------------------------------------
            // Get the configured interval value from the configuration provider.
            var intervalValue = settings.GetSetting(intervalSettingName, long.MinValue);
            
            if (intervalValue == long.MinValue)
            {
                return defaultValue ?? Interval.DefaultInterval;                
            }

            //-------------------------------------------------------------------------------------
            return new Interval(intervalValue, GetIntervalType(intervalSettingName));
        }

        private static IntervalType GetIntervalType(string settingName)
        {
            var name = settingName.ToUpper();

            foreach (var intervalType in IntervalSuffixes.Keys)
            {
                if (name.EndsWith(IntervalSuffixes[intervalType].ToUpper()))
                    return intervalType;
            }

            return Interval.DefaultIntervalType;
        }
    }
}