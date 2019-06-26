using SecurityTravelApp.Models;
using SecurityTravelApp.Services.LocalDataServiceUtils.entities;
using SecurityTravelApp.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SecurityTravelApp.Services
{
    public class LocalDataService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.LocalData;

        private SQLiteAsyncConnection database;
        private const String databaseName = "DB.sqlite";
        private const String UserTrackingFlag = "UserTrackingFlag";
        private const String PreferredLang = "PreferredLang";
        private const String UserLoggedInFlag = "UserLoggedInFlag";

        public LocalDataService() : base(TYPE)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName);

            // for test purposes, the database file is deleted and created from scratch
            // since webservices are not yet implemented, this is a straight forward way to test
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            checkDBExists(path);


        }

        public async Task<Boolean> InitWithTestData()
        {
            // after the database is recreated, we fill it with test data
            await fillDatabase();
            return true;
        }

        private async Task<Boolean> fillDatabase()
        {
            await saveListMessage(getMessages());
            await saveListAlert(getAlerts());
            await saveListDocs(getDocs());
            await saveListPositions(getLocations());

            return true;
        }



        private void checkDBExists(String pPath)
        {
            if (File.Exists(pPath))
            {
                // connect to database
                database = new SQLiteAsyncConnection(pPath);
            }
            else
            {
                // create new database
                database = new SQLiteAsyncConnection(pPath);
                database.CreateTableAsync<MessageDB>();
                database.CreateTableAsync<AlertDB>();
                database.CreateTableAsync<LocationDB>();
                database.CreateTableAsync<AudioRecordDB>();
                database.CreateTableAsync<DocDB>();
            }
        }

        public async Task<Boolean> resetDatabase()
        {
            await database.DeleteAllAsync<MessageDB>();
            await database.DeleteAllAsync<AlertDB>();
            await database.DeleteAllAsync<LocationDB>();
            await database.DeleteAllAsync<AudioRecordDB>();
            await database.DeleteAllAsync<DocDB>();

            return true;
        }

        public async Task<Boolean> toggleSendable(Sendable pObject, Boolean pIsSendable)
        {
            pObject.IsSendable = pIsSendable;
            await database.UpdateAsync(pObject);

            return true;
        }

        public async Task<Boolean> updateToDB(Object pObject)
        {
            var result = await database.UpdateAsync(pObject);
            return true;
        }

        public async Task<Boolean> deleteDataOlderThan(DateTime pLimitDate)
        {
            int nbMsgDeleted, nbAlertDeleted, nbLocaDeleted, nbAudioDeleted, nbDocumentDeleted;
            try
            {
                nbMsgDeleted = await database.Table<MessageDB>().DeleteAsync(x => x.DateSent < pLimitDate);
                nbAlertDeleted = await database.Table<AlertDB>().DeleteAsync(x => x.DateReceived < pLimitDate);
                nbLocaDeleted = await database.Table<LocationDB>().DeleteAsync(x => x.DateSent < pLimitDate);
                nbAudioDeleted = await database.Table<AudioRecordDB>().DeleteAsync(x => x.DateSent < pLimitDate);
                nbDocumentDeleted = await database.Table<DocDB>().DeleteAsync(x => x.DateReceived < pLimitDate);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        #region flags

        static public void setUserLoggedInFlag(Boolean pValue)
        {
            Preferences.Set(UserLoggedInFlag, pValue);
        }

        static public Boolean getUserLoggedInFlag()
        {
            return Preferences.Get(UserLoggedInFlag, false);
        }


        static public void setUserTrackingFlag(Boolean pValue)
        {
            Preferences.Set(UserTrackingFlag, pValue);
        }


        static public Boolean getUserTrackingFlag()
        {
            return Preferences.Get(UserTrackingFlag, false);
        }


        static public void setLanguagePreference(AppLanguage pValue)
        {
            Preferences.Set(PreferredLang, pValue.ToString());
        }


        static public AppLanguage getLanguagePreference()
        {
            String value = Preferences.Get(PreferredLang, AppLanguage.FR.ToString());
            return (AppLanguage)Enum.Parse(typeof(AppLanguage), value);
        }

        #endregion flags


        #region location

        public async Task<Boolean> savePosition(Geoposition pGeoposition)
        {
            LocationDB location = new LocationDB()
            {
                Accuracy = pGeoposition.Accuracy,
                Altitude = pGeoposition.Altitude,
                Longitude = pGeoposition.Longitude,
                Latitude = pGeoposition.Latitude,
                Provider = pGeoposition.Provider,
                DateCheckin = pGeoposition.Date,
                ID = pGeoposition.ID,
                IsSOS = pGeoposition.IsSOS
            };

            await database.InsertAsync(location);
            return true;
        }

        public async Task<Boolean> saveListPositions(List<Geoposition> pList)
        {
            foreach (var position in pList)
            {
                LocationDB location = new LocationDB()
                {
                    Accuracy = position.Accuracy,
                    Altitude = position.Altitude,
                    Longitude = position.Longitude,
                    Latitude = position.Latitude,
                    Provider = position.Provider,
                    DateCheckin = position.Date,
                    ID = position.ID,
                    IsSOS = position.IsSOS
                };
                var number = await database.InsertAsync(location);
            }

            return true;
        }

        public async Task<List<Geoposition>> getListLocation()
        {
            var vList = await database.Table<LocationDB>().OrderByDescending<DateTime>(x => x.DateCheckin).ToListAsync();
            var vListGeoposition = new List<Geoposition>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var locationDB in vList)
                {
                    Geoposition geoposition = new Geoposition()
                    {
                        Accuracy = locationDB.Accuracy,
                        Altitude = locationDB.Altitude,
                        Longitude = locationDB.Longitude,
                        Latitude = locationDB.Latitude,
                        Provider = locationDB.Provider,
                        Date = locationDB.DateCheckin,
                        ID = locationDB.ID,
                        IsSOS = locationDB.IsSOS
                    };
                    vListGeoposition.Add(geoposition);
                }
            }
            return vListGeoposition;
        }

        public async Task<LocationDB> getLocationDB(Geoposition pPosition)
        {
            var location = await database.Table<LocationDB>().FirstAsync(x => x.ID == pPosition.ID);
            return location;
        }

        public async Task<List<Geoposition>> getListLocationForSync()
        {
            var vList = await database.Table<LocationDB>().Where(x => x.IsSendable && !x.IsSent).ToListAsync();
            var vListGeoposition = new List<Geoposition>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var locationDB in vList)
                {
                    Geoposition geoposition = new Geoposition()
                    {
                        Accuracy = locationDB.Accuracy,
                        Altitude = locationDB.Altitude,
                        Longitude = locationDB.Longitude,
                        Latitude = locationDB.Latitude,
                        Provider = locationDB.Provider,
                        Date = locationDB.DateCheckin,
                        ID = locationDB.ID,
                        IsSOS = locationDB.IsSOS
                    };
                    vListGeoposition.Add(geoposition);
                }
            }
            return vListGeoposition;
        }

        public async Task<Geoposition> getLastPosition()
        {
            Geoposition result = null;
            var vTable = database.Table<LocationDB>().OrderByDescending<DateTime>(x => x.DateCheckin);
            LocationDB location = await vTable.FirstOrDefaultAsync();

            if (location != null)
            {
                result = new Geoposition()
                {
                    Accuracy = location.Accuracy,
                    Latitude = location.Latitude,
                    Altitude = location.Altitude,
                    Longitude = location.Longitude,
                    Date = location.DateCheckin,
                    ID = location.ID,
                    IsSOS = location.IsSOS
                };
            }
            return result;
        }

        #endregion location


        #region Document

        public async Task<Boolean> saveDocument(Document pDoc)
        {
            DocDB docDB = new DocDB()
            {
                ID = pDoc.ID,
                Region = pDoc.region,
                Title = pDoc.title,
                Description = pDoc.description,
                Data = pDoc.data,
                FileExtension = pDoc.fileExtension,
                DateReceived = pDoc.dateReceived,
                IsSeen = pDoc.isSeen,
                DateSeen = pDoc.dateSeen
            };

            await database.InsertAsync(docDB);
            return true;
        }

        public async Task<DocDB> getDocDB(Document pDoc)
        {
            var doc = await database.Table<DocDB>().FirstAsync(x => x.ID == pDoc.ID);
            return doc;
        }


        public async Task<Boolean> saveListDocs(List<Document> pListe)
        {
            var vListDocDB = new List<DocDB>();

            foreach (var doc in pListe)
            {
                DocDB docDB = new DocDB();
                // set values
                docDB.ID = doc.ID;
                docDB.Data = doc.data;
                docDB.DateSeen = doc.dateSeen;
                docDB.Description = doc.description;
                docDB.DateReceived = doc.dateReceived;
                docDB.Region = doc.region;
                docDB.Type = doc.type;
                docDB.Title = doc.title;
                docDB.FileExtension = doc.fileExtension;
                docDB.IsSeen = doc.isSeen;
                vListDocDB.Add(docDB);
            }
            await database.InsertAllAsync(vListDocDB);
            return true;

        }


        public async Task<List<Document>> getListDoc()
        {
            var vList = await database.Table<DocDB>().OrderByDescending<DateTime>(x => x.DateReceived).ToListAsync();
            var vListDoc = new List<Document>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var docDB in vList)
                {
                    Document document = new Document();
                    document.ID = docDB.ID;
                    document.data = docDB.Data;
                    document.dateSeen = docDB.DateSeen;
                    document.dateReceived = docDB.DateReceived;
                    document.region = docDB.Region;
                    document.description = docDB.Description;
                    document.type = docDB.Type;
                    document.title = docDB.Title;
                    document.fileExtension = docDB.FileExtension;
                    document.isSeen = docDB.IsSeen;
                    vListDoc.Add(document);
                }
            }
            return vListDoc;
        }


        #endregion Document


        #region AudioRecord

        public async Task<Boolean> saveAudioRecord(AudioRecord pAudioRecord)
        {
            AudioRecordDB location = new AudioRecordDB()
            {
                ID = pAudioRecord.ID,
                AudioFile = pAudioRecord.audioFile,
                DateSent = pAudioRecord.dateSent,
                DateRecorded = pAudioRecord.dateRecorded,
                IsSent = pAudioRecord.isSent,
                AudioLabel = pAudioRecord.audioLabel
            };

            await database.InsertAsync(location);
            return true;
        }


        public async Task<List<AudioRecord>> getListAudioRecord()
        {
            var vList = await database.Table<AudioRecordDB>().ToListAsync();
            var vListAudioRecord = new List<AudioRecord>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var audioDB in vList)
                {
                    AudioRecord audioRecord = new AudioRecord()
                    {
                        ID = audioDB.ID,
                        audioFile = audioDB.AudioFile,
                        dateSent = audioDB.DateSent,
                        dateRecorded = audioDB.DateRecorded,
                        isSent = audioDB.IsSent,
                        audioLabel = audioDB.AudioLabel
                    };
                    vListAudioRecord.Add(audioRecord);
                }
            }
            return vListAudioRecord;
        }

        public async Task<List<AudioRecord>> getListAudioRecordForSync()
        {
            var vList = await database.Table<AudioRecordDB>().Where(x => x.IsSendable && !x.IsSent).ToListAsync();
            var vListAudioRecord = new List<AudioRecord>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var audioDB in vList)
                {
                    AudioRecord audioRecord = new AudioRecord()
                    {
                        ID = audioDB.ID,
                        audioFile = audioDB.AudioFile,
                        dateSent = audioDB.DateSent,
                        dateRecorded = audioDB.DateRecorded,
                        isSent = audioDB.IsSent,
                        audioLabel = audioDB.AudioLabel
                    };
                    vListAudioRecord.Add(audioRecord);
                }
            }
            return vListAudioRecord;
        }

        public async Task<AudioRecordDB> getAudioRecordDB(AudioRecord pAudioRecord)
        {
            var audioRecordDB = await database.Table<AudioRecordDB>().FirstAsync(x => x.ID == pAudioRecord.ID);
            return audioRecordDB;
        }

        #endregion AudioRecord


        #region Alert

        public async Task<Boolean> saveAlert(Alert pALert)
        {
            AlertDB alertDB = new AlertDB();
            alertDB.ID = pALert.ID;
            alertDB.Text = pALert.text;
            alertDB.DateSeen = pALert.dateSeen;
            alertDB.DateReceived = pALert.dateReceived;
            alertDB.Region = pALert.region;
            alertDB.Type = pALert.type;
            alertDB.Title = pALert.title;
            alertDB.IsSeen = pALert.isSeen;
            await database.InsertAsync(alertDB);
            return true;
        }

        public async Task<AlertDB> getAlertDB(Alert pAlert)
        {
            var alert = await database.Table<AlertDB>().FirstAsync(x => x.ID == pAlert.ID);
            return alert;
        }


        public async Task<Boolean> saveListAlert(List<Alert> pListe)
        {
            var vListAlertDB = new List<AlertDB>();

            foreach (var alert in pListe)
            {
                AlertDB alertDB = new AlertDB();
                // set values
                alertDB.ID = alert.ID;
                alertDB.Text = alert.text;
                alertDB.DateSeen = alert.dateSeen;
                alertDB.DateReceived = alert.dateReceived;
                alertDB.Region = alert.region;
                alertDB.Type = alert.type;
                alertDB.Title = alert.title;
                alertDB.IsSeen = alert.isSeen;
                vListAlertDB.Add(alertDB);
            }
            await database.InsertAllAsync(vListAlertDB);
            return true;

        }


        public async Task<List<Alert>> getListAlert()
        {
            var vList = await database.Table<AlertDB>().OrderByDescending<DateTime>(x => x.DateReceived).ToListAsync();
            var vListAlert = new List<Alert>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var alertDB in vList)
                {
                    Alert alert = new Alert();
                    alert.ID = alertDB.ID;
                    alert.text = alertDB.Text;
                    alert.dateSeen = alertDB.DateSeen;
                    alert.dateReceived = alertDB.DateReceived;
                    alert.region = alertDB.Region;
                    alert.type = alertDB.Type;
                    alert.title = alertDB.Title;
                    alert.isSeen = alertDB.IsSeen;
                    vListAlert.Add(alert);
                }
            }
            return vListAlert;
        }


        #endregion Alert


        #region Message


        public async Task<MessageDB> getMessageDB(Message pMessage)
        {
            var message = await database.Table<MessageDB>().FirstAsync(x => x.ID == pMessage.ID);
            return message;
        }


        public async Task<Boolean> saveMessage(Message pMessage)
        {
            MessageDB message = new MessageDB()
            {
                DateSent = pMessage.dateSent,
                IsSent = pMessage.isSent,
                Text = pMessage.text,
                ID = pMessage.ID
            };

            await database.InsertAsync(message);
            return true;
        }

        public async Task<Boolean> saveListMessage(List<Message> pListe)
        {
            if (pListe != null && pListe.Count > 0)
            {
                var vListMsgDB = new List<MessageDB>();

                foreach (var message in pListe)
                {
                    MessageDB messageDB = new MessageDB();
                    // set values
                    messageDB.DateSent = message.dateSent;
                    messageDB.IsSent = message.isSent;
                    messageDB.Text = message.text;
                    messageDB.ID = message.ID;
                    vListMsgDB.Add(messageDB);
                }
                await database.InsertAllAsync(vListMsgDB);
            }
            return true;
        }

        public async Task<List<Message>> getListMessage()
        {
            // order descend by Date sent it is time of message in milliseconds
            var vList = await database.Table<MessageDB>().OrderByDescending<DateTime>(x => x.DateSent).ToListAsync();
            var vListMsg = new List<Message>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var msg in vList)
                {
                    Message message = new Message();
                    message.text = msg.Text;
                    message.isSent = msg.IsSent;
                    message.dateSent = msg.DateSent;
                    message.ID = msg.ID;
                    vListMsg.Add(message);
                }
            }
            return vListMsg;
        }

        public async Task<List<Message>> getListMessageForSync()
        {
            var vList = await database.Table<MessageDB>().Where(x => x.IsSendable && !x.IsSent).ToListAsync();
            var vListMsg = new List<Message>();

            if (vList != null && vList.Count > 0)
            {
                foreach (var msg in vList)
                {
                    Message message = new Message();
                    message.text = msg.Text;
                    message.isSent = msg.IsSent;
                    message.dateSent = msg.DateSent;
                    message.ID = msg.ID;
                    vListMsg.Add(message);
                }
            }
            return vListMsg;
        }


        #endregion Message


        #region Count operations


        public async Task<int> getListAlertCount()
        {
            var count = await database.Table<AlertDB>().CountAsync();
            return count;
        }

        public async Task<int> getListMsgCount()
        {
            var count = await database.Table<MessageDB>().CountAsync();
            return count;
        }

        public async Task<int> getListLocationCount()
        {
            var count = await database.Table<LocationDB>().CountAsync();
            return count;
        }

        public async Task<int> getListDocCount()
        {
            var count = await database.Table<DocDB>().CountAsync();
            return count;
        }

        public async Task<int> getListAudioCount()
        {
            var count = await database.Table<AudioRecordDB>().CountAsync();
            return count;
        }


        public async Task<int> getCountAlertOlderThan(DateTime pDate)
        {
            var count = await database.Table<AlertDB>().CountAsync(x => x.DateReceived < pDate);
            return count;
        }

        public async Task<int> getCountMsgOlderThan(DateTime pDate)
        {
            var count = await database.Table<MessageDB>().CountAsync(x => x.DateSent < pDate);
            return count;
        }

        public async Task<int> getCountLocationOlderThan(DateTime pDate)
        {
            var count = await database.Table<LocationDB>().CountAsync();
            return count;
        }

        public async Task<int> getCountDocOlderThan(DateTime pDate)
        {
            var count = await database.Table<DocDB>().CountAsync(x => x.DateReceived < pDate);
            return count;
        }

        public async Task<int> getCountAudioOlderThan(DateTime pDate)
        {
            var count = await database.Table<AudioRecordDB>().CountAsync(x => x.DateSent < pDate);
            return count;
        }

        #endregion Count operations



        private List<Alert> getAlerts()
        {
            List<Alert> listeAlertes = new List<Alert>();


            listeAlertes.Add(new Alert(AlertType.Critical, "Europe", "Title for Alert", "Text providing clearly the information to be given", "2019/03/25", ""));
            listeAlertes.Add(new Alert(AlertType.Important, "Mali Sud", "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer", "2019/03/19", ""));
            listeAlertes.Add(new Alert(AlertType.Normal, "Morocco Casablanca", "Title for Alert", "Text providing clearly the information to be given", "2019/03/14", ""));
            listeAlertes.Add(new Alert(AlertType.Important, "Mali Nord", "Title for important Alert", "Text emphasizing the importance of the information to be given", "2019/03/01", "2019/02/04 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Important, "Turkey Samhini", "Title for critical Alert", "Text displaying emergency over the utter importance of the information to be given", "2019/01/24", "2019/02/10 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Normal, "Team Fortress Two", "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer", "2018/08/05", ""));
            listeAlertes.Add(new Alert(AlertType.Normal, "Sky Cloud #445", "Title for Alert", "Text emphasizing the importance of the information to be given", "2018/05/22", "2019/01/02 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Normal, "World", "Title for Alert", "Text displaying emergency over the utter importance of the information to be given", "2018/04/19", "2019/01/02 14:07:44"));

            return listeAlertes;
        }

        private List<Document> getDocs()
        {
            List<Document> listeDocs = new List<Document>();

            listeDocs.Add(new Document(DocumentType.Text, "Africa South", "The Title of the document", "A little description of the document", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", ".extension", "2019/08/03", ""));
            listeDocs.Add(new Document(DocumentType.Text, "Africa West", "Another Title of the document", "This time a little longer description of the document", "<URL_path>", ".extension", "2019/06/30", ""));
            listeDocs.Add(new Document(DocumentType.Link, "Mozambique", "Title", "This time a little longer description of the document", "<URL_path>", ".extension", "2019/05/12", ""));
            listeDocs.Add(new Document(DocumentType.PDF, "Gana", "The evolution of time", "Tic toc tic toc tic toc tic toc toc tic toc tic toc tic toc tic toc tic toc tic toc", "<URL_path>", ".extension", "2019/04/06", ""));
            listeDocs.Add(new Document(DocumentType.Word, "Togo", "The expantion of space", "                                                         ", "<URL_path>", ".extension", "2019/04/04", ""));
            listeDocs.Add(new Document(DocumentType.Excel, "Egypt", "Statistics", "This file is <insert very ambiguous text here>", "<URL_path>", ".extension", "2019/03/26", ""));
            listeDocs.Add(new Document(DocumentType.PowerPoint, "Tunisia", "non complete", "Version 0.7.0", "<URL_path>", ".extension", "2019/02/08", ""));
            listeDocs.Add(new Document(DocumentType.Image, "Instaland", "", "An instant description of the instant document", "<URL_path>", ".extension", "2019/01/16", ""));
            listeDocs.Add(new Document(DocumentType.Video, "Youtubia", "Cats cats and cats", "Meow Meaw and Meiw", "<URL_path>", ".extension", "2018/12/27", ""));
            listeDocs.Add(new Document(DocumentType.Audio, "Area 51", "Alien Encounter", "pchhh pttpt kchh ziiit", "<URL_path>", ".extension", "2018/10/05", ""));
            listeDocs.Add(new Document(DocumentType.Other, "", "Downloadable file", "This executable file does not contain any virus, trust me", "<URL_path>", ".extension", "2018/08/15", ""));

            return listeDocs;
        }



        private List<Message> getMessages()
        {
            List<Message> listeMessages = new List<Message>();
            listeMessages.Add(new Message("Bonjour, je viens d'arriver à l'Aéroport", false, ""));
            listeMessages.Add(new Message("j'ai besoin de connaitre les emplacements d'accès rapide", true, "2019/03/19 05:01:33"));
            listeMessages.Add(new Message("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", true, "2019/02/27 14:31:04"));

            return listeMessages;
        }


        private List<Geoposition> getLocations()
        {
            List<Geoposition> listeLocations = new List<Geoposition>();
            listeLocations.Add(new Geoposition(true));
            listeLocations.Add(new Geoposition(true));
            listeLocations.Add(new Geoposition(true));
            listeLocations.Add(new Geoposition(true));
            listeLocations.Add(new Geoposition(true));

            return listeLocations;
        }

        public List<Message> getTestMessages()
        {
            List<Message> listeMessages = new List<Message>();
            for (int i = 1; i <= 80; i++)
            {
                listeMessages.Add(new Message("message test number " + i, true, "2019/03/19 05:01:33"));
            }
            return listeMessages;
        }


        public List<String> getDefinedMessages()
        {
            // plan to add these defined messages to database
            // and allow update form server

            List<String> listeMessages = new List<string>();


            listeMessages.Add("Je viens d'arriver à l'aéroport");
            listeMessages.Add("J'attends l'enregistrement des bagages");
            listeMessages.Add("On vient de me voler mon sac dans l'aéroport");
            listeMessages.Add("Je suis en train de courir derrière le voleur");
            listeMessages.Add("J'ai trébuché sur une valise et me suis entorsé la cheville, aidez-moi");

            return listeMessages;
        }
    }

}
