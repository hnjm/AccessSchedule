using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using MudBlazor.Extensions;
using System.Data;

namespace AccessSchedule.Component
{
    public partial class HolidaySchedule
    {
        private string selectedItem = string.Empty;
        private DateTime? dateValue;
        private IEnumerable<DateTime?> dates;
        public List<DateTime?> HolidayDate { get; set; }
        [Parameter]
        public List<AccessScheduleDto> AccessSchedules { get; set; } 
        private void RowClicked(string date)
        {
            selectedItem = date;
            _accessScheduleDto = AccessSchedules.FirstOrDefault(x => "Holiday".Equals(x.DayName) && x.Date.ToIsoDateString() == selectedItem);
            GetSelection();

        }

        private async void DecrementDate()
        {
            if(dates.Count() == 1)
            {
                AccessSchedules.FirstOrDefault(x => x.DayName == "Holiday").Date = null;
            }
            if (selectedItem != null)
            {
                AccessSchedules.Remove(AccessSchedules.FirstOrDefault(x => x.Date.ToIsoDateString() == selectedItem && x.DayName == "Holiday"));
            }

        }
        private void IncrementDate()
        {
            if (dateValue == null || AccessSchedules.FirstOrDefault(x => "Holiday".Equals(x.DayName) && x.Date == dateValue) != null)
            {
                return;
            }
            try
            {
                if (dates.Count() == 0)
                {
                    selectedItem = dateValue.ToIsoDateString();
                    _ = AccessSchedules.FirstOrDefault(x => "Holiday".Equals(x.DayName)).Date = dateValue;
                }
                else if (dates.Count() >= 1)
                {
                    AccessSchedules.Add(new AccessScheduleDto { Date = dateValue, DayName = "Holiday" });
                }

                dates = AccessSchedules.Where(x => "Holiday".Equals(x.DayName) && x.Date != null).Select(x => x.Date);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [Parameter]
        public bool IsAddOrRemove { get; set; }
        [Parameter]
        public bool SwitchTimeFormat { get; set; }
        private string _timeFormat => SwitchTimeFormat ? "hh:mm tt" : "HH:mm";

        private int Minutes(int j) => (j) switch
        {
            0 => 00,
            1 => 15,
            2 => 30,
            3 => 45,
            _ => 00,
        };
        private string DateTimeStr => string.Join(", ", _accessScheduleDto.ScheduleEntries.Select(x => $"{(new DateTime().Add(x.StartTime).ToString(_timeFormat))} -  {(x.EndTime != new TimeSpan(00, 00, 00) ? (new DateTime().Add(x.EndTime).ToString(_timeFormat)) : SwitchTimeFormat ? "12:00 Am" : "24:00")}"));
        public AccessScheduleDto _accessScheduleDto { get; set; } 
        public string TextValue { get; set; } = "";

        private Dictionary<TimeSpan, bool> _selections = null!;

        private bool _isBeingSelected;

        protected override void OnInitialized()
        {
            dates = AccessSchedules.Where(x => "Holiday".Equals(x.DayName) && x.Date != null).Select(x => x.Date);
            _accessScheduleDto = AccessSchedules.FirstOrDefault(x => "Holiday".Equals(x.DayName));
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

            _accessScheduleDto.ScheduleEntries = [];
            if (_selections.All(x => x.Value == true))
            {
                _accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = _selections.First().Key, EndTime = _selections.Last().Key });

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
                    _accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = tempFirst, EndTime = tempSecond });
                    flag = true;
                }
            }
            if (_selections.Last().Value)
            {
                tempSecond = new TimeSpan(00, 00, 00);
                _accessScheduleDto.ScheduleEntries.Add(new AccessScheduleEntryDto { StartTime = tempFirst, EndTime = tempSecond });
            }

        }

        private bool ScheduleExistInBetween(TimeSpan timeStr)
        {
            bool flag = false;
            flag = _accessScheduleDto.ScheduleEntries.Any(x => timeStr >= x.StartTime && timeStr <= x.EndTime);
            if (flag)
            {
                _selections[timeStr] = true;
            }
            return flag;
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