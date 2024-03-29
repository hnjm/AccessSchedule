using System.Collections.Specialized;

namespace AccessSchedule
{
    public class AccessScheduleEntryDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

    public class AccessScheduleDto
    {
        public string? AccessScheduleTitle { get; set; }
        public string? DayName { get; set; }
        public DateTime? Date { get; set; }
        public ICollection<AccessScheduleEntryDto> ScheduleEntries { get; set; } = [];
        public AccessScheduleDto(string dayName = "")
        {
            DayName = dayName;
        }
        public static List<AccessScheduleDto> Init()
        {
            return new List<AccessScheduleDto> {
                new AccessScheduleDto("Sun"),
                new AccessScheduleDto("Mon"),
                new AccessScheduleDto("Tue"),
                new AccessScheduleDto("Wed"),
                new AccessScheduleDto("Thu"),
                new AccessScheduleDto("Fri"),
                new AccessScheduleDto("Sat"),
            };
        }
    }

}
