﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StatsN
{
    public partial class Statsd
    {
        /// <summary>
        /// Simple Counter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public Task CountAsync(string name, long count = 1) => LogMetricAsync(name, count, Constants.Metrics.COUNT);
        /// <summary>
        /// arbitrary values, which can be recorded.  
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task GaugeAsync(string name, long value) => LogMetricAsync(name, value, Constants.Metrics.GAUGE);
        /// <summary>
        /// Add or remove from gauge instead of setting
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task GaugeDeltaAsync(string name, long value) => LogMetricAsync(name, value >= 0? $"+{value.ToString()}": value.ToString(), Constants.Metrics.GAUGE);
        /// <summary>
        /// How long something takes to complete. StatsD figures out percentiles, average (mean), standard deviation, sum, lower and upper bounds for the flush interval
        /// </summary>
        /// <param name="name"></param>
        /// <param name="milliseconds">time to log in ms</param>
        /// <returns></returns>
        public Task TimingAsync(string name, long milliseconds) => LogMetricAsync(name, milliseconds, Constants.Metrics.TIMING);
        /// <summary>
        /// How long something takes to complete. StatsD figures out percentiles, average (mean), standard deviation, sum, lower and upper bounds for the flush interval
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actionToTime">action to instrument</param>
        /// <returns></returns>
        public Task TimingAsync(string name, Action actionToTime)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            actionToTime?.Invoke();
            stopwatch.Stop();
            return LogMetricAsync(name, stopwatch.ElapsedMilliseconds, Constants.Metrics.TIMING);
        }
        /// <summary>
        /// StatsD supports counting unique occurences of events between flushes, using a Set to store all occuring events
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Task SetAsync(string name, long value) => LogMetricAsync(name, value, Constants.Metrics.SET);

    }
}
