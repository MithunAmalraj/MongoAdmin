using MongoAdmin.BO;
using MongoAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Web;

namespace MongoAdmin
{
    public class BasePage : System.Web.UI.Page
    {
        public AppConfigSettings _appSettings;
        public Cryoto crytoObj = new Cryoto();

        public BasePage()
        {
            //this.Load += new EventHandler(this.Page_Load);
            HttpCookie reqCookies = HttpContext.Current.Request.Cookies["MongoAdmin"];
            if (reqCookies != null)
            {
                string EncryptedCookieString = reqCookies["value"].ToString();
                string DecryptedCookieString = crytoObj.Decrypt(EncryptedCookieString);
                var jsonSettings = new JsonSerializerSettings();
                jsonSettings.DateFormatString = "dd-MM-yyyy hh:mm:ss";
                _appSettings = JsonConvert.DeserializeObject<AppConfigSettings>(DecryptedCookieString, jsonSettings);
            }
            if (_appSettings == null)
            {
                Server.Transfer("Login.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }
    }
}