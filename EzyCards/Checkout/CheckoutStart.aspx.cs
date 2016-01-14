using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EzyCards.Checkout
{
  public partial class CheckoutStart : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["payment_method"] == "paypal")
        {
            NVPAPICaller payPalCaller = new NVPAPICaller();
            string retMsg = "";
            string token = "";

            if (Session["payment_amt"] != null)
            {
                string amt = Session["payment_amt"].ToString();

                bool ret = payPalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
                if (ret)
                {
                    Session["token"] = token;
                    Response.Redirect(retMsg);
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
            else
            {
                Response.Redirect("CheckoutError.aspx?ErrorCode=AmtMissing");
            }
        }
        else if (Session["payment_method"] == "ezycard")
        {
            EzyCardFunctions ezyCardCaller = new EzyCardFunctions();
            string retMsg = "";
            string token = "";

            if (Session["payment_amt"] != null)
            {
                string amt = Session["payment_amt"].ToString();

                bool ret = ezyCardCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
                if (ret)
                {
                    Session["token"] = token;
                    Response.Redirect(retMsg);
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
            else
            {
                Response.Redirect("CheckoutError.aspx?ErrorCode=AmtMissing");
            }
        }
    }
  }
}