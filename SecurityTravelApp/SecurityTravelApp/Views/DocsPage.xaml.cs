using SecurityTravelApp.Models;
using SecurityTravelApp.Services;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SecurityTravelApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocsPage : ContentPage, Updatable, I18nable
    {
        LocalDataService localDataSrv;

        private Boolean IsTextFilterSet = true;
        private Boolean IsLinkFilterSet = true;
        private Boolean IsPDFFilterSet = true;
        private Boolean IsWordFilterSet = true;
        private Boolean IsImageFilterSet = true;
        private Boolean IsPowerPointFilterSet = true;
        private Boolean IsAudioFilterSet = true;
        private Boolean IsVideoFilterSet = true;
        private Boolean IsOtherFilterSet = true;

        private List<Document> listDocs;

        private Color FilterOnColor = Color.FromHex("#D12A1A");
        private Color FilterOffColor = Color.FromHex("#C8C8C8");

        public DocsPage(ServiceFactory pSrvFactory, NavigationParams pParam)
        {
            InitializeComponent();

            NavigationBar.initializeContent(pSrvFactory, pParam);
            localDataSrv = (LocalDataService)pSrvFactory.getService(ServiceType.LocalData);


            populate();
            updateTXT();
        }

        private void populateDocs(List<Document> pDocs)
        {
            if (pDocs.Count == 0)
            {
                EmptyInfo.IsVisible = true;
            }
            else
            {
                foreach (var doc in pDocs)
                {
                    DocComp alertComp = new DocComp(doc);
                    DocsContainer.Children.Add(alertComp);
                }
                // adding spacer to be able to scroll up the last elements
                DocsContainer.Children.Add(new BoxView() { HeightRequest = 60 });
            }
        }

        private void filterAlerts()
        {
            if (IsTextFilterSet && IsLinkFilterSet && IsPDFFilterSet && IsWordFilterSet && IsImageFilterSet && IsPowerPointFilterSet && IsAudioFilterSet && IsVideoFilterSet && IsOtherFilterSet)
            {
                FilterIcon.TextColor = FilterOffColor;
            }
            else
            {
                FilterIcon.TextColor = FilterOnColor;
            }
            int notVisibleCounter = 0;
            for (int i = 0; i < listDocs.Count; i++)
            {

                var doc = listDocs[i];

                switch (doc.type)
                {
                    case DocumentType.Text: DocsContainer.Children[i].IsVisible = IsTextFilterSet; break;
                    case DocumentType.Link: DocsContainer.Children[i].IsVisible = IsLinkFilterSet; break;
                    case DocumentType.PDF: DocsContainer.Children[i].IsVisible = IsPDFFilterSet; break;
                    case DocumentType.Word: DocsContainer.Children[i].IsVisible = IsWordFilterSet; break;
                    case DocumentType.Image: DocsContainer.Children[i].IsVisible = IsImageFilterSet; break;
                    case DocumentType.PowerPoint: DocsContainer.Children[i].IsVisible = IsPowerPointFilterSet; break;
                    case DocumentType.Audio: DocsContainer.Children[i].IsVisible = IsAudioFilterSet; break;
                    case DocumentType.Video: DocsContainer.Children[i].IsVisible = IsVideoFilterSet; break;
                    case DocumentType.Other: DocsContainer.Children[i].IsVisible = IsOtherFilterSet; break;
                    default: DocsContainer.Children[i].IsVisible = false; notVisibleCounter++; break;
                }
            }
            if (notVisibleCounter < listDocs.Count || listDocs.Count == 0)
            {
                EmptyFilterInfo.IsVisible = false;
            }
            else
            {
                EmptyFilterInfo.IsVisible = true;
            }
        }

        private async void populate()
        {
            // clear 
            DocsContainer.Children.Clear();

            // get data from server
            // webservices not yet implemented 

            // get data locally
            listDocs = await localDataSrv.getListDoc();
            populateDocs(listDocs);
            filterAlerts();
        }

        public void updateTXT()
        {
            DocTXT.Text = I18n.GetText(AppTextID.DOCUMENTS);
            EmptyTXT.Text = I18n.GetText(AppTextID.EMPTY);
            EmptyTXT2.Text = I18n.GetText(AppTextID.EMPTY);
            FilterEmptyTXT.Text = I18n.GetText(AppTextID.FILTER_EMPTY);
            NavigationBar.updateTXT();
        }

        public void update(NavigationParams pParam)
        {
            // update navigation bar
            NavigationBar.update(pParam);
            // update data
            if (!pParam.NavigationBarOnly)
            {
                populate();
            }
        }
    }
}