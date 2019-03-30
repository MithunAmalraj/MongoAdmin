using MongoAdmin.BO;
using MongoAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MongoAdmin
{
    public partial class Login : System.Web.UI.Page
    {
        private AuthenticateBO _authenticateBO = new AuthenticateBO();
        private string MongoURL = System.Configuration.ConfigurationManager.ConnectionStrings["MongoServerAccountDBURL"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkbtnApplicationLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["UserName"] = null;
                Session["AvailableServers"] = null;
                Session["DefaultServer"] = null;

                UserLogin userLoginObj = new UserLogin();

                if (txtApplicationUserName.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Admin User Id cannot be empty";
                    return;
                }

                if (txtApplicationUserName.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Admin Password cannot be empty";
                    return;
                }

                userLoginObj = _authenticateBO.Login(txtApplicationUserName.Text, txtApplicationUserName.Text, MongoURL);
                if (userLoginObj != null)
                {
                    var item = userLoginObj.AvailableServers.Where(m => m.IsDefaultDB).FirstOrDefault();
                    Cryoto crytoObj = new Cryoto();
                    AppConfigSettings _appSettings = new AppConfigSettings();
                    _appSettings.UserName = userLoginObj.UserName;
                    _appSettings.MongoServer = item.MongoServer;
                    _appSettings.MongoPort = item.MongoPort;
                    _appSettings.MongoDBName = item.MongoDBName;
                    _appSettings.MongoUserName = item.MongoUserName;
                    _appSettings.MongoPassword = item.MongoPassword;
                    _appSettings.CreateAccess = item.CreateAccess;
                    _appSettings.ReadAccess = item.ReadAccess;
                    _appSettings.UpdateAccess = item.UpdateAccess;
                    _appSettings.DeleteAccess = item.DeleteAccess;
                    _appSettings.ExportAccess = item.ExportAccess;
                    _appSettings.MongoServerPath = WebConfigurationManager.AppSettings["MongoServerPath"];
                    _appSettings.ExportPath = WebConfigurationManager.AppSettings["ExportPath"];
                    _appSettings.CreatedDate = DateTime.Now;
                    Session["LoginType"] = "A";
                    Session["UserName"] = txtApplicationUserName.Text;
                    Session["AvailableServers"] = userLoginObj.AvailableServers;
                    Session["DefaultServer"] = item.MongoUserId;
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
                else
                {
                    lblErrorMessage.Text = "Authentication Failed";
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected void lnkbtnMongoLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Session["UserName"] = null;
                Session["AvailableServers"] = null;
                Session["DefaultServer"] = null;

                UserLogin userLoginObj = new UserLogin();

                if (txtMongoServer.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Mongo Server cannot be empty";
                    return;
                }

                if (txtMongoPort.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Mongo Port cannot be empty";
                    return;
                }

                if (txtMongoDBName.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Mongo DB name cannot be empty";
                    return;
                }

                //Check Connection and get List Of Databases
                //Construct Connection String with Login
                string MongoURL = "";
                if (txtMongoUserName.Text == "")
                    MongoURL = "mongodb://" + txtMongoServer.Text + ":" + txtMongoPort.Text + "/" + txtMongoDBName.Text + "";
                else
                    MongoURL = "mongodb://" + txtMongoUserName.Text + ":" + txtMongoPassword.Text + "@" + txtMongoServer.Text + ":" + txtMongoPort.Text + "/" + txtMongoDBName.Text + "";
                if (txtMongoDBName.Text == "admin")
                {

                }
                DatabaseStats databaseStats = _authenticateBO.GetDatabaseStats(MongoURL, txtMongoDBName.Text);
                if (databaseStats != null && databaseStats.db != null)
                {
                    Cryoto crytoObj = new Cryoto();
                    AppConfigSettings _appSettings = new AppConfigSettings();
                    _appSettings.UserName = userLoginObj.UserName;
                    _appSettings.MongoServer = txtMongoServer.Text;
                    _appSettings.MongoPort = txtMongoPort.Text;
                    _appSettings.MongoDBName = txtMongoDBName.Text;
                    _appSettings.MongoUserName = txtMongoUserName.Text;
                    _appSettings.MongoPassword = txtMongoPassword.Text;
                    _appSettings.CreateAccess = true;
                    _appSettings.ReadAccess = true;
                    _appSettings.UpdateAccess = true;
                    _appSettings.DeleteAccess = true;
                    _appSettings.ExportAccess = true;
                    _appSettings.MongoServerPath = WebConfigurationManager.AppSettings["MongoServerPath"];
                    _appSettings.ExportPath = WebConfigurationManager.AppSettings["ExportPath"];
                    _appSettings.CreatedDate = DateTime.Now;
                    Session["LoginType"] = "M";
                    Session["UserName"] = txtMongoUserName.Text;
                    List<UserLoginMongoServer> serverList = new List<UserLoginMongoServer>();
                    if (txtMongoDBName.Text == "admin")
                    {
                        List<Database> databaseList = _authenticateBO.GetAllDatabases(MongoURL);
                        if (databaseList != null && databaseList.Count > 0)
                        {
                            foreach (var item in databaseList)
                            {
                                UserLoginMongoServer serverObj = new UserLoginMongoServer();
                                serverObj.MongoServer = txtMongoServer.Text;
                                serverObj.MongoPort = txtMongoPort.Text;
                                serverObj.MongoDBName = item.DatabaseName;
                                serverList.Add(serverObj);
                            }
                        }
                    }
                    else
                    {
                        UserLoginMongoServer serverObj = new UserLoginMongoServer();
                        serverObj.MongoServer = txtMongoServer.Text;
                        serverObj.MongoPort = txtMongoPort.Text;
                        serverObj.MongoDBName = txtMongoDBName.Text;
                        serverList.Add(serverObj);
                    }

                    Session["AvailableServers"] = serverList;
                    Session["DefaultServer"] = null;
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
                else
                {
                    lblErrorMessage.Text = "Authentication Failed";
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}