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
    public class MasterNavigationItem
    {
        public string Title { get; set; }
        public Type ViewModelType { get; set; }
    }


    public class NavigationViewModel : ViewModelBase
    {
        public MasterNavigationItem SelectedNavigationItem { get; set; }
        public BindableCollection<MasterNavigationItem> Navigation { get; set; } = new BindableCollection<MasterNavigationItem>();

        private IMasterNavigationService navigationService;

        public NavigationViewModel(IMasterNavigationService navigationService)
        {
            this.navigationService = navigationService;
            Navigation.Add(new MasterNavigationItem { Title = "Meine Flüge", ViewModelType = typeof(FlyEventListViewModel) });
        }

        public void SelectNavItem()
        {
            try
            {
                navigationService.ShowDetailPageFor(SelectedNavigationItem.ViewModelType);
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
    }
}
