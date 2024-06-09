using log4net;
using log4net.Config;
using System;

namespace Logger4Net
{
    public class Logger
    {
        const string RollingFileAppender = @"RollingFileAppender";
        const string RollingDbAppender = @"RollingDbAppender";
        const string RollingRabbitMQAppender = @"RollingRabbitMQAppender";
        private static readonly ILog Log = null;
        private static readonly ILog DbLog = null;
        private static readonly ILog RabbitMQLog = null;

        static Logger()
        {
            if (Log == null)
            {
                Log = LogManager.GetLogger(RollingFileAppender);
                XmlConfigurator.Configure();
            }
            if (DbLog == null)
            {
                DbLog = LogManager.GetLogger(RollingDbAppender);
                XmlConfigurator.Configure();
            }
            if (RabbitMQLog == null)
            {
                RabbitMQLog = LogManager.GetLogger(RollingRabbitMQAppender);
                XmlConfigurator.Configure();
            }
        }

        public static void Write(string message)
        {
            Log.Info(message ?? string.Empty);
            Console.WriteLine(message ?? string.Empty);
        }

        public static void WriteAuditLog(OrderAudit orderAudit)
        {
            DbLog.Info(orderAudit);
            RabbitMQLog.Info(orderAudit);
            Console.WriteLine(orderAudit);
        }

        public static void Write(Exception exc)
        {
            Log.Fatal(exc);
            Console.WriteLine(string.Format("Exc: {0}", exc.Message));
        }

        public static void Write(Exception exc, string message)
        {
            Log.Error(message ?? string.Empty, exc);
            Console.WriteLine(string.Format("Msg: {0}, Exc: {1}.", message ?? string.Empty, exc.Message));
        }

        // ------------------------ Debug Methods Begin ------------------------

        public static void WriteDebug(string message)
        {
            Log.Debug(message);
            Console.WriteLine(message);
        }

        public static void WriteDebug(Exception exc)
        {
            Log.Debug(exc);
            Console.WriteLine(string.Format("Exc: {0}", exc.Message));
        }

        public static void WriteDebug(Exception exc, string message)
        {
            Log.Debug(message ?? string.Empty, exc);
            Console.WriteLine(string.Format("Msg: {0}, Exc: {1}.", message ?? string.Empty, exc.Message));
        }

        // ------------------------ Debug Methods End ------------------------
    }
}