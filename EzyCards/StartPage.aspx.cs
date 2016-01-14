using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facebook;
using Microsoft.AspNet.Facebook;
using Newtonsoft.Json.Linq;
using EzyCards.Models.FacebookStore;

namespace EzyCards
{
	
	public partial class AdminPage : System.Web.UI.Page
	{
		[FacebookAuthorize]
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				var signedRequest = Request.Form["signed_request"];
			if (signedRequest != null)
					Response.Write("Success");
				string url = GlobalFacebookConfiguration.Configuration.AppUrl;
				if (signedRequest != null)
				{
					ShowPageProduct showProduct = CheckPageIsAdmin(signedRequest);
					if (showProduct.InPage)
					{
						Session["PageId"] = showProduct.PageId;
						Response.Redirect("/ProductList?PageId=" + Session["PageId"]);

					}

				}
			}
		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			//FacebookContext facebookContext = new FacebookContext();
			string signedRequest = Request.Form["signed_request"];
			CheckPageIsAdmin(signedRequest);
		}

		
		public IDictionary<string, object> DecodePayload(string payload)
		{
			string base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=')
				.Replace('-', '+').Replace('_', '/');
			string json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
			return (IDictionary<string, object>)new JavaScriptSerializer().DeserializeObject(json);
		}
		[FacebookAuthorize]
		public ShowPageProduct CheckPageIsAdmin(string signedRequest)
		{
			ShowPageProduct showPageResult = new ShowPageProduct();

			


			//dynamic mesdf = client.Get("https://www.facebook.com/pages/Zad-Blady/139084059523528?sk=app_633881506758022");

			/******************/
			
			string test = string.Empty;
			string requestData = Request.Form["signed_request"];
			string[] splitPayload = signedRequest.Split('.');
			string sig = splitPayload[0];
			string payload1 = splitPayload[1];
			var decodedObj = DecodePayload(payload1);
			IDictionary<string, object> pageValues = null;
			if (decodedObj.ContainsKey("page"))
				pageValues = decodedObj["page"] as IDictionary<string, object>;
			if (pageValues == null)
			{
				showPageResult.InPage = false;
				return showPageResult;
			}
			string id = string.Empty;
			bool isAdmin = false;

			id = pageValues["id"].ToString();
			isAdmin = (bool)pageValues["admin"];

			showPageResult.InPage = true;
			showPageResult.IsAdmin = isAdmin;
			showPageResult.PageId = id;

			return showPageResult;
		}
	}
}