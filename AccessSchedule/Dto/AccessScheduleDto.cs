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
        public ICollection<AccessScheduleEntryDto> ScheduleEntries { get; set; } = [];
    }
    public enum WeedDays
    {
        Sun,
        Mon,
        Tue,
        Wed,
        Thu,
        Fri,
        Sat

    }
}
