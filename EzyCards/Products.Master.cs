using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using EzyCards.Models;
using EzyCards.Logic;

namespace EzyCards
{
	public partial class Products : System.Web.UI.MasterPage
	{
		private const string AntiXsrfTokenKey = "__AntiXsrfToken";
		private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
		private string _antiXsrfTokenValue;

		protected void Page_Init(object sender, EventArgs e)
		{
			// The code below helps to protect against XSRF attacks
			var requestCookie = Request.Cookies[AntiXsrfTokenKey];
			Guid requestCookieGuidValue;
			if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
			{
				// Use the Anti-XSRF token from the cookie
				_antiXsrfTokenValue = requestCookie.Value;
				Page.ViewStateUserKey = _antiXsrfTokenValue;
			}
			else
			{
				// Generate a new Anti-XSRF token and save to the cookie
				_antiXsrfTokenValue = Guid.NewGuid().ToString("N");
				Page.ViewStateUserKey = _antiXsrfTokenValue;

				var responseCookie = new HttpCookie(AntiXsrfTokenKey)
				{
					HttpOnly = true,
					Value = _antiXsrfTokenValue
				};
				if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
				{
					responseCookie.Secure = true;
				}
				Response.Cookies.Set(responseCookie);
			}

			Page.PreLoad += master_Page_PreLoad;
		}

		protected void master_Page_PreLoad(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				// Set Anti-XSRF token
				ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
				ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
			}
			else
			{
				// Validate the Anti-XSRF token
				if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
					|| (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
				{
					throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
				}
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HttpContext.Current.User.IsInRole("canEdit"))
			{
				adminLink.Visible = true;
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
			{
				string cartStr = string.Format("Cart ({0})", usersShoppingCart.GetCount());
				cartCount.InnerText = cartStr;
			}
		}

		public IQueryable<Category> GetCategories()
		{
			var _db = new EzyCards.Models.ProductContext();
			IQueryable<Category> query = _db.Categories;
			return query;
		}

		protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
		{
			Context.GetOwinContext().Authentication.SignOut();
		}

		// The return type can be changed to IEnumerable, however to support
		// paging and sorting, the following parameters must be added:
		//     int maximumRows
		//     int startRowIndex
		//     out int totalRowCount
		//     string sortByExpression
		public IQueryable<EzyCards.Models.FaceBookPage> GetFaceBookPages()
		{
			var _db = new EzyCards.Models.ProductContext();
			IQueryable<FaceBookPage> query = _db.FaceBookPages;
			return query;
		}

		// The return type can be changed to IEnumerable, however to support
		// paging and sorting, the following parameters must be added:
		//     int maximumRows
		//     int startRowIndex
		//     out int totalRowCount
		//     string sortByExpression

	}

}