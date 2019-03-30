using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MongoAdmin
{
    public partial class AuthSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            }
            catch(Exception ex)
            {

            }
        }

        protected void btnSetupAuthDB_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAdminUserId.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Admin User Id cannot be empty";
                    return;
                }

                if (txtAdminPassword.Text.Trim() == "")
                {
                    lblErrorMessage.Text = "Admin Password cannot be empty";
                    return;
                }

                string HostIP = WebConfigurationManager.AppSettings["MongoHost"];
                string PortNumber = WebConfigurationManager.AppSettings["MongoPort"];

                if(HostIP != null && PortNumber != "")
                {
                    Session["MongoAdminDatabase"] = "admin";
                    Session["MongoAdminUserId"] = txtAdminUserId.Text;
                    Session["MongoAdminPassword"] = txtAdminPassword.Text;

                    //Check DB and Collection already exists or not

                    //Create Database and Insert Collections Role and User
                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}