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
        private static AppLanguage selectedLanguage;
        private static I18n instance;


        public static void SelectLang(AppLanguage pLang)
        {
            //selectedLanguage = pLang;
            selectedLanguage = AppLanguage.EN;
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
            dictFR.Add(AppTextID.WELCOME_GCC, "Bienvenue à GCC");
            dictFR.Add(AppTextID.PASSWORD, "Mot de passe");
            dictFR.Add(AppTextID.LOGIN, "Se connecter");
            dictFR.Add(AppTextID.RECIEVED, "Reçu");
            dictFR.Add(AppTextID.SEEN, "Vu");
            dictFR.Add(AppTextID.LOGOUT, "Se déconnecter");
            dictFR.Add(AppTextID.RECORDED_AUDIO, "Audios enregistrés");
            dictFR.Add(AppTextID.NEW, "Nouveau");
            dictFR.Add(AppTextID.ALERTS, "Alertes");
            dictFR.Add(AppTextID.EMPTY, "Vide");
            dictFR.Add(AppTextID.WAITING_FOR_TRANSFER, "En attente d'envoi");
            dictFR.Add(AppTextID.MESSAGES, "Messages");
            dictFR.Add(AppTextID.LAST_CHECKIN, "Dernier Chekin");
            dictFR.Add(AppTextID.GPS_TAP_INDICATION, "Tapez l'épingle pour envoyer la position.");
            dictFR.Add(AppTextID.HOME, "Accueil");
            dictFR.Add(AppTextID.DOCS, "Docs");
            dictFR.Add(AppTextID.WARNINGS, "Alertes");




            //EN
            dictEN.Add(AppTextID.AUTHENTICATE, "Log in");
            dictEN.Add(AppTextID.SOS_EN_COURS, "launching...");
            dictEN.Add(AppTextID.APPEL_DANS, "call in");
            dictEN.Add(AppTextID.ALERT_GPS_DISABLED, "Location cannot be requested. Please turn on GPS.");
            dictEN.Add(AppTextID.WELCOME_GCC, "Welcome to GCC");
            dictEN.Add(AppTextID.PASSWORD, "Password");
            dictEN.Add(AppTextID.LOGIN, "Log in");
            dictEN.Add(AppTextID.RECIEVED, "Recieved");
            dictEN.Add(AppTextID.SEEN, "Seen");
            dictEN.Add(AppTextID.LOGOUT, "Log out");
            dictEN.Add(AppTextID.RECORDED_AUDIO, "Recorded audios");
            dictEN.Add(AppTextID.NEW, "New");
            dictEN.Add(AppTextID.ALERTS, "Alerts");
            dictEN.Add(AppTextID.EMPTY, "Empty");
            dictEN.Add(AppTextID.WAITING_FOR_TRANSFER, "Waiting for transfer");
            dictEN.Add(AppTextID.MESSAGES, "Messages");
            dictEN.Add(AppTextID.LAST_CHECKIN, "Last Checkin");
            dictEN.Add(AppTextID.GPS_TAP_INDICATION, "Tap the map marker to send location");
            dictEN.Add(AppTextID.HOME, "Home");
            dictEN.Add(AppTextID.DOCS, "Docs");
            dictEN.Add(AppTextID.WARNINGS, "Warnings");
        }
    }

    public enum AppTextID
    {
        AUTHENTICATE,
        SOS_EN_COURS,
        APPEL_DANS,
        ALERT_GPS_DISABLED,
        WELCOME_GCC,
        PASSWORD,
        LOGIN,
        RECIEVED,
        SEEN,
        LOGOUT,
        RECORDED_AUDIO,
        ALERTS,
        NEW,
        EMPTY,
        WAITING_FOR_TRANSFER,
        MESSAGES,
        LAST_CHECKIN,
        GPS_TAP_INDICATION,
        HOME,
        DOCS,
        WARNINGS
    }

    public enum AppLanguage
    {
        Empty,
        FR,
        EN
    }
}
