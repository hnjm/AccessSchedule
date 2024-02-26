using Microsoft.AspNetCore.Components;

namespace AccessSchedule.Component
{
    public partial class TimeRuller
    {
        private bool _switchTimeFormat = false;
        public string weekSchedule = "Week Schedule";

        private string Minutes(int j) => (j) switch
        {
            0 => "00",
            1 => "15",
            2 => "30",
            3 => "45",
            _ => "",
        };
        private string DateTimeStr => string.Join(", ", accessScheduleDto.ScheduleEntries.Select(x => $"{x.StartTime.ToString("HH:mm")}-{x.EndTime.ToString("HH:mm")}"));
        private List<(string, string)> dateTimePair = new();
        [Parameter]
        public AccessScheduleDto accessScheduleDto { get; set; } = new();
        private string TicLargePosition(int i) => $"{(i / 24.0 * 100)}%";
        private string TicPosition(int i, int j) => $"{((i + j / 4.0) / 24.0 * 100)}%";
        public string TextValue { get; set; } = "";

        public bool _isOpen = false;

        public void ToggleOpen()
        {
            if (_isOpen)
            {
                _isOpen = false;
            }
            else
            {
                _isOpen = true;
            }
        }

        private Dictionary<string, bool> _selections = null!;

        private bool _isBeingSelected;

        protected override void OnInitialized()
        {
            _selections =new Dictionary<string, bool>();
            for (int i = 0; i <= 24; i++)
            {

                _selections[$"{i}:00"] = ScheduleExist($"{i}:00") ? true : false ;
                if (i < 24)
                {

                    for (int j = 1; j < 4; j++)
                    {
                        _selections[$"{i}:{Minutes(j)}"] = ScheduleExist($"{i}:{Minutes(j)}") ? true : false; ;
                    }
                }
            }
            CheckSelection();
        }

        private void HandleMouseDown(string idx)
        {
            _selections[idx] = !_selections[idx];
            _isBeingSelected = true;

        }

        private void HandleMouseEnter(string idx)
        {
            if (_isBeingSelected)
            {
                _selections[idx] = !_selections[idx];
            }

            CheckSelection();
        }

        private void HandleMouseUp()
        {
            _isBeingSelected = false;
        }
        private void CheckSelection()
        {
            bool flag = true;
            string tempFirst = String.Empty;
            string tempSecond = string.Empty;
            accessScheduleDto = new();
            if (_selections.All(x => x.Value == true))
            {
                if (DateTime.TryParse(_selections.First().Key, out var startTime) && DateTime.TryParse(_selections.First().Key, out var endTime))
                {
                    accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = startTime, EndTime = endTime });
                }

                return;
            }
            foreach (var i in _selections)
            {
                if (i.Value is true && flag)
                {
                    tempFirst = i.Key;
                    flag = false;
                }
                if (i.Value is true && flag is false)
                {
                    tempSecond = i.Key;
                }
                if (i.Value is false && flag is false)
                {
                    if (DateTime.TryParse(tempFirst, out var startTime) && DateTime.TryParse(tempSecond, out var endTime))
                    {
                        accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = startTime, EndTime = endTime });
                    }

                    flag = true;
                }
            }
        }

        private bool ScheduleExist(string timeStr)
        {
            bool flag = false;
            if (DateTime.TryParse(timeStr, out var time))
                flag = accessScheduleDto.ScheduleEntries.Any(x => time >= x.StartTime && time <= x.EndTime);
            return flag;
        }
    }
}