<%@ Page Title="Update Security Questions" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="ManageSecurityQuestions.aspx.cs" Inherits="BRBPortal_CSharp.Account.ManageSecurityQuestions" %>
<%@ MasterType  virtualPath="~/Site.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <hr />
    <section id="updateSecurityQuestionsForm">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Quest1" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
                <div class="col-md-10" style="max-width:43rem;">
                    <asp:dropdownlist runat="server" ID="Quest1" ToolTip="Select a question from the list." CssClass="form-control selectpicker">
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer1" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer1" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Quest2" CssClass="col-md-2 control-label">* Security Question:</asp:Label>
                <div class="col-md-10" style="max-width:43rem;">
                    <asp:dropdownlist runat="server" ID="Quest2" ToolTip="Select a question from the list." CssClass="form-control selectpicker">
                        <asp:ListItem enabled="true" text="What was your favorite sport in high school?" value="What was your favorite sport in high school?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was your favorite food as a child?" value="What was your favorite food as a child?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What is your favorite movie?" value="What is your favorite movie?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="In what town was your first job?" value="In what town was your first job?"></asp:ListItem>
                        <asp:ListItem enabled="true" text="What was the make and model of your first car?" value="What was the make and model of your first car?"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Answer2" CssClass="col-md-2 control-label">* Security Answer:</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="Answer2" CssClass="form-control" TextMode="SingleLine" style="width:40rem; max-width:40rem;" data-lpignore="true" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:CheckBox ID="chkDeclare" runat="server" Text="Declaration: I hereby declare under penalty of perjury that .." CssClass="checkbox bold" />
                    <asp:CustomValidator runat="server" ID="CheckBoxRequired" ClientValidationFunction="CheckBoxRequired_ClientValidate" CssClass="text-danger hidden">Declaration must be checked.</asp:CustomValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="DeclareInits" CssClass="col-md-2 control-label">Declaration initials: </asp:Label>
                <div class="col-md-10">
                    <asp:TextBox runat="server" ID="DeclareInits" Width="70px" CssClass="form-control" ToolTip="Enter your initials acknowledging the Declaration above." />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="DeclareInits" CssClass="text-danger" Display="Dynamic" ErrorMessage="required" />
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:Button runat="server" ID="btnCancel" OnClick="Cancel_Click" Text="Cancel" CssClass="btn btn-sm btn-default" UseSubmitBehavior="false" CausesValidation="false" TabIndex="-1" />
                    <asp:Button runat="server" ID="btnSubmit" OnClick="Submit_Click" OnClientClick="return validate();" Text="Submit" CssClass="btn btn-primary" ToolTip="Click to confirm this information is correct." style="margin-left:1rem;" />
                </div>
            </div>
        </div>
    </section>

    <script>
        var valErrors = [];

        function addValError(msg) {
            valErrors.push('<li>' + msg + '</li>');
        }

        function validate() {
            try {
                var question1 = $('#<%:Quest1.ClientID%>').val();
                var question2 = $('#<%:Quest2.ClientID%>').val();

                var hasQuestion1 = question1.length;
                var hasAnswer1 = $('#<%:Answer1.ClientID%>').val().length;
                var hasQuestion2 = question2.length;
                var hasAnswer2 = $('#<%:Answer2.ClientID%>').val().length;

                if (hasQuestion1 && hasAnswer1 && hasQuestion2 && hasAnswer2) {
                    if (question1 === question2) {
                        addValError('Security Questions cannot be the same.');
                    }
                } else {
                    if (!hasQuestion1) {
                        addValError('Security Question 1 is required.');
                    }

                    if (!hasAnswer1) {
                        addValError('Security Answer 1 is required.');
                    }

                    if (!hasQuestion2) {
                        addValError('Security Question 2 is required.');
                    }

                    if (!hasAnswer2) {
                        addValError('Security Answer 2 is required.');
                    }
                }

                if (valErrors.length) {
                    showErrorModal(('<ul>' + valErrors.join('') + '</ul>'), "Validation Errors");
                    return false;
                } else {
                    return true;
                }
            } catch (ex) {
                showErrorModal(ex.message, "Validation Errors");
                return false;
            }
        }

        function _enableSubmitButton() {
            var isChecked = $('#<%:chkDeclare.ClientID%>').is(':checked');
            var hasInitials = $('#<%:DeclareInits.ClientID%>').val().length;

            $('#<%:btnSubmit.ClientID%>').attr('disabled', (isChecked && hasInitials) ? false : true);
        }

        $(document).ready(function () {
            $('#<%:btnSubmit.ClientID%>').attr('disabled', true); // initial state

            $('#<%:chkDeclare.ClientID%>').change(_enableSubmitButton);
            $('#<%:DeclareInits.ClientID%>').change(_enableSubmitButton);
        });
    </script>
</asp:Content>
