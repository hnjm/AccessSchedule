using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Data;

namespace AccessSchedule.Component
{
    public partial class HolidaySchedule
    {
        [Inject]
        public ISnackbar Snackbar { get; set; }
        [Parameter]
        public bool IsAddOrRemove { get; set; }
        [Parameter]
        public bool SwitchTimeFormat { get; set; }
        [Parameter]
        public string DayName { get; set; }
        public DateTime? _date { get; set; }
        private string _timeFormat => SwitchTimeFormat ? "hh:mm tt" : "HH:mm";
        public string weekSchedule = "Week Schedule";

        private int Minutes(int j) => (j) switch
        {
            0 => 00,
            1 => 15,
            2 => 30,
            3 => 45,
            _ => 00,
        };
        private string DateTimeStr => string.Join(", ", accessScheduleDto.ScheduleEntries.Select(x => $"{(new DateTime().Add(x.StartTime).ToString(_timeFormat))} -  {(x.EndTime != new TimeSpan(00, 00, 00) ? (new DateTime().Add(x.EndTime).ToString(_timeFormat)) : SwitchTimeFormat ? "12:00 Am" : "24:00")}"));
        private List<(string, string)> dateTimePair = new();
        [Parameter]
        public AccessScheduleDto accessScheduleDto { get; set; } = new();
        public string TextValue { get; set; } = "";



        private Dictionary<TimeSpan, bool> _selections = null!;

        private bool _isBeingSelected;

        protected override void OnInitialized()
        {
            _date = accessScheduleDto.Date;
            GetSelection();
            CheckSelection();
        }

        private void HandleMouseDown(TimeSpan idx)
        {
            if (IsAddOrRemove)
            {
                _selections[idx] = true;
            }
            else
            {
                _selections[idx] = false;
            }
            _isBeingSelected = true;
            CheckSelection();
        }

        private void HandleMouseEnter(TimeSpan idx)
        {
            if (_isBeingSelected)
            {
                if (IsAddOrRemove)
                {
                    _selections[idx] = true;
                }
                else
                {
                    _selections[idx] = false;
                }
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
            TimeSpan tempFirst = new();
            TimeSpan tempSecond = new();
            accessScheduleDto = new();
            accessScheduleDto.Date = _date;

            if (_selections.All(x => x.Value == true))
            {
                accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = _selections.First().Key, EndTime = _selections.Last().Key });

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
                    accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = tempFirst, EndTime = tempSecond });
                    flag = true;
                }
            }
            if (_selections.Last().Value)
            {
                tempSecond = new TimeSpan(00, 00, 00);
                accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = tempFirst, EndTime = tempSecond });
            }

        }

        private bool SheduleExist(TimeSpan start, TimeSpan end)
        {
            return accessScheduleDto.ScheduleEntries.Any(x => start == x.StartTime && end == x.EndTime);
        }

        private bool ScheduleExistInBetween(TimeSpan timeStr)
        {
            bool flag = false;
            flag = accessScheduleDto.ScheduleEntries.Any(x => timeStr >= x.StartTime && timeStr <= x.EndTime);
            if (flag)
            {
                _selections[timeStr] = true;
            }
            return flag;
        }


        private TimeSpan? _startTime = new TimeSpan(00, 45, 00);
        private TimeSpan? _endTime = new TimeSpan(00, 45, 00);
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

        private void AddManually()
        {
            if (_startTime < _endTime)
            {
                bool check = SheduleExist((TimeSpan)_startTime, (TimeSpan)_endTime);

                if (!check)
                {
                    accessScheduleDto.ScheduleEntries.Add(new() { StartTime = (TimeSpan)_startTime, EndTime = (TimeSpan)_endTime });
                    ToggleOpen();
                }
                GetSelection();
            }
            else
            {
                Snackbar.Clear();
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopEnd;
                Snackbar.Add("Start Time must be less than end time", Severity.Error);
            }
        }

        private void GetSelection()
        {
            _selections = new Dictionary<TimeSpan, bool>();
            for (int i = 0; i <= 24; i++)
            {

                _selections[new TimeSpan((i == 24 ? 23 : i), (i == 24 ? 59 : 00), 00)] = ScheduleExistInBetween(new TimeSpan((i == 24 ? 23 : i), (i == 24 ? 59 : 00), 00)) ? true : false;
                if (i < 24)
                {

                    for (int j = 1; j < 4; j++)
                    {
                        _selections[new TimeSpan(i, Minutes(j), 00)] = ScheduleExistInBetween(new TimeSpan(i, Minutes(j), 00)) ? true : false; ;
                    }
                }
            }
            StateHasChanged();
        }
    }
}