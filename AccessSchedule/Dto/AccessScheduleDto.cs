namespace AccessSchedule
{
    public class AccessScheduleEntryDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class AccessScheduleDto
    {
        public string? AccessScheduleTitle { get; set; }
        public ICollection<AccessScheduleEntryDto> ScheduleEntries { get; set; } = [];
    }
}
