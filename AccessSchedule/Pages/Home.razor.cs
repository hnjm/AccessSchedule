using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor.Extensions;

namespace AccessSchedule.Pages
{
    public partial class Home
    {
        private string selectedItem = null;
        private DateTime? dateValue;
        private string otherValue;
    
    public List<AccessScheduleDto> AccessSchedules { get; set; } = AccessScheduleDto.Init();
        public List<DateTime?> HolidayDate { get; set; }
        public bool _isAddOrRemove {  get; set; }
        private bool _switchTimeFormat = false;
        private Virtualize<string> virtualizeComponent;

        private async void DecrementDate()
        {
            if(selectedItem != null)
            {
                AccessSchedules.Remove(AccessSchedules.FirstOrDefault(x => x.Date.ToString() == selectedItem));
                await virtualizeComponent.RefreshDataAsync();

            }

        }
        private async void IncrementDate()
        {
            AccessSchedules.Add(new AccessScheduleDto { Date = dateValue});
            await virtualizeComponent.RefreshDataAsync();
        }

        private async ValueTask<ItemsProviderResult<string>> LoadItems(ItemsProviderRequest request)
        {
            HolidayDate = AccessSchedules.Select(x => x.Date).ToList() ?? [];
            var subset = HolidayDate.Skip(request.StartIndex).Take(request.Count).ToList();
            return new ItemsProviderResult<string>(subset.Select(x => x.ToIsoDateString()), HolidayDate.Count);
        }

    }
}