using log4net;

namespace AutoGenerateAPI
{ 
    public class LoggerManager
    {
        public static ILog GetLogger()
        {
            return LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }
    }
}
