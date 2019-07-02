using System;
using System.Collections.Generic;
using System.Text;

namespace SecurityTravelApp.Utils
{
    public class I18n
    {
        private Dictionary<AppLanguage, Dictionary<AppTextID, String>> translation = new Dictionary<AppLanguage, Dictionary<AppTextID, String>>();
        private Dictionary<AppTextID, String> dictFR;
        private Dictionary<AppTextID, String> dictEN;
        private static AppLanguage selectedLanguage = AppLanguage.Empty;
        private static I18n instance;


        public static void SelectLang(AppLanguage pLang)
        {
            selectedLanguage = pLang;
        }

        public static AppLanguage SelectedLang()
        {
            return selectedLanguage;
        }

        public static String GetText(AppTextID pID, AppLanguage pSelectedLang = AppLanguage.Empty)
        {
            AppLanguage lang = selectedLanguage;
            String text = "";
            Dictionary<AppTextID, String> dictSelectedLang;
            if (instance == null)
            {
                instance = new I18n();
            }
            if (!pSelectedLang.Equals(AppLanguage.Empty))
            {
                lang = pSelectedLang;
            }
            instance.translation.TryGetValue(lang, out dictSelectedLang);
            dictSelectedLang?.TryGetValue(pID, out text);
            return text;
        }


        private I18n()
        {
            // Languages
            translation.Add(AppLanguage.FR, new Dictionary<AppTextID, String>());
            translation.Add(AppLanguage.EN, new Dictionary<AppTextID, String>());

            // Dictionaries
            translation.TryGetValue(AppLanguage.FR, out dictFR);
            translation.TryGetValue(AppLanguage.EN, out dictEN);

            //FR
            dictFR.Add(AppTextID.AUTHENTICATE, "Se connecter");
            dictFR.Add(AppTextID.SOS_EN_COURS, "en cours...");
            dictFR.Add(AppTextID.APPEL_DANS, "appel dans");
            dictFR.Add(AppTextID.ALERT_GPS_DISABLED, "Le GPS n'est pas activé. Impossible de récupérer la position");
            dictFR.Add(AppTextID.GPS_PERMISSION_KO, "Impossible de récupérer la position. Permission requise");
            dictFR.Add(AppTextID.WELCOME_GCC, "Bienvenue à GCC");
            dictFR.Add(AppTextID.PASSWORD, "Mot de passe");
            dictFR.Add(AppTextID.LOGIN, "Se connecter");
            dictFR.Add(AppTextID.RECIEVED, "Reçu");
            dictFR.Add(AppTextID.SEEN, "Vu");
            dictFR.Add(AppTextID.CONFIRM, "Confirmer");
            dictFR.Add(AppTextID.CLEANING, "Suppression...");
            dictFR.Add(AppTextID.LOGOUT, "Se déconnecter");
            dictFR.Add(AppTextID.RECORDED_AUDIO, "Audios enregistrés");
            dictFR.Add(AppTextID.FREE_SPACE, "Suppr. anciennes données");
            dictFR.Add(AppTextID.NEW, "Nouveau");
            dictFR.Add(AppTextID.ALERTS, "Alertes");
            dictFR.Add(AppTextID.DOCUMENTS, "Documents");
            dictFR.Add(AppTextID.EMPTY, "Vide");
            dictFR.Add(AppTextID.FILTER_EMPTY, "Les données sont filtrées, vérifiez le filtre.");
            dictFR.Add(AppTextID.WAITING_FOR_TRANSFER, "En attente d'envoi");
            dictFR.Add(AppTextID.WAITING_FOR_TRANSFER_SMS, "En attente d'envoi via SMS");
            dictFR.Add(AppTextID.MESSAGES, "Messages");
            dictFR.Add(AppTextID.LAST_CHECKIN, "Dernier Checkin ");
            dictFR.Add(AppTextID.GPS_TAP_INDICATION, "Tapez l'épingle pour envoyer la position");
            dictFR.Add(AppTextID.HOME, "Accueil");
            dictFR.Add(AppTextID.DOCS, "Docs");
            dictFR.Add(AppTextID.TEST, "#TEST");
            dictFR.Add(AppTextID.WARNINGS, "Alertes");
            dictFR.Add(AppTextID.TRACKING_ONGOING, "Suivi en cours...");
            dictFR.Add(AppTextID.TRACKING_STOPPED, "Suivi arrêté");
            dictFR.Add(AppTextID.AT, "à");
            dictFR.Add(AppTextID.DELETE, "Supprimer");
            dictFR.Add(AppTextID.AREYOUSURE, "Etes-vous sûr?");
            dictFR.Add(AppTextID.DELETE_DATA_OLDER_THAN, "Supprimer les données anciennes de ");
            dictFR.Add(AppTextID._2WEEKS, "2 Semaines");
            dictFR.Add(AppTextID._1MONTH, "1 Mois");
            dictFR.Add(AppTextID._3MONTHS, "3 Mois");
            dictFR.Add(AppTextID._6MONTHS, "6 Mois");
            dictFR.Add(AppTextID._1YEAR, "1 An");




            //EN
            dictEN.Add(AppTextID.AUTHENTICATE, "Log in");
            dictEN.Add(AppTextID.SOS_EN_COURS, "launching...");
            dictEN.Add(AppTextID.APPEL_DANS, "call in");
            dictEN.Add(AppTextID.ALERT_GPS_DISABLED, "Location cannot be requested. Please turn on GPS.");
            dictEN.Add(AppTextID.GPS_PERMISSION_KO, "Location cannot be requested. Permission required.");
            dictEN.Add(AppTextID.WELCOME_GCC, "Welcome to GCC");
            dictEN.Add(AppTextID.PASSWORD, "Password");
            dictEN.Add(AppTextID.LOGIN, "Log in");
            dictEN.Add(AppTextID.RECIEVED, "Recieved");
            dictEN.Add(AppTextID.SEEN, "Seen");
            dictEN.Add(AppTextID.CONFIRM, "Confirm");
            dictEN.Add(AppTextID.CLEANING, "Deleting...");
            dictEN.Add(AppTextID.LOGOUT, "Log out");
            dictEN.Add(AppTextID.RECORDED_AUDIO, "Recorded audios");
            dictEN.Add(AppTextID.FREE_SPACE, "Clear old data");
            dictEN.Add(AppTextID.NEW, "New");
            dictEN.Add(AppTextID.ALERTS, "Alerts");
            dictEN.Add(AppTextID.DOCUMENTS, "Documents");
            dictEN.Add(AppTextID.EMPTY, "Empty");
            dictEN.Add(AppTextID.FILTER_EMPTY, "Data are filtered. Check the Filter.");
            dictEN.Add(AppTextID.WAITING_FOR_TRANSFER, "Waiting for transfer");
            dictEN.Add(AppTextID.WAITING_FOR_TRANSFER_SMS, "Waiting for transfer via SMS");
            dictEN.Add(AppTextID.MESSAGES, "Messages");
            dictEN.Add(AppTextID.LAST_CHECKIN, "Last Checkin ");
            dictEN.Add(AppTextID.GPS_TAP_INDICATION, "Tap the map marker to send location");
            dictEN.Add(AppTextID.HOME, "Home");
            dictEN.Add(AppTextID.DOCS, "Docs");
            dictEN.Add(AppTextID.TEST, "#TEST");
            dictEN.Add(AppTextID.WARNINGS, "Warnings");
            dictEN.Add(AppTextID.TRACKING_ONGOING, "Tracking ongoing...");
            dictEN.Add(AppTextID.TRACKING_STOPPED, "Tracking stopped");
            dictEN.Add(AppTextID.AT, "at");
            dictEN.Add(AppTextID.DELETE, "Delete");
            dictEN.Add(AppTextID.AREYOUSURE, "Are you sure?");
            dictEN.Add(AppTextID.DELETE_DATA_OLDER_THAN, "Delete Data older than ");
            dictEN.Add(AppTextID._2WEEKS, "2 Weeks");
            dictEN.Add(AppTextID._1MONTH, "1 Month");
            dictEN.Add(AppTextID._3MONTHS, "3 Months");
            dictEN.Add(AppTextID._6MONTHS, "6 Months");
            dictEN.Add(AppTextID._1YEAR, "1 Year");
        }
    }

    public enum AppTextID
    {
        AUTHENTICATE,
        SOS_EN_COURS,
        APPEL_DANS,
        ALERT_GPS_DISABLED,
        GPS_PERMISSION_KO,
        WELCOME_GCC,
        PASSWORD,
        LOGIN,
        RECIEVED,
        SEEN,
        CONFIRM,
        CLEANING,
        LOGOUT,
        RECORDED_AUDIO,
        FREE_SPACE,
        ALERTS,
        DOCUMENTS,
        NEW,
        EMPTY,
        FILTER_EMPTY,
        WAITING_FOR_TRANSFER,
        WAITING_FOR_TRANSFER_SMS,
        MESSAGES,
        LAST_CHECKIN,
        GPS_TAP_INDICATION,
        HOME,
        DOCS,
        TEST,
        WARNINGS,
        TRACKING_ONGOING,
        TRACKING_STOPPED,
        AT,
        DELETE,
        AREYOUSURE,
        DELETE_DATA_OLDER_THAN,
        _2WEEKS,
        _1MONTH,
        _3MONTHS,
        _6MONTHS,
        _1YEAR
    }

    public enum AppLanguage
    {
        Empty,
        FR,
        EN
    }
}
