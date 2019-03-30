using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoAdmin.BO;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System.Data;
using System.IO;

namespace MongoAdmin
{

    public partial class Dashboard : BasePage
    {
        private Logging _logObj = new Logging();
        private ServerBO _serverBO;
        private DocumentBO _documentBO;
        private DatabaseBO _databaseBO;
        public Dashboard()
        {
            _serverBO = new ServerBO(_appSettings);
            _documentBO = new DocumentBO(_appSettings);
            _databaseBO = new DatabaseBO(_appSettings);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = _documentBO.ExportDocuments();
                //GridView GVWGrid = new GridView();
                //GVWGrid.DataSource = dt;
                //GVWGrid.DataBind();
                //Response.ClearContent();
                //Response.Buffer = true;
                //Response.AddHeader("Content-disposition", string.Format("attachement; filename={0}", "MongoQueryResultExport.xls"));
                //Response.ContentType = "application/ms-excel";
                //StringWriter sw1 = new StringWriter();
                //HtmlTextWriter html1 = new HtmlTextWriter(sw1);
                //GVWGrid.RenderControl(html1);
                //Response.Write(sw1.ToString());
                //Response.End();
                //return;
                //Admin Dashboard
                if (_appSettings.MongoDBName == "admin")
                {
                    ServerStats serverStatsObj = new ServerStats();
                    divAdminDashboard1.Visible = true;
                    divAdminDashboard2.Visible = true;
                    divAdminDashboard3.Visible = true;
                    serverStatsObj = _serverBO.GetServerStats();
                    if (serverStatsObj != null)
                    {
                        lblAdminDash1HostName.Text = serverStatsObj.host;
                        lblAdminDash1MongoVersion.Text = serverStatsObj.version;
                        lblAdminDash1Uptime.Text = serverStatsObj.uptime.ToString();
                        lblAdminDash1ServerTime.Text = serverStatsObj.localTime.ToString();
                        lblAdminDash1CurrentConnections.Text = serverStatsObj.connections;
                        lblAdminDash1AvailableConnections.Text = serverStatsObj.process;
                        lblAdminDash1ActiveClients.Text = "";
                        lblAdminDash1QueuedOperations.Text = "";
                        lblAdminDash1ClientReading.Text = "";
                        lblAdminDash1ClientWriting.Text = "";
                        lblAdminDash1ReadLockQueue.Text = "";
                        lblAdminDash1WriteLockQueue.Text = "";
                        lblAdminDash1DiskFlushes.Text = "";
                        lblAdminDash1LastFlush.Text = "";
                        lblAdminDash1TimeSpentFlushing.Text = "";
                        lblAdminDash1AverageFlushTime.Text = "";
                        lblAdminDash1TotalInserts.Text = "";
                        lblAdminDash1TotalQueries.Text = "";
                        lblAdminDash1TotalUpdates.Text = "";
                        lblAdminDash1TotalDeletes.Text = "";
                    }
                }
                else
                {
                    DatabaseStats databaseStatsObj = _databaseBO.GetDatabaseStats(_appSettings.MongoDBName);
                    divDatabaseDashboard1.Visible = true;
                    if (databaseStatsObj != null)
                    {
                        lblDatabaseAverageObjSize.Text = databaseStatsObj.avgObjSize.ToString();
                        lblDatabaseCollectionCount.Text = databaseStatsObj.collections.ToString();
                        lblDatabaseDataSize.Text = databaseStatsObj.dataSize.ToString();
                        lblDatabaseExtents.Text = databaseStatsObj.numExtents.ToString();
                        lblDatabaseIndexes.Text = databaseStatsObj.indexes.ToString();
                        lblDatabaseIndexSize.Text = databaseStatsObj.indexSize.ToString();
                        lblDatabaseObjectsCount.Text = databaseStatsObj.objects.ToString();
                        lblDatabaseStorageSize.Text = databaseStatsObj.storageSize.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }
    }
}