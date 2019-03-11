namespace SecurityTravelApp.Services
{
    public abstract class ServiceAbstract
    {
        // the abstract class should have a logger

        protected ServiceType type;

        //protected NLog.Logger Logger;


        protected ServiceAbstract(ServiceType pType)
        {
            type = pType;
            /* file configuration is possible through installation of nuget package Nlog.config */
            //var config = new NLog.Config.LoggingConfiguration();
            //var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
            //config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole);
            //NLog.LogManager.Configuration = config;
            //NLog.LogManager.ThrowExceptions = true;
            //Logger = NLog.LogManager.GetLogger(pType.ToString("g"));
        }

        public ServiceType Type
        {
            get
            {
                return type;
            }
        }
    }
}