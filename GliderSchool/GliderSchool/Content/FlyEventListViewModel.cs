using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
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

        protected override void OnInitialize()
        {
            base.OnInitialize();
            FlyEvents.Add(new FlyEvent { Title = "Engstlegenalp Überflug", Description = "Wir gehen auf die Engstlegenalp und machen einen tollen Tag", Remarks = "Warme kleider mitnehmen" });
            FlyEvents.Add(new FlyEvent { Title = "Hummel Abendflug", Description = "Wir gehen auf den Hummel und machen einen tollen Tag", Remarks = "Testmaterial dabei" });
            FlyEvents.Add(new FlyEvent { Title = "Rigi Soaring", Description = "Wir gehen auf die Rigi und soaren", Remarks = "Warme kleider mitnehmen" });
        }

        public void Init()
        {
            OnInitialize();
        }

        public async void  RefreshFlyEvents()
        {
            await Task.Delay(1000);
            IsLoading = false;
        }

        public async void SelectFlyEvent()
        {
            await navigationService.NavigateToViewModelAsync<FlyEventDetailViewModel>();
        }

    }
}
