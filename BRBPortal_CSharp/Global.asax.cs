using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

using BRBPortal_CSharp.Shared;

namespace BRBPortal_CSharp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // handle the exceptions raised outside of the controller's actions
        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Exception ex = Server.GetLastError();

                if (ex is HttpException)
                {
                    HttpException httpEx = ex as HttpException;

                    int statusCode = httpEx.GetHttpCode();
                    switch (statusCode)
                    {
                        case 404:
                            break;

                        default:
                            Logger.LogException("Global.asax", ex);
                            break;
                    }
                }
                else
                {
                    Logger.LogException("Global.asax", ex);
                }
            }
            catch { }
        }
    }
}