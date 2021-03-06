<%@ Page Title="Admin - Event List" Inherits="System.Web.Mvc.ViewPage<Site.Areas.Admin.Controllers.EventAdminController.ListViewModel>"  MasterPageFile="~/Areas/Admin/admin-no-form.master" Language="C#" %>
<%@ Import Namespace="Beweb"%>
<%@ Import Namespace="SavvyMVC.Helpers"%>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
  <%var dataList = Model; %>
  
  <%=Html.InfoMessage()%>
    
	<table class="databox" cellpadding="0" cellspacing="0">
		<%=dataList.TitleRow("Event List") %>
		<tr class="colheadfilters">
			<td colspan="99">
				<input type=button value="Create New" onclick="location='<%=Url.Action("Create") %>'" class="CreateNewButton" />
				<%dataList.Form(() => {%>
						<%--extra filter--%>
				<%}); %>
				<%=Html.SavvyHelpText(new HelpText("Event List")) %>
			</td>
		</tr>	
		<tr class="colhead">
			<td></td>
			<td><%=dataList.ColSort("Title","Title")%></td>
			<td><%=dataList.ColSort("Location","Location")%></td>
			<td><%=dataList.ColSort("StartDate","Start Date")%></td>
			<td><%=dataList.ColHead("Status")%></td>
			<td><%=dataList.ColSort("LinkURL","Link Url")%></td>
			<td><%=dataList.ColSort("IsPublished","Is Published")%></td>
		</tr>
		<%foreach (var listItem in dataList.GetResults()) {%> 
			<tr class="<%=dataList.RowClass(listItem) %>">
				<td><%=Html.ActionLink("Edit", "Edit", new { id=listItem.ID }) %></td>
				
				<td><%=Forms.EditableTextField(listItem,listItem.Fields.Title)%></td>
				<td><%=listItem.Location.HtmlEncode() %></td>
				<td><%=Fmt.DateTime(listItem.StartDate) %></td>
				<td><%=Fmt.PublishStatusHtml(listItem.StartDate,listItem.EndDate) %></td>
				<td><%=Fmt.WebAddress(listItem.LinkURL.HtmlEncode()) %></td>
				<td><%=Fmt.YesNo(listItem.IsPublished) %></td>
			</tr>
		<%} %>
		<%=dataList.ItemCountRow()%>
	</table>


</asp:Content>

