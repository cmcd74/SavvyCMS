<%@ Page Title="Edit News Rss" Inherits="System.Web.Mvc.ViewPage<Models.NewsRSS>" Language="C#" MasterPageFile="~/Areas/Admin/admin-no-form.master" %>
<%@ Import Namespace="Beweb" %>

<asp:Content ID="head" runat="server" ContentPlaceHolderID="HeadContent">
	<script type="text/javascript">
		$(document).ready(function() {
			BewebInitForm("form")
		});
	</script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BodyContent" Runat="Server">
	<%var record = Model; %>

	<%=Html.InfoMessage()%>
	<%=Html.ValidationSummary("This record could not be saved.") %>
	
	<form name="form" id="form" method="post" enctype="multipart/form-data">
		<table class="svyEdit" cellspacing="0">
			<tr>
				<th colspan="2">News Rss</th>
			</tr>
			<tr>
				<td colspan="2" class="header">
					<!--this replaced by .footer inner html-->
				</td>
			</tr>
			<tr>
				<td class="label">Feed Name:</td>
				<td class="field"><%=new Forms.TextField(record.Fields.FeedName, true) %></td>
			</tr>
			<tr>
				<td class="label">Feed Url:</td>
				<td class="field"><%= new Forms.UrlField(record.Fields.FeedURL, true) %></td>
			</tr>
			<tr>
				<td class="label">Last Updated:</td>
				<td class="field"><%//= new Forms.DateField(record.Fields.LastUpdated, true) %><%=record.LastUpdated.FmtDateTime() %></td>
			</tr>
			<tr>
				<td class="label">Date Added:</td>
				<td class="field"><%= new Forms.DisplayField(record.Fields.DateAdded)%></td>
			</tr>
			<tr>
				<td class="label">Is Published:</td>
				<td class="field"><%=new Forms.YesNoField(record.Fields.IsPublished, true) %></td>
			</tr>
			<tr>
				<td colspan="2" class="footer">
					<div class="std-footer-buttons">
						<%=Html.SaveButton() %>
						<%=Html.SaveAndRefreshButton() %>
						<%//=Html.DuplicateCopyButton() %>
						<%=Html.CancelButton() %>
						<%=Html.DeleteButton(record) %>
					</div>
					<div class="extra-footer-buttons">
						<%=Html.SavvyHelpText(new Beweb.HelpText("News RSS Edit")) %>
						<%--<%=Html.PreviewLink(record, "View this page")%> |--%>
					</div>
				</td>
			</tr>
		</table>
		<%=Html.AntiForgeryToken() %>
		<%=Html.ReturnPageToken() %>
	</form>
</asp:Content>

