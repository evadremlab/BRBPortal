using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.AspNet.Identity;

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

        public void ShowDialogOK(string message, string title = "Status")
        {
            var safeTitle = HttpContext.Current.Server.HtmlEncode(title);
            var safeMessage = HttpContext.Current.Server.HtmlEncode(message);
            var jsFunction = string.Format("showDialogOK('{0}', '{1}');", safeMessage, safeTitle);

            Page.ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }

        private bool PageNeedsUserObject()
        {
            return !Regex.IsMatch(Path.GetFileName(Request.Url.AbsolutePath), "Default|Login|Register|ManagePassword|ResetPassword|ProfileConfirm|PaymentProcessed|PaymentCancelled|PaymentError", RegexOptions.IgnoreCase);
        }
    }
}
