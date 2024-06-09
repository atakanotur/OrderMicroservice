using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace Core.Utilities.Logger
{
    public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("AdoNetAppender");
        public static void LogAuditEvent(string message)
        {
            XmlConfigurator.Configure();
            if (log.IsInfoEnabled)
            {
                log.Info(message);
            }
        }

        public static void LogError(string message, Exception ex)
        {
            XmlConfigurator.Configure();
            if (log.IsErrorEnabled)
            {
                log.Error(message, ex);
            }
        }
    }
}
