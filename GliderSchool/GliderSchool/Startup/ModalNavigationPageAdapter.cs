using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GliderSchool.Startup
{
    public interface IMasterNavigationService : INavigationService
    {
        Conductor<ViewModelBase> MainViewModel { get; set; }
        MasterDetailPage MasterDetailPage { get; set; }
        void ShowDetailPageFor(Type viewModelType);
        void ActivateViewModel(ViewModelBase viewModel);
        Task NavigateModalToViewModelAsync<TViewModel>(object parameter = null, bool animated = true);
    }

    public class MasterNavigationService : NavigationPageAdapter, IMasterNavigationService
    {
        private readonly NavigationPage navigationPage;

        public MasterDetailPage MasterDetailPage { get; set; }
        public Conductor<ViewModelBase> MainViewModel { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }

        public MasterNavigationService(NavigationPage navigationPage) : base(navigationPage)
        {
            this.navigationPage = navigationPage;
        }


        #region Navigation Methods

        public async Task NavigateModalToViewModelAsync<TViewModel>(object parameter = null, bool animated = true)
        {
            var view = ViewLocator.LocateForModelType(typeof(TViewModel), null, null);
            await PushModalAsync(view, parameter, animated);
        }

        public void ShowDetailPageFor(Type viewModelType)
        {
            DeactivateViewModel(CurrentViewModel);

            var view = ViewLocator.LocateForModelType(viewModelType, null, null);

            var page = view as Page;

            if (page == null)
                throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));

            MasterDetailPage.Detail = new NavigationPage(page);
            CurrentViewModel = ViewModelLocator.LocateForViewType(page.GetType()) as ViewModelBase;
            ViewModelBinder.Bind(CurrentViewModel, view, null);

            page.Appearing += (s, e) => ActivateView(page);
            page.Disappearing += (s, e) => DeactivateView(page);

            MasterDetailPage.IsPresented = false;

            ActivateViewModel(CurrentViewModel);
        }


        #endregion

        #region Non callable navigation methods


        private Task PushModalAsync(Element view, object parameter, bool animated)
        {
            var page = view as Page;

            if (page == null)
                throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));


            var viewModel = ViewModelLocator.LocateForView(view);

            if (viewModel != null)
            {
                TryInjectParameters(viewModel, parameter);

                ViewModelBinder.Bind(viewModel, view, null);
            }

            page.Appearing += (s, e) => ActivateView(page);
            page.Disappearing += (s, e) => DeactivateView(page);

            return navigationPage.Navigation.PushModalAsync(page, animated);
        }

        #endregion

        #region Helpers

        public void ActivateViewModel(ViewModelBase viewModel)
        {
            if (MainViewModel != null && viewModel != null)
            {
                MainViewModel.ActivateItem(viewModel);
            }
        }

        public void DeactivateViewModel(ViewModelBase viewModel)
        {
            if (MainViewModel != null && viewModel != null)
            {
                MainViewModel.DeactivateItem(viewModel, false);
            }
        }


        private static void DeactivateView(BindableObject view)
        {
            if (view == null)
                return;

            var deactivate = view.BindingContext as IDeactivate;

            if (deactivate != null)
            {
                deactivate.Deactivate(false);
            }
        }

        private static void ActivateView(BindableObject view)
        {
            if (view == null)
                return;

            var activator = view.BindingContext as IActivate;

            if (activator != null)
            {
                activator.Activate();
            }
        }

        #endregion
    }
}