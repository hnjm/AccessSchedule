using Microsoft.AspNetCore.Components;

namespace AccessSchedule.Component
{
    public partial class DateComponent
    {
        [Parameter]
        public string IsSelected { get; set; }
        [Parameter] public string Selected { get; set; }
        [Parameter] public string Date { get; set; }
        [Parameter] public EventCallback<string> SelectedChanged { get; set; }
        private string GetStyle()
        {
            return Selected == Date ? "background-color:grey;" :"";
        }

        private async Task CellClicked()
        {
            Selected = Date;
            await SelectedChanged.InvokeAsync(Date);
        }

    }
}