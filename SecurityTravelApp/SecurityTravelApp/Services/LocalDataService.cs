using SecurityTravelApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Services
{
    class LocalDataService : ServiceAbstract
    {
        const ServiceType TYPE = ServiceType.LocalData;

        public LocalDataService() : base(TYPE)
        {
        }

        public List<Alert> getAlerts()
        {
            List<Alert> listeAlertes = new List<Alert>();


            listeAlertes.Add(new Alert(AlertType.Normal, "Title for Alert", "Text providing clearly the information to be given"));
            listeAlertes.Add(new Alert(AlertType.Normal, "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer") { });
            listeAlertes.Add(new Alert(AlertType.Important, "Title for important Alert", "Text emphasizing the importance of the information to be given"));
            listeAlertes.Add(new Alert(AlertType.Important, "Title for critical Alert", "Text displaying emergency over the utter importance of the information to be given"));
             listeAlertes.Add(new Alert(AlertType.Critical, "Title for Alert", "Text providing clearly the information to be given"));
            listeAlertes.Add(new Alert(AlertType.Normal, "Title for Alert", "Text providing clearly the information to be given, but this time I keep wrinting a little longer") { });
            listeAlertes.Add(new Alert(AlertType.Normal, "Title for Alert", "Text emphasizing the importance of the information to be given"));
            listeAlertes.Add(new Alert(AlertType.Normal, "Title for Alert", "Text displaying emergency over the utter importance of the information to be given"));

            return listeAlertes;
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
