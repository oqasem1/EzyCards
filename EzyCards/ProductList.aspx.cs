using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EzyCards.Models;
using System.Web.ModelBinding;
using System.Web.Routing;
using System.Web.UI.HtmlControls;

namespace EzyCards
{
  public partial class ProductList : System.Web.UI.Page
  {
	 
	  protected void Page_Load(object sender, EventArgs e)
	  {
		  if (!Page.IsPostBack)
		  {
			  string url = HttpContext.Current.Request.Url.LocalPath;
			  if (!String.IsNullOrEmpty(Request.QueryString["PageId"]))
			  {
				  PageId.Value = Request.QueryString["PageId"];
				  Session["PageId"] = Request.QueryString["PageId"];
				  string message = "sessionStorage.setItem('PageId', JSON.stringify('" + PageId.Value + "'))";
				  ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
				  ScriptManager.RegisterStartupScript((sender as Control), typeof(Page), "script", "GetSessionStorage()", true);

				  if (!String.IsNullOrEmpty(PageId.Value))
				  {
					  Session["PageId"] = PageId.Value;
				  }

			  }
			  else
			  {
				
				  ScriptManager.RegisterStartupScript((sender as Control), typeof(Page), "script", "GetSessionStorage()", true);
				  if (!String.IsNullOrEmpty(PageId.Value))
				  {
					  Session["PageId"] = PageId.Value;
				  }
			  }
			 
		  }
		  else
		  {
			  ScriptManager.RegisterStartupScript((sender as Control), typeof(Page), "script", "GetSessionStorage()", true);
			  if (!String.IsNullOrEmpty(PageId.Value))
			  {
				  Session["PageId"] = PageId.Value;
			  }
		  }
		 

		  
		 
	  }

    public IQueryable<Product> GetProducts(
                        [QueryString("id")] int? categoryId,
                        [RouteData] string categoryName)
    {
      var _db = new EzyCards.Models.ProductContext();
      IQueryable<Product> query = _db.Products;
		IQueryable<FaceBookPage> facePage = _db.FaceBookPages;

		string faceBookPageId = Session["PageId"] == null ? null : Session["PageId"].ToString();

		//if (!String.IsNullOrEmpty(faceBookPageId))
		{
			query = query.Where(p =>
							String.Compare(p.faceBookPage.FaceBookPageId,
							faceBookPageId) == 0);
		}

      if (categoryId.HasValue && categoryId > 0)
      {
        query = query.Where(p => p.CategoryID == categoryId);
      }

      if (!String.IsNullOrEmpty(categoryName))
      {
        query = query.Where(p =>
                            String.Compare(p.Category.CategoryName,
							categoryName) == 0 && String.Compare(p.faceBookPage.FaceBookPageId,
							faceBookPageId) == 0);
      }
      return query;
    }
  }
}