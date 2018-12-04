using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    static class Extensions
    {
        public static TimeSpan Sum<T>(this IEnumerable<T> sequence, Func<T, TimeSpan> selector) =>
            sequence.Aggregate(TimeSpan.Zero, (total, container) => total + selector(container));

        public static MinutesSlept Sum<T>(this IEnumerable<T> sequence, Func<T, MinutesSlept> selector) =>
            sequence.Aggregate(MinutesSlept.Zero, (total, container) => total + selector(container));

        // WHY IS THIS NOT BUILT IN
        public static T MaxElement<T, U>(this IEnumerable<T> sequence, Func<T, U> selector) where U : IComparable<U>
        {
            return sequence.Aggregate((i1, i2) => selector(i1).CompareTo(selector(i2)) == 1 ? i1 : i2);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var shiftGroups =
                ParseShifts(File.ReadLines("input.txt").OrderBy(l => l))
                    .GroupBy(shift => shift.GuardId)
                    .ToArray();

            Console.WriteLine("Strategy 1: " + GetStrategy1Answer(shiftGroups));
            Console.WriteLine("Part B: " + GetPart2Answer(shiftGroups));

            Console.ReadKey();
        }

        public static int GetStrategy1Answer(IEnumerable<IGrouping<int, Shift>> shiftGroups)
        {
            var groupOfSleepiestGuard =
                shiftGroups.Select(group => (group, group.Sum(s => s.GetTotalTimeAsleep()))).MaxElement(tuple => tuple.Item2).group;

            var minuteSleepCounts = new ConcurrentDictionary<int, int>();

            foreach (var minute in groupOfSleepiestGuard.SelectMany(shift => shift.GetMinutesAsleep()))
            {
                minuteSleepCounts.AddOrUpdate(minute, 1, (key, total) => total + 1);
            }

            return groupOfSleepiestGuard.Key * minuteSleepCounts.MaxElement(pair => pair.Value).Key;
        }

        public static int GetPart2Answer(IEnumerable<IGrouping<int, Shift>> shiftGroups)
        {
            var result = shiftGroups.Select(group =>
               (id: group.Key,
                mostSlept: group.Sum(shift => shift.GetMinutesSlept()).GetMostSleptMinute()))
                .MaxElement(pair => pair.mostSlept.Times);

            return result.id * result.mostSlept.Minute;
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

    public struct SleepMinute
    {
        public int Minute { get; }
        public int Times { get; }

        public SleepMinute(int minute, int times)
        {
            Minute = minute;
            Times = times;
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

        public TimeSpan GetTotalTimeAsleep() => TimesAsleep.Sum(s => s.Duration);

        public IEnumerable<int> GetMinutesAsleep()
        {
            foreach (var sleepTime in TimesAsleep)
            {
                var start = sleepTime.Start.Minute;
                var duration = (int)sleepTime.Duration.TotalMinutes;
                for (int minute = start; minute < start + duration; minute++)
                {
                    yield return minute;
                }
            }
        }

        public Shift(int guardId, IEnumerable<SleepTime> sleepTimes)
        {
            GuardId = guardId;
            TimesAsleep = sleepTimes.ToImmutableList();
        }

        public MinutesSlept GetMinutesSlept() => TimesAsleep.Sum(MinutesSlept.From);
    }

    public sealed class MinutesSlept
    {
        private readonly int[] _minutes;

        public static readonly MinutesSlept Zero = new MinutesSlept(new int[60]);

        private MinutesSlept(int[] minutes)
        {
            _minutes = minutes;
        }

        public static MinutesSlept From(SleepTime sleepTime)
        {
            var minutes = new int[60];
            foreach (var index in Enumerable.Range(sleepTime.Start.Minute, (int)sleepTime.Duration.TotalMinutes))
            {
                minutes[index] = 1;
            }

            return new MinutesSlept(minutes);
        }

        public static MinutesSlept operator +(MinutesSlept ms1, MinutesSlept ms2)
        {
            var sum = new int[60];
            for (int i = 0; i < 60; i++)
            {
                sum[i] = ms1._minutes[i] + ms2._minutes[i];
            }

            return new MinutesSlept(sum);
        }

        public SleepMinute GetMostSleptMinute()
        {
            return _minutes.Select((count, index) => new SleepMinute(index, count)).MaxElement(min => min.Times);
        }
    }
}
