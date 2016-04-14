using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using UnconsciousBias.Helpers;
using System.Diagnostics;

namespace UnconsciousBias.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        string _Value = "email@domain.com";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public async Task GotoDetailsPage()
        {
            var graphClient = await AuthenticationHelper.GetAuthenticatedClientAsync();

            if (graphClient != null)
            {
                var user = await graphClient.Me.Request().GetAsync();

                var emails = await graphClient.Me
                                            .Messages
                                            .Request()
                                            .Search("\"to:jstur@microsoft.com\"")
                                            .GetAsync();




                foreach (var email in emails)
                {
                    //add call here
                    Debug.WriteLine(email.BodyPreview);
                }
            }

            // can pass value to other screen and do fancy display
            NavigationService.Navigate(typeof(Views.DetailPage), Value);

            await Task.CompletedTask;
        }

        public void GotoSettings() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 0);

        public void GotoPrivacy() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 1);

        public void GotoAbout() =>
            NavigationService.Navigate(typeof(Views.SettingsPage), 2);

    }
}

