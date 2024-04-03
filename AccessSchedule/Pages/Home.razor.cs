using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor.Extensions;

namespace AccessSchedule.Pages
{
    public partial class Home
    {
      
        public List<AccessScheduleDto> AccessSchedules { get; set; } = AccessScheduleDto.Init();

        public bool _isAddOrRemove { get; set; }
        private bool _switchTimeFormat = false;





    }
}