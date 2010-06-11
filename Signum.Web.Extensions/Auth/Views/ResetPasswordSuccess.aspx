﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Signum.Web" %>
<%@ Import Namespace="Signum.Web.Extensions.Properties" %>
<%@ Import Namespace="Signum.Utilities" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div id="reset-password-container">    
        <h2><%= Resources.PasswordHasBeenChangedSuccessfully %></h2>
        <p><%= Resources.Please0IntoYourAccount.Formato(Html.ActionLink("login", "login", "auth"))%></p>    
    </div>
</asp:Content>