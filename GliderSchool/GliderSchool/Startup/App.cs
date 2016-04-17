using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Content;
using GliderSchool.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GliderSchool
{
    public class App : FormsApplication
    {
        private readonly SimpleContainer container;
        private readonly NavigationPage navigationPage = new NavigationPage();

        public App(SimpleContainer container)
        {

            try
            {
                this.container = container;

                var adapter = new MasterNavigationService(navigationPage);

                container
                    .Instance(navigationPage)
                    .Instance<IMasterNavigationService>(adapter)
                    .PerRequest<NavigationViewModel>()
                    .PerRequest<MainViewModel>()
                    .PerRequest<FlyEventDetailViewModel>()
                    .PerRequest<FlyEventListViewModel>();

                adapter.NavigateToViewModelAsync<FlyEventListViewModel>();

                Initialize();


                DisplayRootViewFor<MainViewModel>();
            }
            catch (Exception e)
            {
                while (e.InnerException != null) { e = e.InnerException; }
                throw e;
            }


        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


    }
}
