using Caliburn.Micro;
using Caliburn.Micro.Xamarin.Forms;
using GliderSchool.Content;
using System;
using System.Collections;
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

    public class MasterNavigationService : IMasterNavigationService
    {
        internal class ViewModelPair
        {
            public Page View { get; set; }
            public ViewModelBase ViewModel { get; set; }
        }

        private NavigationPage navigationPage;

        public MasterDetailPage MasterDetailPage { get; set; }
        public Conductor<ViewModelBase> MainViewModel { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }

        public MasterNavigationService(NavigationPage navigationPage)
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
            DeactivateCurrentModel();
            var viewPair = ForViewModel(viewModelType);
            var newNavigationPage = new NavigationPage(viewPair.View);
            MasterDetailPage.Detail = newNavigationPage;
            navigationPage = newNavigationPage;
            CurrentViewModel = viewPair.ViewModel;
            MasterDetailPage.IsPresented = false;
            ActivateCurrentModel();
        }

        public Task GoBackAsync(bool animated = true)
        {
            return navigationPage.Navigation.PopAsync(animated);
        }

        public async Task NavigateToViewAsync(Type viewType, object parameter = null, bool animated = true)
        {
            DeactivateCurrentModel();
            var viewPair = ForView(viewType, parameter);
            CurrentViewModel = viewPair.ViewModel;
            await PushAsync(viewPair.View, parameter, animated);
            ActivateCurrentModel();
        }

        public async Task NavigateToViewAsync<TViewType>(object parameter = null, bool animated = true)
        {
            DeactivateCurrentModel();
            var viewPair = ForView(typeof(TViewType), parameter);
            CurrentViewModel = viewPair.ViewModel;
            await PushAsync(viewPair.View, parameter, animated);
            ActivateCurrentModel();
        }

        public async Task NavigateToViewModelAsync(Type viewModelType, object parameter = null, bool animated = true)
        {
            DeactivateCurrentModel();
            var viewPair = ForViewModel(viewModelType, parameter);
            CurrentViewModel = viewPair.ViewModel;
            await PushAsync(viewPair.View, parameter, animated);
            ActivateCurrentModel();
        }

        public async Task NavigateToViewModelAsync<TViewModel>(object parameter = null, bool animated = true)
        {
            DeactivateCurrentModel();
            var viewPair = ForViewModel(typeof(TViewModel), parameter);
            CurrentViewModel = viewPair.ViewModel;
            await PushAsync(viewPair.View, parameter, animated);
            ActivateCurrentModel();
        }

        public async Task GoBackToRootAsync(bool animated = true)
        {
            await navigationPage.Navigation.PopToRootAsync(animated);
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
                //TryInjectParameters(viewModel, parameter);
                ViewModelBinder.Bind(viewModel, view, null);
            }

            page.Appearing += (s, e) => ActivateView(page);
            page.Disappearing += (s, e) => DeactivateView(page);

            return navigationPage.Navigation.PushModalAsync(page, animated);
        }

        private Task PushAsync(Element view, object parameter, bool animated)
        {
            var page = view as Page;

            if (page == null)
                throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));
 

            var viewModel = ViewModelLocator.LocateForView(view);

            if (viewModel != null)
            {
                //TryInjectParameters(viewModel, parameter);
                ViewModelBinder.Bind(viewModel, view, null);
            }

            page.Appearing += (s, e) => ActivateView(page);
            page.Disappearing += (s, e) => DeactivateView(page);

            return navigationPage.Navigation.PushAsync(page, animated);
        }

        #endregion

        #region Helpers

        public void ActivateCurrentModel()
        {
            ActivateViewModel(CurrentViewModel);
        }

        public void DeactivateCurrentModel(bool close = false)
        {
            DeactivateViewModel(CurrentViewModel);
        }

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

        private static ViewModelPair ForViewModel(Type viewModelType, object parameters = null)
        {
            try
            {
                var view = ViewLocator.LocateForModelType(viewModelType, null, null) as Page;
                if (view == null)
                    throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));

                var viewModel = ViewModelLocator.LocateForView(view) as ViewModelBase;
                ViewModelBinder.Bind(viewModel, view, parameters);
                TrySetParameters(viewModel, parameters);
                return new ViewModelPair { View = view, ViewModel = viewModel };
            }
            catch (Exception e)
            { 
                throw;
            }
            
        }

        private static ViewModelPair ForView(Type viewType, object parameters = null)
        {
            try
            {
                var view = ViewLocator.LocateForModelType(viewType, null, parameters);
                var viewModel = ViewModelLocator.LocateForView(view) as ViewModelBase;
                var page = view as Page;
                if (page == null)
                    throw new NotSupportedException(String.Format("{0} does not inherit from {1}.", view.GetType(), typeof(Page)));

                ViewModelBinder.Bind(viewModel, page, parameters);
                TrySetParameters(viewModel, parameters);
                return new ViewModelPair { View = page, ViewModel = viewModel };
            }
            catch (Exception e)
            {
                throw;
            }
            
        }


        public static void TrySetParameters(ViewModelBase viewModel, object parameters)
        {
            if(parameters == null)
            {
                return;
            }

            if(parameters is IEnumerable)
            {
                var parameterList = parameters as IEnumerable<KeyValuePair<string, object>>;

                foreach (var parameter in parameterList)
                {
                    var property = viewModel.GetType().GetPropertyCaseInsensitive(parameter.Key);
                    if(property != null)
                    {
                        property.SetValue(viewModel, parameter.Value);
                    }
                }
                
            }
            else
            {
                var typeName = parameters.GetType().Name;
                var property = viewModel.GetType().GetPropertyCaseInsensitive(typeName);
                if(property != null)
                {
                    property.SetValue(viewModel, parameters);
                    return;
                }

                typeName = "Parameter";
                property = viewModel.GetType().GetPropertyCaseInsensitive(typeName);
                if (property != null)
                {
                    property.SetValue(viewModel, parameters);
                    return;
                }
            }
        }

        #endregion
    }
}