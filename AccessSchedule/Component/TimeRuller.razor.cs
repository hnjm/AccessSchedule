using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AccessSchedule.Component
{
    public partial class TimeRuller
    {

        private bool _switchTimeFormat = false;

        private string _timeFormat => _switchTimeFormat ? "hh:mm tt" : "HH:mm";
        public string weekSchedule = "Week Schedule";

        private int Minutes(int j) => (j) switch
        {
            0 => 00,
            1 => 15,
            2 => 30,
            3 => 45,
            _ => 00,
        };
        private string DateTimeStr => string.Join(", ", accessScheduleDto.ScheduleEntries.Select(x => $"{new DateTime().Add(x.StartTime).ToString(_timeFormat)} -  {new DateTime().Add(x.EndTime).ToString(_timeFormat)}"));
        private List<(string, string)> dateTimePair = new();
        [Parameter]
        public AccessScheduleDto accessScheduleDto { get; set; } = new();
        public string TextValue { get; set; } = "";

       
       
        private Dictionary<TimeSpan, bool> _selections = null!;

        private bool _isBeingSelected;

        protected override void OnInitialized()
        {
            _selections =new Dictionary<TimeSpan, bool>();
            for (int i = 0; i < 24; i++)
            {

                _selections[new TimeSpan(i,00, 00)] = ScheduleExist(new TimeSpan(i, 00, 00)) ? true : false ;
                if (i < 24)
                {

                    for (int j = 1; j < 4; j++)
                    {
                        _selections[new TimeSpan(i, Minutes(j), 00)] = ScheduleExist(new TimeSpan(i, Minutes(j), 00)) ? true : false; ;
                    }
                }
            }
            CheckSelection();
        }

        private void HandleMouseDown(TimeSpan idx)
        {
            _selections[idx] = !_selections[idx];
            _isBeingSelected = true;
            CheckSelection();
        }

        private void HandleMouseEnter(TimeSpan idx)
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
            TimeSpan tempFirst = new();
            TimeSpan tempSecond = new();
            accessScheduleDto = new();
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
                accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = tempFirst, EndTime = tempSecond });
            }
        }

        private bool ScheduleExist(TimeSpan timeStr)
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
            
            accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = (TimeSpan)_startTime, EndTime = (TimeSpan)_endTime });
            CheckSelection();
        }
    }
}