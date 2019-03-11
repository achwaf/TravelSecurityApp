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
