using Rg.Plugins.Popup.Services;
using SecurityTravelApp.Models;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterAlertPopup : Rg.Plugins.Popup.Pages.PopupPage, I18nable
    {

        private Boolean IsCriticFilterSet = true;
        private Boolean IsWarningFilterSet = true;
        private Boolean IsInfoFilterSet = true;

        public FilterAlertPopup(Boolean pIsCriticFilterSet, Boolean pIsWarningFilterSet, Boolean pIsInfoFilterSet)
        {
            InitializeComponent();

            initFilter(criticToggle, ref IsCriticFilterSet, pIsCriticFilterSet);
            initFilter(warningToggle, ref IsWarningFilterSet, pIsWarningFilterSet);
            initFilter(infoToggle, ref IsInfoFilterSet, pIsInfoFilterSet);


            // correct the behavior of backgroud tapping in android and IOS
            if (Device.RuntimePlatform.Equals(Device.Android) || Device.RuntimePlatform.Equals(Device.iOS))
            {
                BackgroundContainer.IsVisible = true;
                var tapGestureRecognizerContainer = new TapGestureRecognizer();
                tapGestureRecognizerContainer.Tapped += (s, e) =>
                {
                    PopupNavigation.Instance.RemovePageAsync(this);
                };
                BackgroundContainer.GestureRecognizers.Add(tapGestureRecognizerContainer);
            }

            // tap gesture recognizers
            var tapGestureRecognizerCriticFilter = new TapGestureRecognizer();
            criticTap.GestureRecognizers.Add(tapGestureRecognizerCriticFilter);

            var tapGestureRecognizerWarningFilter = new TapGestureRecognizer();
            warningTap.GestureRecognizers.Add(tapGestureRecognizerWarningFilter);

            var tapGestureRecognizerInfoFilter = new TapGestureRecognizer();
            infoTap.GestureRecognizers.Add(tapGestureRecognizerInfoFilter);

            // setting the handler to CriticFilter
            tapGestureRecognizerCriticFilter.Tapped += (s, e) =>
            {
                afterTap(criticToggle, ref IsCriticFilterSet);
                MessagingCenter.Send<FilterAlertPopup, FilterAlertMessage>(this, "ALERTFILTER", new FilterAlertMessage() { Filter = AlertType.Critical, IsSet = IsCriticFilterSet });
            };

            // setting the handler to WarningFilter
            tapGestureRecognizerWarningFilter.Tapped += (s, e) =>
            {
                afterTap(warningToggle, ref IsWarningFilterSet);
                MessagingCenter.Send<FilterAlertPopup, FilterAlertMessage>(this, "ALERTFILTER", new FilterAlertMessage() { Filter = AlertType.Important, IsSet = IsWarningFilterSet });
            };

            // setting the handler to Info Filter
            tapGestureRecognizerInfoFilter.Tapped += (s, e) =>
            {
                afterTap(infoToggle, ref IsInfoFilterSet);
                MessagingCenter.Send<FilterAlertPopup, FilterAlertMessage>(this, "ALERTFILTER", new FilterAlertMessage() { Filter = AlertType.Normal, IsSet = IsInfoFilterSet });
            };


        }

        private void afterTap(View pFilter, ref Boolean pIsSet)
        {
            if (pIsSet)
            {
                pFilter.FadeTo(.3, 80);
                pIsSet = false;
            }
            else
            {
                pFilter.FadeTo(1, 80);
                pIsSet = true;
            }
        }

        private void initFilter(View pFilter, ref Boolean pIsSetToInit, Boolean pIsSet)
        {
            if (pIsSet)
            {
                pFilter.FadeTo(1, 80);
            }
            else
            {
                pFilter.FadeTo(.3, 80);
            }
        }

        public void updateTXT()
        {
        }
    }

    public class FilterAlertMessage
    {
        public AlertType Filter;
        public Boolean IsSet;
    }
}