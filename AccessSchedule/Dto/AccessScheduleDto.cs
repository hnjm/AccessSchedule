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
    }
}
