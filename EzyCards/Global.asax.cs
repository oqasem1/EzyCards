using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.Entity;
using EzyCards.Models;
using EzyCards.Logic;
using Microsoft.AspNet.Facebook;
using System.Web.Mvc;


namespace EzyCards
{
    public class Global : HttpApplication
    {
		void Application_Start(object sender, EventArgs e)
		{
			// Code that runs on application startup
			//RouteConfig.RegisterRoutes(RouteTable.Routes);
			//BundleConfig.RegisterBundles(BundleTable.Bundles);
			//FacebookConfig.Register(GlobalFacebookConfiguration.Configuration);
			FacebookConfig.Register(GlobalFacebookConfiguration.Configuration);
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);


			// Initialize the product database.
			// Database.SetInitializer(new ProductDatabaseInitializer());

			 //Database.SetInitializer<ProductContext>(
			 //										new DropCreateDatabaseAlways<ProductContext>());

			// Create the custom role and user.
			RoleActions roleActions = new RoleActions();
			roleActions.AddUserAndRole();

			// Add Routes.
			RegisterCustomRoutes(RouteTable.Routes);
		}

        void RegisterCustomRoutes(RouteCollection routes)
        {
          routes.MapPageRoute(
              "ProductsByCategoryRoute",
              "Category/{categoryName}",
              "~/ProductList.aspx"
          );
          routes.MapPageRoute(
              "ProductByNameRoute",
              "Product/{productName}",
              "~/ProductDetails.aspx"
          );
        }

        void Application_Error(object sender, EventArgs e)
        {
          // Code that runs when an unhandled error occurs.

          // Get last error from the server
          Exception exc = Server.GetLastError();

          if (exc is HttpUnhandledException)
          {
            if (exc.InnerException != null)
            {
              exc = new Exception(exc.InnerException.Message);
              Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax",
                  true);
            }
          }
        }
    }
}