using MongoAdmin.BO;
using MongoAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MongoAdmin
{
    public partial class SiteMaster : MasterPage
    {
        public Cryoto crytoObj = new Cryoto();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    lblErrorMessage.Text = "";
                    lblSuccessMessage.Text = "";
                    if (Session["LoginType"].ToString() == "A")
                    {
                        //Load list of available servers in drop down
                        if (Session["AvailableServers"] != null)
                        {
                            int i = 0;
                            var availableServers = (List<UserLoginMongoServer>)Session["AvailableServers"];
                            foreach (var item in availableServers)
                            {
                                ddlMongoServer.Items.Insert(i++, new ListItem(item.MongoServer + ":" + item.MongoPort + "/" + item.MongoDBName, item.MongoUserId.ToString()));
                            }
                            if (Session["DefaultServer"] != null)
                                ddlMongoServer.SelectedValue = Session["DefaultServer"].ToString();
                        }
                        if (Session["UserName"] != null)
                        {
                            ltrlUserName.Text = "Welcome " + Convert.ToString(Session["UserName"]) + "!";
                        }
                    }
                    else if (Session["LoginType"].ToString() == "M")
                    {
                        //Load list of available servers in drop down
                        if (Session["AvailableServers"] != null)
                        {
                            int i = 0;
                            var availableServers = (List<UserLoginMongoServer>)Session["AvailableServers"];
                            foreach (var item in availableServers)
                            {
                                ddlMongoServer.Items.Insert(i++, new ListItem(item.MongoServer + ":" + item.MongoPort + "/" + item.MongoDBName, item.MongoUserId.ToString()));
                            }
                            if (Session["DefaultServer"] != null)
                                ddlMongoServer.SelectedValue = Session["DefaultServer"].ToString();
                        }
                        if (Session["UserName"] != null)
                        {
                            ltrlUserName.Text = "Welcome " + Convert.ToString(Session["UserName"]) + "!";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string ErrorMessage
        {
            get
            {
                return lblErrorMessage.Text;
            }
            set
            {
                lblErrorMessage.Text = value;
            }
        }

        public string SuccessMessage
        {
            get
            {
                return lblSuccessMessage.Text;
            }
            set
            {
                lblSuccessMessage.Text = value;
            }
        }

        protected void ddlMongoServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var availableServers = (List<UserLoginMongoServer>)Session["AvailableServers"];
                var selectedServer = availableServers.Where(m => m.MongoUserId == Convert.ToInt32(ddlMongoServer.SelectedValue)).FirstOrDefault();

                if (selectedServer != null)
                {
                    AppConfigSettings _appSettings = new AppConfigSettings();
                    HttpCookie reqCookies = HttpContext.Current.Request.Cookies["MongoAdmin"];
                    if (reqCookies != null)
                    {
                        string EncryptedCookieString = reqCookies["value"].ToString();
                        string DecryptedCookieString = crytoObj.Decrypt(EncryptedCookieString);
                        var jsonDesSettings = new JsonSerializerSettings();
                        jsonDesSettings.DateFormatString = "dd-MM-yyyy hh:mm:ss";
                        _appSettings = JsonConvert.DeserializeObject<AppConfigSettings>(DecryptedCookieString, jsonDesSettings);
                    }
                    _appSettings.MongoServer = selectedServer.MongoServer;
                    _appSettings.MongoPort = selectedServer.MongoPort;
                    _appSettings.MongoDBName = selectedServer.MongoDBName;
                    _appSettings.MongoUserName = selectedServer.MongoUserName;
                    _appSettings.MongoPassword = selectedServer.MongoPassword;
                    Session["DefaultServer"] = selectedServer.MongoUserId;
                    var jsonSettings = new JsonSerializerSettings();
                    jsonSettings.DateFormatString = "dd-MM-yyyy hh:mm:ss";
                    string SID = JsonConvert.SerializeObject(_appSettings, jsonSettings);
                    string EncryptSID = crytoObj.Encrypt(SID);

                    HttpCookie searchInfo = new HttpCookie("MongoAdmin");
                    searchInfo["value"] = EncryptSID;
                    searchInfo.Expires.Add(new TimeSpan(0, 20, 0));
                    Response.Cookies.Add(searchInfo);

                    Response.Redirect("Dashboard.aspx");
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            try
            {
                Session["UserName"] = null;
                Session["AvailableServers"] = null;
                Session["DefaultServer"] = null;
                AppConfigSettings _appSettings = new AppConfigSettings();
                HttpCookie reqCookies = HttpContext.Current.Request.Cookies["MongoAdmin"];
                if (reqCookies != null)
                {
                    reqCookies.Expires = DateTime.Now.AddDays(-1d);
                    //Update the Cookie in Browser.
                    Response.Cookies.Add(reqCookies);
                }
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {

            }
        }
    }
}