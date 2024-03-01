namespace AccessSchedule.Pages
{
    public partial class Home
    {
        public bool _isAddOrRemove {  get; set; }
        private bool _switchTimeFormat = false;


        private void Remove()
        {
            _isAddOrRemove = false;
        }
        private void Add()
        {
            _isAddOrRemove = true;
        }
    }
}