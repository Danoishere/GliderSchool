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
    public class MainViewModel : Conductor<ViewModelBase>
    {
        public Screen ActiveScreen { get; set; }

        public NavigationViewModel Navigation { get; set; } = IoC.Get<NavigationViewModel>();

        private IMasterNavigationService navigationService;

        public MainViewModel(IMasterNavigationService navigationService)
        {
            this.navigationService = navigationService;
            navigationService.MainViewModel = this;
        }
    }
}
