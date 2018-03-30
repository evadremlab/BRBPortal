<%@ Page Title="Confirm Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmPayment.aspx.cs" Inherits="BRBPortal_CSharp.ConfirmPayment" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h4>is under construction</h4>
    <hr />
    <p>Submit button posts to the payment gateway.</p>
    <p>This page will be updated to show payment details.</p>

    <input type="hidden" name="lockAmount" value="true" />
    <input type="hidden" name="productId" value="24241003506464897434114212714156124" />
    <input type="hidden" name="returnUrl" value="http://rentportaldev.cityofberkeley.info/PaymentProcessed" />
    <input type="hidden" name="errorUrl" value="http://rentportaldev.cityofberkeley.info/PaymentError" />
    <input type="hidden" name="cancelUrl" value="http://rentportaldev.cityofberkeley.info/PaymentCancelled" />
    <input type="hidden" name="postbackUrl" value="http://clipper.transsight.com/api/Values" />
    <input type="hidden" name="cde-CartID-17" value="" />
    <input type="hidden" name="cde-BillingCode-1" value="" />
    <input type="hidden" name="paymentAmount" value="" />
    <button id="btnSubmit" runat="server" type="button" class="btn btn-primary">Submit</button>
    <script>
        $(document).ready(function () {
            $('#MainContent_btnSubmit').click(function (evt) { // hijack the asp.net form
                evt.preventDefault();

                $('#aspForm').prop('action', 'https://staging.officialpayments.com/pc_entry_cobrand.jsp');
                $('#aspForm').find('input[name="cde-CartID-17"]').val('<%: CartID %>');
                $('#aspForm').find('input[name="cde-BillingCode-1"]').val('<%: BillingCode %>');
                $('#aspForm').find('input[name="paymentAmount"]').val('<%: PaymentAmount %>');
                $('#aspForm').find('.aspNetHidden').remove(); // delete ASP.NET generated fields
                $('#aspForm').submit();
            });
        });
    </script>
</asp:Content>
