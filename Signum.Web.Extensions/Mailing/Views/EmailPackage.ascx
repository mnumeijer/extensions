<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="Signum.Web" %>
<%@ Import Namespace="Signum.Engine" %>
<%@ Import Namespace="Signum.Entities" %>
<%@ Import Namespace="Signum.Utilities" %>
<%@ Import Namespace="Signum.Entities.Mailing" %>

<%
using (var e = Html.TypeContext<EmailPackageDN>()) 
{
	Html.ValueLine(e, f => f.Name, f => f.ReadOnly = true);
	Html.EntityLine(e, f => f.Template, f => f.ReadOnly = true);
	Html.ValueLine(e, f => f.NumLines, f => f.ReadOnly = true);
	Html.ValueLine(e, f => f.NumErrors, f => f.ReadOnly = true);
%>
<fieldset>
    <legend>Lines</legend>
    <%
    Html.SearchControl(
      new FindOptions()
      {
          QueryName = typeof(EmailMessageDN),
          FilterOptions = { new FilterOption("Package", e.Value)},
          SearchOnLoad = true,
          FilterMode = FilterMode.Hidden,
          Create = false,
          View = true,
          Async = true
      }, e);%>
</fieldset>
<%
}
 %>