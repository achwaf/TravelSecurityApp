using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SecurityTravelApp.Models;
using SecurityTravelApp.Utils;
using SecurityTravelApp.Views;
using SecurityTravelApp.Views.ViewsUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;

namespace SecurityTravelApp.Services
{
    class AppManagementService : ServiceAbstract
    {

        const ServiceType TYPE = ServiceType.AppManagement;

        private List<NavigationItem> navigationItems;


        private Dictionary<NavigationItemTarget, Page> ExistingPages;
        private Application AppReference;
        private Task syncTask;
        private Timer syncTimer;
        LocalDataService localDataSrv;
        ServerDataService serverDataSrv;

        public Page CurrentPage;


        public AppManagementService() : base(TYPE)
        {
            ExistingPages = new Dictionary<NavigationItemTarget, Page>();
            navigationItems = new List<NavigationItem>();
            fillWithData();

            syncTimer = new Timer();
            syncTimer.Elapsed += new ElapsedEventHandler(OnTimeEventAsync);
            syncTimer.Interval = 300000;  // 5 min
            syncTimer.AutoReset = true;
        }

        public void launchTaskSync()
        {
            if (!syncTimer.Enabled)
            {
                // launch immediately
                Debug.WriteLine("Triggered");
                triggerSyncTask();
                // and after TimeInterval continue looping over
                syncTimer.Start();
            }
        }

        private void OnTimeEventAsync(object source, ElapsedEventArgs e)
        {
            triggerSyncTask();
        }

        public void triggerSyncTask()
        {
            if (syncTask == null || syncTask.IsCompleted)
            {
                syncTask = Task.Run(async () =>
                {
                    syncData();
                });
            }
        }

        public async void syncData()
        {
            { // Run code here to sync data about

                Boolean AllLocationsAreSync = true;
                Boolean AllAudiosAreSync = true;
                Boolean AllMessagesAreSync = true;

                // locations
                var listLocation = await localDataSrv.getListLocationForSync();
                foreach (Geoposition position in listLocation)
                {
                    // send the position to server

                    // if failure to send to server 
                    AllLocationsAreSync = false;

                    // upond result update the database
                    var locationDB = await localDataSrv.getLocationDB(position);
                    locationDB.IsSent = true;
                    locationDB.DateSent = DateTime.Now;
                    await localDataSrv.updateToDB(locationDB);
                }


                // audios

                var listAudioFile = await localDataSrv.getListAudioRecordForSync();
                List<Task<Boolean>> taskList = new List<Task<Boolean>>();
                for (int i = 0; i < listAudioFile.Count; i++)
                {
                    AudioRecord audio = listAudioFile[i];
                    // send the audio to server
                    Debug.WriteLine("audio" + i + " is being sent to server ");
                    var task = serverDataSrv.sendDataToServer();
                    taskList.Add(task);
                }
                // wait for all requests to complete
                Debug.WriteLine("waiting for all");
                Task.WaitAll(taskList.ToArray());
                // process the results 
                for (int i = 0; i < taskList.Count; i++)
                {
                    Task<Boolean> task = taskList[i];
                    AllAudiosAreSync = AllAudiosAreSync && task.Result;
                    if (task.Result)
                    {
                        // upond result update the database
                        var audioDB = await localDataSrv.getAudioRecordDB(listAudioFile[i]);
                        audioDB.IsSent = true;
                        audioDB.DateSent = DateTime.Now;
                        localDataSrv.updateToDB(audioDB);
                    }
                }

                // messages
                var listMessages = await localDataSrv.getListMessageForSync();
                foreach (Message message in listMessages)
                {
                    // send the msg to server

                    // if failure to send to server 
                    AllMessagesAreSync = false;

                    // upond result update the database
                    var messageDB = await localDataSrv.getMessageDB(message);
                    messageDB.IsSent = true;
                    messageDB.DateSent = DateTime.Now;
                    await localDataSrv.updateToDB(messageDB);
                }


                // if all are sync stop the timer
                if (AllAudiosAreSync && AllLocationsAreSync && AllMessagesAreSync)
                {
                    Debug.WriteLine("Timer stopped");
                    syncTimer.Stop();
                }


                // update the current page
                //if (AppReference.MainPage is UpdatablePage)
                //{
                //    UpdatablePage updatblePage = (UpdatablePage)AppReference.MainPage;
                //    updatblePage.update(new NavigationParams() { NavigationBarOnly = false });
                //}

            }
        }

        public void config(Application pAppRef, LocalDataService pLocalSrv, ServerDataService pServerSrv)
        {
            AppReference = pAppRef;
            localDataSrv = pLocalSrv;
            serverDataSrv = pServerSrv;
        }

        public Page lookUpPage(NavigationItemTarget pTarget)
        {
            if (ExistingPages.ContainsKey(pTarget))
            {
                return ExistingPages[pTarget];
            }
            else return null;

        }

        public Type TypeOfNavigationTarget(NavigationItemTarget pTarget)
        {
            switch (pTarget)
            {
                case NavigationItemTarget.Home: return typeof(HomePage);
                case NavigationItemTarget.Messages: return typeof(MessagesPage);
                case NavigationItemTarget.Warnings: return typeof(AlertsPage);
                case NavigationItemTarget.Login: return typeof(LoginPage);
                case NavigationItemTarget.Docs: return typeof(PopupTestPage);
                default: return typeof(PopupTestPage);
            }
        }


        private void navigateToAndSave(Page pPage, NavigationItemTarget pTarget)
        {
            if (!ExistingPages.ContainsKey(pTarget))
            {
                ExistingPages.Add(pTarget, pPage);
            }
            CurrentPage = pPage;
            AppReference.MainPage = CurrentPage;
        }

        public void incementNotif(NavigationItemTarget pType, int pIncrement = 1)
        {
            NavigationItem navItem = navigationItems.Find(x => x.target.Equals(pType));
            // if navItem is null ghir nmchi n3awd n9ra lbac
            navItem.carriesNotif = true;
            navItem.numberOfNotif += pIncrement;

            if (AppReference.MainPage is Updatable)
            {
                Updatable updatblePage = (Updatable)AppReference.MainPage;
                updatblePage.update(new NavigationParams() { navigationUpdateTarget = pType, NavigationBarOnly = true });
            }

        }

        public void navigateTo(NavigationItemTarget pType, ServiceFactory pSrvFactory, NavigationParams pParamAfterNav = null)
        {
            Page targetPage = lookUpPage(pType);
            if (targetPage == null)
            {
                targetPage = (Page)Activator.CreateInstance(TypeOfNavigationTarget(pType), pSrvFactory, pParamAfterNav);// if the page is I18nable , update the text

            }
            else
            {
                // if page is Updatable, update the view
                if (targetPage is Updatable)
                {
                    Updatable updatblePage = (Updatable)targetPage;
                    updatblePage.update(new NavigationParams() { navigationTarget = pType, NavigationBarOnly = false });
                }

                // if the page is I18nable , update the text
                if (targetPage is I18nable)
                {
                    I18nable i18nablePage = (I18nable)targetPage;
                    i18nablePage.updateTXT();
                }
            }
            navigateToAndSave(targetPage, pType);
        }

        public List<NavigationItem> getTheNavigationItems()
        {
            return navigationItems;
        }

        public async Task<Boolean> checkForAllRequiredPermissions()
        {
            await checkForPermission(Permission.Phone);
            await checkForPermission(Permission.Location);
            await checkForPermission(Permission.Microphone);
            await checkForPermission(Permission.Storage);
            await checkForPermission(Permission.Sms);
            return false;
        }

        public async Task<Boolean> checkForPermission(Permission pPermission)
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(pPermission);
                if (status != PermissionStatus.Granted)
                {

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(pPermission);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(pPermission)) status = results[pPermission];

                    if (status == PermissionStatus.Granted)
                    {
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void fillWithData()
        {
            navigationItems.Add(createNavigationItem(2, AppTextID.HOME, NavigationItemTarget.Home, LineAwesomeIcons.Home, false, NavigationItemNotifType.Dot));
            navigationItems.Add(createNavigationItem(4, AppTextID.MESSAGES, NavigationItemTarget.Messages, LineAwesomeIcons.Messages, false));
            navigationItems.Add(createNavigationItem(3, AppTextID.WARNINGS, NavigationItemTarget.Warnings, LineAwesomeIcons.Warnings, false, NavigationItemNotifType.Numerical, NavigationItemState.Current));
            navigationItems.Add(createNavigationItem(5, AppTextID.DOCS, NavigationItemTarget.Docs, LineAwesomeIcons.Docs, false, NavigationItemNotifType.Numerical, NavigationItemState.Shaded));
        }


        private NavigationItem createNavigationItem(int pId, AppTextID pText, NavigationItemTarget pTarget, String pSolidIcon, Boolean pCarriesNotif = true, NavigationItemNotifType pType = NavigationItemNotifType.Numerical, NavigationItemState pState = NavigationItemState.Shaded, int pNumberNotif = 0)
        {
            return new NavigationItem
            {
                id = pId,
                textId = pText,
                font = NavigationItemFont.FASolid,
                icon = pSolidIcon,
                carriesNotif = pCarriesNotif,
                state = pState,
                numberOfNotif = pNumberNotif,
                notifType = pType,
                target = pTarget


            };
        }
    }
}
