using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GliderSchool.Content
{
    public partial class MainView : MasterDetailPage
    {
        public MainView()
        {
            InitializeComponent();
            IoC.Get<IMasterNavigationService>().MasterDetailPage = this;
            Detail = IoC.Get<NavigationPage>();
        }
    }
}
