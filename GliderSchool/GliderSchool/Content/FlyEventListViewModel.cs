using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Common;
using GliderSchool.DataClient;
using GliderSchool.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GliderSchool.Content
{
    public class FlyEventListViewModel : ViewModelBase
    {
        public BindableCollection<FlyEvent> FlyEvents { get; set; } = new BindableCollection<FlyEvent>();
        public FlyEvent SelectedFlyEvent { get; set; }
        public bool IsLoading { get; set; }


        private readonly IMasterNavigationService navigationService;
        public FlyEventListViewModel(IMasterNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
        }

        private async Task LoadData()
        {
            base.OnInitialize();
            var dataClient = new DataClientBase<FlyEvent>("api/FlyEvent");
            var result = await dataClient.GetAll();
            FlyEvents.Clear();
            FlyEvents.AddRange(result);
        }

        protected override async void OnInitialize()
        {
            await LoadData();
        }

        public void Init()
        {
            OnInitialize();
        }

        public async void  RefreshFlyEvents()
        {
            await LoadData();
            IsLoading = false;
        }

        public async void SelectFlyEvent()
        {
            await navigationService.NavigateToViewModelAsync<FlyEventDetailViewModel>(SelectedFlyEvent);
        }

    }
}
