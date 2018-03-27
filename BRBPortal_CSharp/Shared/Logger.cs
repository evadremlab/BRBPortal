using System;

namespace BRBPortal_CSharp.Shared
{
    public static class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Log(string source, string msg)
        {
            log.InfoFormat("{0} : {1}", source, msg);
        }

        public static void LogException(string source, Exception ex)
        {
            log.ErrorFormat("EXCEPTION in {0} : {1}", source, ex);

            log.ErrorFormat("STACK TRACE in {0} : {1}", source, ex.StackTrace);

            if (ex.InnerException != null)
            {
                log.ErrorFormat("INNER EXCEPTION in {0} : {1}", source, ex.InnerException);
            }
        }
    }
}