using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;

namespace AccessSchedule.Component
{
    public partial class Minute
    {
        private MudTheme Theme = new MudTheme();
        [Parameter]
        public TimeSpan Idx { get; set; }
        [Parameter]
        public int? Number { get; set; } = null;
        [Parameter]
        public bool IsSelected { get; set; }

        [Parameter]
        public EventCallback<TimeSpan> OnMouseDown { get; set; }

        [Parameter]
        public EventCallback<TimeSpan> OnMouseEnter { get; set; }

        [Parameter]
        public EventCallback OnMouseUp { get; set; }
        [Parameter]
        public bool SwitchTimeFormat { get; set; }
        [Inject]
        private IBreakpointService BreakpointService { get; set; }

        private int? GetNumber => SwitchTimeFormat == false ? Number : Number > 12 ? (Number - 12) : Number;
        private string GetAmPm()
        {
            var amPm = Number == 5 ? "AM" : Number == 18? "PM" : "";
            return amPm;
        }
        private string GetAmPmClass()
        {

            return SwitchTimeFormat ? "ampm" : "";
        }
        
        private string GetNumberClass()
        {
            return SwitchTimeFormat == false ? (Number == 0 || Number == 24 ? "number2" : "number1"): Number == 0 || (Number - 12) == 12 ? "number2" : "number1";
        }

        private string GetClassLine()
        {
            string cl = Idx >= new TimeSpan(11, 30, 00) && Idx <= new TimeSpan(12, 15, 00) ? "" : "tickline";
            return $"{cl}";
        }

        private string GetClass()
        {
            string large = Number is not null ? " large large-position " : " small-position ";
            string tick = Number == 0 ? "tick edge-tick-position edge-r-8" : Number == 24 ? "tick edge-tick-position tick24-l " : $"tick{large}";
            if(Number == 24)
            {

            }
                return tick;
        }
        private string GetStyle()
        {
            string selected = IsSelected ? $" background-color:{Theme.Palette.SuccessLighten};" : "";
            string border = Number == 0 ? " width:8px; border-top:solid;border-bottom:solid;" : Number == 24 ? "width:4px; border-top:solid;border-bottom:solid;" : "width:8px; border-top:solid;border-bottom:solid;";
            if (Number == 24)
            {

            }
            return $"cursor:auto; {selected}{border}";
        }

        private async Task HandleMouseDown(MouseEventArgs e)
        {
            if (e.Button == 0)
            {
                await OnMouseDown.InvokeAsync(Idx);
            }
        }

        private async Task HandleMouseEnter(MouseEventArgs e)
        {
            if (e.Button == 0)
            {
                await OnMouseEnter.InvokeAsync(Idx);
            }
        }

        private async Task HandleMouseUp()
        {
            await OnMouseUp.InvokeAsync();
        }
    }
}