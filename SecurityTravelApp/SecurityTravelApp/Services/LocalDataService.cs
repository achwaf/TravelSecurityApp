using SecurityTravelApp.Models;
using SecurityTravelApp.Services.LocalDataServiceUtils.entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SecurityTravelApp.Services
{
    class LocalDataService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.LocalData;

        private SQLiteAsyncConnection database;
        private const String databaseName = "DB.sqlite";

        public LocalDataService() : base(TYPE)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), databaseName);
            //checkDBExists(path);
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
                database.CreateTableAsync<MessageDB>().Wait();
                database.CreateTableAsync<AlertDB>().Wait();
                database.CreateTableAsync<UserLocation>().Wait();
            }
        }

        public async Task<Boolean> resetDatabase()
        {
            await database.DeleteAllAsync<MessageDB>();
            await database.DeleteAllAsync<AlertDB>();
            await database.DeleteAllAsync<UserLocation>();

            return true;
        }

        public async Task<Boolean> saveListAlertToDB(List<Alert> pListe)
        {
            var vListAlertDB = new List<AlertDB>();

            foreach (var agent in pListe)
            {
                AlertDB vAlertDB = new AlertDB();
                // set values
                vListAlertDB.Add(vAlertDB);
            }
            await database.InsertAllAsync(vListAlertDB);
            return true;

        }

        public async Task<List<MessageDB>> getListMessageFromDB()
        {
            var vList = await database.Table<MessageDB>().ToListAsync();
            return vList;
        }


        public async Task<MessageDB> getAgentFromDB(string A, string B)
        {
            if (!String.IsNullOrWhiteSpace(A))
            {

                MessageDB result = await database.Table<MessageDB>().Where(x => true).FirstOrDefaultAsync();
                return result;

            }
            return null;

        }


        public List<Alert> getAlerts()
        {
            List<Alert> listeAlertes = new List<Alert>();


            listeAlertes.Add(new Alert(AlertType.Normal, "Morocco Casablanca", "Title for Alert", "Text providing clearly the information to be given", "2019/03/14", ""));
            listeAlertes.Add(new Alert(AlertType.Normal, "Mali Sud", "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer Text providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer \nText providing clearly the information to be given, but this time I keep wrinting a little longer", "2019/02/10", "2019/03/14 07:45:44"));
            listeAlertes.Add(new Alert(AlertType.Important, "Mali Nord", "Title for important Alert", "Text emphasizing the importance of the information to be given", "2019/03/01", "2019/02/04 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Important, "Turkey Samhini", "Title for critical Alert", "Text displaying emergency over the utter importance of the information to be given", "2019/01/24", "2019/02/10 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Critical, "Europe No one said ever", "Title for Alert", "Text providing clearly the information to be given", "2018/12/14", "2019/01/02 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Normal, "Team Fortress Two", "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer", "2018/08/05", ""));
            listeAlertes.Add(new Alert(AlertType.Normal, "Sky Cloud #445", "Title for Alert", "Text emphasizing the importance of the information to be given", "2018/05/22", "2019/01/02 14:07:44"));
            listeAlertes.Add(new Alert(AlertType.Normal, "World", "Title for Alert", "Text displaying emergency over the utter importance of the information to be given", "2018/04/19", "2019/01/02 14:07:44"));

            return listeAlertes;
        }



        public List<Message> getMessages()
        {
            List<Message> listeMessages = new List<Message>();


            listeMessages.Add(new Message("Bonjour, je viens d'arriver à l'Aéroport", false, ""));
            listeMessages.Add(new Message("j'ai eu un embrouillement avec le chauffeur de taxi", true, "2019/03/19 09:11:40"));
            listeMessages.Add(new Message("j'ai besoin de connaitre les emplacements d'accès rapide", true, "2019/03/19 05:01:33"));
            listeMessages.Add(new Message("Hey, est ce qu'il y a quelqu'un!", true, "2019/03/18 18:14:00"));
            listeMessages.Add(new Message("ce texte se veut assez long pour tester le cas d'affichage d'un message important dont l'auteur a pris tout son temps de le rédiger sans laisser le moindre des détails \n et pourquoi pas un copier coller de text pour rallonger davantage \nce texte se veut assez long pour tester le cas d'affichage d'un message important dont l'auteur a pris tout son temps de le rédiger sans laisser le moindre des détails ", true, "2019/02/27 14:31:04"));

            return listeMessages;
        }


        public List<String> getDefinedMessages()
        {
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
