using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BRBPortal_CSharp.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorMsg.Visible = false;
            FailureText.Text = "";

            if (!IsPostBack)
            {
                NameGrp.Visible = true;
                AgencyGrp.Visible = false;
                PropRelate.SelectedValue = "Owner";
            }
        }

        protected void RegisterUser_Click(object sender, EventArgs e)
        {
            string SoapStr;

            if (PropRelate.SelectedValue.ToString() == "Agent")
            {
                if (AgencyName.Text == "")
                {
                    ShowDialogOK("Agency name must be entered when property relationship is Agent.");
                    return;
                }
            }
            else if (PropRelate.SelectedValue.ToString() == "Owner")
            {
                if (FirstName.Text == "" || LastName.Text == "")
                {
                    ShowDialogOK("First and Last name must be entered when property relationship is Owner.");
                    return;
                }
            }

            //SoapStr = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:api=\"http://cityofberkeley.info/RTS/ClientPortal/API\">";
            //SoapStr += "<soapenv:Header/>";
            //SoapStr += "<soapenv:Body>";
            //SoapStr += "<api:validateRegistrationRequest>";
            //SoapStr += "<registrationRequestReq>";
            //SoapStr += "<profileDetails>";
            //SoapStr += "<userId>" + ReqUserID.Text + "</userId>";
            //SoapStr += "<billingCode>" + BillCode.Text + "</billingCode>";
            //SoapStr += "<name>";
            //SoapStr += "<first>" + ChgXMLChars(FirstName.Text) + "</first>";
            //SoapStr += "<!--Optional:--><middle>" + ChgXMLChars(MidName.Text) + "</middle>";
            //SoapStr += "<last>" + ChgXMLChars(LastName.Text) + "</last>";
            //if (Suffix.SelectedIndex < 1)
            //{
            //    SoapStr += "<suffix></suffix>";
            //}
            //else
            //{
            //    SoapStr += "<suffix>" + Suffix.Text + "</suffix>";
            //}

            //SoapStr += "<!--Optional:--><nameLastFirstDisplay></nameLastFirstDisplay>";
            //SoapStr += "<!--Optional:--><agencyName>" + ChgXMLChars(AgencyName.Text) + "</agencyName>";
            //SoapStr += "</name>";
            //SoapStr += "<mailingAddress>";
            //SoapStr += "<!--Optional:--><streetNumber>" + StNum.Text + "</streetNumber>";
            //SoapStr += "<!--Optional:--><streetName>" + ChgXMLChars(StName.Text) + "</streetName>";
            //SoapStr += "<!--Optional:--><unitNumber>" + StUnit.Text + "</unitNumber>";
            //SoapStr += "<!--Optional:--><fullAddress></fullAddress>";
            //SoapStr += "<!--Optional:--><city>" + StCity.Text + "</city>";
            //SoapStr += "<!--Optional:--><state>" + StState.Text + "</state>";
            //SoapStr += "<!--Optional:--><zip>" + StZip.Text + "</zip>";
            //SoapStr += "<!--Optional:--><country>" + StCountry.Text + "</country>";
            //SoapStr += "</mailingAddress>";
            //SoapStr += "<emailAddress>" + EmailAddress.Text + "</emailAddress>";
            //SoapStr += "<phone>" + PhoneNo.Text + "</phone>";
            //SoapStr += "<securityQuestion1>" + Quest1.Text + "</securityQuestion1>";
            //SoapStr += "<securityAnswer1>" + Answer1.Text + "</securityAnswer1>";
            //SoapStr += "<securityQuestion2>" + Quest2.Text + "</securityQuestion2>";
            //SoapStr += "<securityAnswer2>" + Answer2.Text + "</securityAnswer2>";
            //SoapStr += "</profileDetails>";
            //SoapStr += "<propertyDetails>";
            //SoapStr += "<relationship>" + PropRelate.SelectedValue.ToString + "</relationship>";
            //SoapStr += "<ownerLastName>" + PropOwnLastName.Text + "</ownerLastName>";
            //SoapStr += "<address>" + PropAddress.Text + "</address>";
            //SoapStr += "<purchaseYear>" + PurchaseYear.Text + "</purchaseYear>";
            //SoapStr += "</propertyDetails>";
            //SoapStr += "</registrationRequestReq>";
            //SoapStr += "</api:validateRegistrationRequest>";
            //SoapStr += "</soapenv:Body>";
            //SoapStr += "</soapenv:Envelope>";
            //if (Register_Soap(SoapStr) == false)
            //{
            //    if (iErrMsg.IndexOf("(500) Internal Server Error") > -1)
            //        iErrMsg = "(500) Internal Server Error.";
            //    ShowDialogOK("Error registering request." + iErrMsg, "Register User");
            //    return;
            //}

            //Response.Redirect("~/Account/Login.aspx", false);
        }

        protected void PropRelate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropRelate.SelectedValue == "Owner")
            {
                NameGrp.Visible = true;
                AgencyGrp.Visible = false;
                AgencyName.Text = "";
            }
            else
            {
                NameGrp.Visible = false;
                AgencyGrp.Visible = true;
                FirstName.Text = "";
                MidName.Text = "";
                LastName.Text = "";
                Suffix.Text = "";
                Suffix.SelectedIndex = 0;
            }
        }

        private void ShowDialogOK(string message, string title = "Status")
        {
            var jsFunction = string.Format("showOkModalOnPostback('{0}', '{1}');", message, title);

            ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:" + jsFunction, true);
        }
    }
}