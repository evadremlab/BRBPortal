<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConfirmPayment.aspx.cs" Inherits="BRBPortal_CSharp.ConfirmPayment" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">



    <input type="hidden" name="lockAmount" value="true" />
    <input type="hidden" name="productId" value="24241003506464897434114212714156124" />
    <input type="hidden" name="returnUrl" value="http://rentportaldev.cityofberkeley.info/PaymentProcessed" />
    <input type="hidden" name="errorUrl" value="http://rentportaldev.cityofberkeley.info/PaymentError" />
    <input type="hidden" name="cancelUrl" value="http://rentportaldev.cityofberkeley.info/PaymentCancelled" />
    <input type="hidden" name="postbackUrl" value="http://clipper.transsight.com/api/Values" />
    <input type="hidden" name="cde-CartID-17" value="" />
    <input type="hidden" name="cde-BillingCode-1" value="" />
    <input type="hidden" name="paymentAmount" value="" />
    <button id="btnSubmit" runat="server" type="submit" class="btn btn-primary">Submit</button>
    <script>
        $(document).ready(function () {
            $('#btnSubmit').click(function (evt) { // hijack the asp.net form
                evt.preventDefault();

                $('#aspForm')
                    .prop('action', 'https://staging.officialpayments.com/pc_entry_cobrand.jsp')
                    .find('input[name="cde-CartID-17"]').val(<%: CartID %>)
                    .find('input[cde-BillingCode-1]').val('<%: BillingCode %>')
                    .find('input[paymentAmount]').val(<%: PaymentAmount %>)
                    .find('.aspNetHidden').remove() // delete ASP.NET generated fields
                    .submit();
            });
        });
    </script>
</asp:Content>
