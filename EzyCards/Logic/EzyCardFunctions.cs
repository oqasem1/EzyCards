//
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using EzyCards;
using EzyCards.Models;
using System.Collections.Generic;
using System.Linq;

public class EzyCardFunctions
{
    //Flag that determines the PayPal environment (live or sandbox)
    private const bool bSandbox = true;
    private const string CVV2 = "CVV2";

    // Live strings.
    //private string pEndPointURL = "https://localhost:44304/NVP/APICheckNVP.aspx";
	private string pEndPointURL = "https://" + ConfigurationManager.AppSettings["gatewayWebSite"] + "/NVP/APICheckNVP.aspx";
    private string host = "www.paypal.com";

    // Sandbox strings.
    //private string pEndPointURL_SB = "https://localhost:44304/NVP/APICheckNVP.aspx";
	private string pEndPointURL_SB = "https://" + ConfigurationManager.AppSettings["gatewayWebSite"] + "/NVP/APICheckNVP.aspx";
    //private string host_SB = "localhost:44304/";
    private string host_SB = ConfigurationManager.AppSettings["gatewayWebSite"]+"/";
	private string EzyGateFaceBookWebsite = ConfigurationManager.AppSettings["EzyGateFaceBookWebsite"];
    private const string SIGNATURE = "SIGNATURE";
    private const string PWD = "PWD";
    private const string ACCT = "ACCT";

    //Replace <Your API Username> with your API Username
    //Replace <Your API Password> with your API Password
    //Replace <Your Signature> with your Signature
    //public string APIUsername = "oqaasem.test-facilitator_api1.gmail.com";//"or_qasim-facilitator_api1.yahoo.com";
    //private string APIPassword = "RH69TH49HKX9LHN4";//"425ES6UJL2FM3VDP";
    //private string APISignature = "AjLfMYKAWa60PDSAru9Y6TWQUj8RAkKmB1UTcwhNSbCwDeO0zJo.FeeU";//"AFcWxV21C7fd0v3bYYYRCpSSRl31AF1CU8pGNaeah-.dVLllrIrVusJR";

    // We use inside it (when log in PayPal) or_qasim@yahoo.com (100 USD)OR oqasem.pal@gmail.com (99999 USD)
    public string APIUsername = "or_qasim-facilitator_api1.yahoo.com";
    private string APIPassword = "425ES6UJL2FM3VDP";
    private string APISignature = "AFcWxV21C7fd0v3bYYYRCpSSRl31AF1CU8pGNaeah-.dVLllrIrVusJR";


    private string Subject = "";
    private string BNCode = "PP-ECWizard";


    //HttpWebRequest Timeout specified in milliseconds 
    private const int Timeout = 15000;
    private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

    public void SetCredentials(string Userid, string Pwd, string Signature)
    {
        APIUsername = Userid;
        APIPassword = Pwd;
        APISignature = Signature;
    }

    public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg)
    {
        if (bSandbox)
        {
            pEndPointURL = pEndPointURL_SB;
            host = host_SB;
        }

		string returnURL = "https://" + EzyGateFaceBookWebsite + "/Checkout/CheckoutReview.aspx";
		string cancelURL = "https://" + EzyGateFaceBookWebsite + "/Checkout/CheckoutCancel.aspx";

        NVPEzyCodec encoder = new NVPEzyCodec();
        encoder["METHOD"] = "SetExpressCheckout";
        encoder["RETURNURL"] = returnURL;
        encoder["CANCELURL"] = cancelURL;
		encoder["BRANDNAME"] = "EzyGate Facebook Store";
        encoder["PAYMENTREQUEST_0_AMT"] = amt;
        encoder["PAYMENTREQUEST_0_ITEMAMT"] = amt;
        encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";
        
        // Get the Shopping Cart Products
        using (EzyCards.Logic.ShoppingCartActions myCartOrders = new EzyCards.Logic.ShoppingCartActions())
        {
            List<CartItem> myOrderList = myCartOrders.GetCartItems();
            encoder["PAYMENTREQUEST_0_QTYCOUNT"] = myOrderList.Count.ToString();
            for (int i = 0; i < myOrderList.Count; i++)
            {
                encoder["L_PAYMENTREQUEST_0_NAME" + i] = myOrderList[i].Product.ProductName.ToString();
                encoder["L_PAYMENTREQUEST_0_AMT" + i] = myOrderList[i].Product.UnitPrice.ToString();
                encoder["L_PAYMENTREQUEST_0_QTY" + i] = myOrderList[i].Quantity.ToString();
            }
        }

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        NVPEzyCodec decoder = new NVPEzyCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            token = decoder["TOKEN"];
            // Don't forget to make the http to https (SSL)
            string ECURL = "https://" + host + "/NVP/APIGetToken.aspx?cmd=_express-checkout" + "&token=" + token;
            retMsg = ECURL;
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];
            return false;
        }
    }

    public bool GetCheckoutDetails(string token, ref string PayerID, ref NVPEzyCodec decoder, ref string retMsg)
    {
        if (bSandbox)
        {
            pEndPointURL = pEndPointURL_SB;
        }

        NVPEzyCodec encoder = new NVPEzyCodec();
        encoder["METHOD"] = "GetExpressCheckoutDetails";
        encoder["TOKEN"] = token;

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        decoder = new NVPEzyCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            PayerID = decoder["PAYERID"];
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    public bool DoCheckoutPayment(string finalPaymentAmount, string token, string PayerID, ref NVPEzyCodec decoder, ref string retMsg)
    {
        if (bSandbox)
        {
            pEndPointURL = pEndPointURL_SB;
        }

        NVPEzyCodec encoder = new NVPEzyCodec();
        encoder["METHOD"] = "DoExpressCheckoutPayment";
        encoder["TOKEN"] = token;
        encoder["PAYERID"] = PayerID;
        encoder["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount;
        encoder["PAYMENTREQUEST_0_CURRENCYCODE"] = "USD";
        encoder["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale";

        string pStrrequestforNvp = encoder.Encode();
        string pStresponsenvp = HttpCall(pStrrequestforNvp);

        decoder = new NVPEzyCodec();
        decoder.Decode(pStresponsenvp);

        string strAck = decoder["ACK"].ToLower();
        if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
        {
            return true;
        }
        else
        {
            retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                "Desc2=" + decoder["L_LONGMESSAGE0"];

            return false;
        }
    }

    public string HttpCall(string NvpRequest)
    {
        string url = pEndPointURL;

        string strPost = NvpRequest + "&" + buildCredentialsNVPString();
        strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode(BNCode);

        HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
        //objRequest.Timeout = Timeout;
        objRequest.Method = "POST";
        objRequest.ContentLength = strPost.Length;
        // This is important in aspx
        objRequest.ContentType = "application/x-www-form-urlencoded";

        try
        {
            using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
            {
                myWriter.Write(strPost);
            }
        }
        catch (Exception e)
        {
            // Log the exception.
            EzyCards.Logic.ExceptionUtility.LogException(e, "HttpCall in PayPalFunction.cs");
        }

        //Retrieve the Response returned from the NVP API call to PayPal.
        HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
        string result;
        using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
        {
            result = sr.ReadToEnd();
        }

        return result;
    }

    private string buildCredentialsNVPString()
    {
        NVPEzyCodec codec = new NVPEzyCodec();

        if (!IsEmpty(APIUsername))
            codec["USER"] = APIUsername;

        if (!IsEmpty(APIPassword))
            codec[PWD] = APIPassword;

        if (!IsEmpty(APISignature))
            codec[SIGNATURE] = APISignature;

        if (!IsEmpty(Subject))
            codec["SUBJECT"] = Subject;

        codec["VERSION"] = "88.0";

        return codec.Encode();
    }

    public static bool IsEmpty(string s)
    {
        return s == null || s.Trim() == string.Empty;
    }
}

public sealed class NVPEzyCodec : NameValueCollection
{
    private const string AMPERSAND = "&";
    private const string EQUALS = "=";
    private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
    private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

    public string Encode()
    {
        StringBuilder sb = new StringBuilder();
        bool firstPair = true;
        foreach (string kv in AllKeys)
        {
            string name = HttpUtility.UrlEncode(kv);
            string value = HttpUtility.UrlEncode(this[kv]);
            if (!firstPair)
            {
                sb.Append(AMPERSAND);
            }
            sb.Append(name).Append(EQUALS).Append(value);
            firstPair = false;
        }
        return sb.ToString();
    }

    public void Decode(string nvpstring)
    {
        Clear();
        foreach (string nvp in nvpstring.Split(AMPERSAND_CHAR_ARRAY))
        {
            string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
            if (tokens.Length >= 2)
            {
                string name = HttpUtility.UrlDecode(tokens[0]);
                string value = HttpUtility.UrlDecode(tokens[1]);
                Add(name, value);
            }
        }
    }

    public void Add(string name, string value, int index)
    {
        this.Add(GetArrayName(index, name), value);
    }

    public void Remove(string arrayName, int index)
    {
        this.Remove(GetArrayName(index, arrayName));
    }

    public string this[string name, int index]
    {
        get
        {
            return this[GetArrayName(index, name)];
        }
        set
        {
            this[GetArrayName(index, name)] = value;
        }
    }

    private static string GetArrayName(int index, string name)
    {
        if (index < 0)
        {
            throw new ArgumentOutOfRangeException("index", "index cannot be negative : " + index);
        }
        return name + index;
    }
}
