<%@ Page Title="Products" Language="C#" MasterPageFile="~/Products.Master" AutoEventWireup="true"
	CodeBehind="ProductList.aspx.cs" Inherits="WingtipToys.ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">

		function SetSessionStorage(payload) {
			var variable = "PageId";
			//var payload = document.getElementById("lbl").value;
			sessionStorage.setItem(variable, JSON.stringify(payload));
			//alert(variable);
		}

		function GetSessionStorage() {
			var variable = "PageId";
			var payload = sessionStorage.getItem(variable);
			
			if (payload == "undefined") {

				return null;

			}

			else {
				//alert(payload);
				document.getElementById('PageId').value = payload;
				//document.getElementById(result).innerHTML = payload;
				return payload;

			}

		}
		function GetHiddenValue() {
			var variable = "PageId";
			var payload = document.getElementById('PageId').value;
			//alert(payload);
		}
	</script>
	<section>

		<div>
			<button id="dsf" onclick="GetHiddenValue()" hidden="hidden">Get Page Id From Hiddden Value</button>

			<hgroup>
				<h2><%: Page.Title %></h2>
				</hgroup>
				<asp:HiddenField ID="PageId" runat="server" />
			
			<asp:ListView ID="productList" runat="server"
				DataKeyNames="ProductID" GroupItemCount="4"
				ItemType="WingtipToys.Models.Product" SelectMethod="GetProducts">
				<EmptyDataTemplate>
					<table>
						<tr>
							<td>No data was returned.</td>
						</tr>
					</table>
				</EmptyDataTemplate>
				<EmptyItemTemplate>
					<td />
				</EmptyItemTemplate>
				<GroupTemplate>
					<tr id="itemPlaceholderContainer" runat="server">
						<td id="itemPlaceholder" runat="server"></td>
					</tr>
				</GroupTemplate>
				<ItemTemplate>
					<td runat="server">
						<table>
							<tr>
								<td>
									<a href="<%#: GetRouteUrl("ProductByNameRoute", new {productName = Item.ProductName}) %>">
										<image src='/Catalog/Images/Thumbs/<%#:Item.ImagePath%>'
											width="120" height="100" border="1" />
									</a>
								</td>
							</tr>
							<tr>
								<td>
									<a href="<%#: GetRouteUrl("ProductByNameRoute", new {productName = Item.ProductName}) %>">
										<%#:Item.ProductName%>
									</a>
									<br />
									<span>
										<b>Price: </b><%#:String.Format("{0:c}", Item.UnitPrice)%>
									</span>
									<br />
									<a href="/AddToCart.aspx?productID=<%#:Item.ProductID %>">
										<span class="ProductListItem">
											<b>Add To Cart<b>
										</span>
									</a>
								</td>
							</tr>
							<tr>
								<td>&nbsp;</td>
							</tr>
						</table>
						</p>
					</td>
				</ItemTemplate>
				<LayoutTemplate>
					<table style="width: 100%;">
						<tbody>
							<tr>
								<td>
									<table id="groupPlaceholderContainer" runat="server" style="width: 100%">
										<tr id="groupPlaceholder"></tr>
									</table>
								</td>
							</tr>
							<tr>
								<td></td>
							</tr>
							<tr></tr>
						</tbody>
					</table>
				</LayoutTemplate>
			</asp:ListView>
		</div>
	</section>
</asp:Content>
