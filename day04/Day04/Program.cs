using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Strategy 1: " + GetStrategy1Answer(File.ReadLines("input.txt").OrderBy(l => l)));

        }

        public static int GetStrategy1Answer(IEnumerable<string> sortedLines)
        {
            var shifts = ParseShifts(sortedLines);

            shifts.GroupBy(shift => shift.GuardId);

            // TODO:
            // group by id
            // get total sleep time across all shifts
            // select the group with the largest duration
            // find out the minute most often asleep

            // wrong
            // Why is there no Max that returns the element????
            var mostSleepy = shifts.OrderByDescending(shift => shift.GetTotalTimeAsleep()).First();

            return 0;
        }

        private static IEnumerable<Shift> ParseShifts(IEnumerable<string> sortedLines)
        {
            int id = -1;
            DateTime startTime = new DateTime();
            var sleepTimes = new List<SleepTime>();

            var shiftChangeRegex = new Regex(@"Guard #(\d+) begins shift", RegexOptions.Compiled);

            DateTime ParseTimeStamp(string line)
            {
                return DateTime.Parse(line.Substring(1, 16));
            }

            foreach (var line in sortedLines)
            {
                switch (line)
                {
                    case string guardChange when line.Contains("begins shift"):
                        var match = shiftChangeRegex.Match(guardChange);
                        if (!match.Success) throw new Exception("How?");
                        if (id != -1)
                        {
                            yield return new Shift(id, sleepTimes);
                            sleepTimes.Clear();
                        }
                        id = int.Parse(match.Groups[1].Value);
                        break;

                    case string fallsAsleep when line.Contains("falls asleep"):
                        startTime = ParseTimeStamp(fallsAsleep);
                        break;

                    case string wakesUp when line.Contains("wakes up"):
                        var endTime = ParseTimeStamp(wakesUp);
                        sleepTimes.Add(new SleepTime(startTime, endTime));
                        break;
                }

            }

            if (id != -1)
            {
                yield return new Shift(id, sleepTimes);
            }
        }
    }

    public struct SleepTime
    {
        public DateTime Start { get; }
        public TimeSpan Duration { get; }

        public SleepTime(DateTime start, DateTime end)
        {
            Start = start;
            Duration = end - start;
        }
    }

    public sealed class Shift
    {
        public int GuardId { get; }
        public ImmutableList<SleepTime> TimesAsleep { get; }

        public TimeSpan GetTotalTimeAsleep() =>
            TimesAsleep.Aggregate(TimeSpan.Zero, (total, sleepTime) => total + sleepTime.Duration);

        public Shift(int guardId, IEnumerable<SleepTime> sleepTimes)
        {
            GuardId = guardId;
            TimesAsleep = sleepTimes.ToImmutableList();
        }
    }
}
