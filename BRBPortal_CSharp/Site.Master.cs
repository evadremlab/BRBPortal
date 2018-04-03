using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using BRBPortal_CSharp.DAL;
using BRBPortal_CSharp.Models;

namespace BRBPortal_CSharp
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        /// <summary>
        /// Visible to child pages as "Master.User" via MasterType declaration on the child page.
        /// </summary>

        public BRBUser User
        {
            get
            {
                return (BRBUser)Session["User"];
            }
            set
            {
                Session["User"] = value;
            }
        }

        public DataProvider DataProvider { get; private set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };

                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }

            this.User = (BRBUser)Session["User"];
            this.DataProvider = new DataProvider();

            if (this.User == null)
            {
                UpdateSession(new BRBUser());
            }

            if (PageNeedsUserObject())
            {
                // must be authenticated and have either UserCode or BillingCode

                if (!Context.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login");
                }
                else if (this.User == null)
                {
                    Response.Redirect("~/Account/Login");
                }
                else if (string.IsNullOrEmpty(this.User.UserCode) && string.IsNullOrEmpty(this.User.BillingCode))
                {
                    Response.Redirect("~/Account/Login");
                }
            }

            // see if we have any messages to display
            // will look like: "Your password has been updated.|Update Password" ie: "msg|title"

            var infoMessage = Session["ShowAfterRedirect"] as String ?? "";

            if (!string.IsNullOrEmpty(infoMessage))
            {
                var parts = infoMessage.Split('|');
                var message = parts[0];
                var defaultTitle = "Status Message";
                var title = parts.Length > 1 ? parts[1] ?? defaultTitle : defaultTitle;

                ShowOKModal(message, title, 500);
            }

            Session.Remove("InfoMessage");
            Session.Remove("ShowAfterRedirect");
        }

        protected void Logoff(object sender, EventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Session.RemoveAll();

            Response.Redirect("~/Default");
        }

        public void UpdateSession(BRBUser user)
        {
            this.User = user;
        }

        public void ShowOKModal(string message, string title = "Status Message", int? delay = 0)
        {
            var jsFunction = string.Format("showOKModal('{0}', '{1}', {2});", message, title, delay);

            Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }

        public void ShowErrorModal(string message, string title = "System Error", int? delay = 0)
        {
            var jsFunction = string.Format("showErrorModal('{0}', '{1}', {2});", message, title, delay);

            Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }

        private bool PageNeedsUserObject()
        {
            return !Regex.IsMatch(Path.GetFileName(Request.Url.AbsolutePath), "Default|Login|Register|ManagePassword|ResetPassword|PaymentProcessed|PaymentCancelled|PaymentError", RegexOptions.IgnoreCase);
        }
    }
}
