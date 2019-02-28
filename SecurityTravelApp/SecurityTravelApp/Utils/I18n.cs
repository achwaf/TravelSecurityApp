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
            selectedLanguage = pLang;
        }

        public static String GetText(AppTextID pID)
        {
            String text = "";
            Dictionary<AppTextID, String> dictSelectedLang;
            if (instance == null)
            {
                instance = new I18n();
            }
            instance.translation.TryGetValue(selectedLanguage, out dictSelectedLang);
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


            //EN
            dictEN.Add(AppTextID.AUTHENTICATE, "Log in");
            dictEN.Add(AppTextID.SOS_EN_COURS, "launching...");
            dictEN.Add(AppTextID.APPEL_DANS, "call in");
        }
    }

    public enum AppTextID
    {
        AUTHENTICATE,
        SOS_EN_COURS,
        APPEL_DANS
    }

    public enum AppLanguage
    {
        FR,
        EN
    }
}
