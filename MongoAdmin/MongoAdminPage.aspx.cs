using MongoAdmin.BO;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.UI.WebControls;

namespace MongoAdmin
{
    public partial class MongoAdminPage : BasePage
    {
        private Logging _logObj = new Logging();
        private DatabaseBO _databaseBO;
        private CollectionBO _collectionBO;
        private DocumentBO _documentBO;

        public MongoAdminPage()
        {
            _databaseBO = new DatabaseBO(_appSettings);
            _collectionBO = new CollectionBO(_appSettings);
            _documentBO = new DocumentBO(_appSettings);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LoadDatabase();
                    EnableDisableAccess();
                }
                this.Master.SuccessMessage = "";
                this.Master.ErrorMessage = "";
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        #region Database

        public void LoadDatabase()
        {
            try
            {
                if (_appSettings.MongoDBName == "admin")
                {
                    LoadAllDatabases();
                }
                else
                {
                    LoadCurrentDatabase();
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        public void LoadAllDatabases()
        {
            try
            {
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                ddlCollectionFields.Items.Clear();
                ddlCollectionFields.Items.Add(new ListItem("Select a Field", "0"));
                List<Database> databaseList = new List<Database>();
                databaseList = _databaseBO.SelectAllDatabase();
                gvwDatabaseNames.DataSource = databaseList;
                gvwDatabaseNames.DataBind();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        public void LoadCurrentDatabase()
        {
            try
            {
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                ddlCollectionFields.Items.Clear();
                ddlCollectionFields.Items.Add(new ListItem("Select a Field", "0"));

                string DBName = _databaseBO.GetCurrentDatabase();
                if (DBName != null && DBName != "")
                {
                    lblDatabaseName.Text = DBName;

                    List<Collection> collectionList = new List<Collection>();
                    collectionList = _collectionBO.SelectAllCollection(DBName);
                    if (collectionList != null && collectionList.Count > 0)
                    {
                        gvwCollections.DataSource = collectionList;
                        gvwCollections.DataBind();

                        ltrlCollectionCount.Text = collectionList.Count.ToString() + " Collections Found";

                        divDatabases.Visible = false;
                        divCollections.Visible = true;
                    }
                    else
                    {
                        this.Master.ErrorMessage = "Your Error Message here";
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCreateDatabaseOpen_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                txtCreateDatabaseName.Text = "";
                txtCreateDatabaseCollectionName.Text = "";
                txtCreateDatabaseCappedCollectionSize.Text = "";
                IsCappedCollection.Checked = false;
                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myCreateDatabaseModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCreateDatabase_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (txtCreateDatabaseName.Text == "")
                {
                    this.Master.ErrorMessage = "Please enter database name";
                    return;
                }
                if (txtCreateDatabaseCollectionName.Text == "")
                {
                    this.Master.ErrorMessage = "Please enter collection name";
                    return;
                }
                long cappedsize = 0;
                if (IsCappedCollection.Checked == true)
                {
                    if (txtCreateDatabaseCappedCollectionSize.Text == "")
                    {
                        this.Master.ErrorMessage = "Please enter Capped Collection Size";
                        return;
                    }
                    cappedsize = Convert.ToInt64(txtCreateDatabaseCappedCollectionSize.Text);
                }

                _databaseBO.CreateDatabase(txtCreateDatabaseName.Text, txtCreateDatabaseCollectionName.Text, IsCappedCollection.Checked, cappedsize);
                LoadDatabase();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void lnkbtnShowDatabaseStats_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                string DatabaseName = lblDatabaseName.Text;
                if (DatabaseName != null && DatabaseName != "")
                {
                    DatabaseStats databaseStatsObj = _databaseBO.GetDatabaseStats(DatabaseName);
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

                        ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myDatabaseModal').modal('toggle');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnDatabaseDrop_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string DatabaseName = ((Label)gvr.FindControl("lblDatabaseName")).Text.ToString();
                if (DatabaseName != null && DatabaseName != "")
                {
                    _databaseBO.DropDatabase(DatabaseName);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void IsCappedCollection_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                trCappedCollectionSize.Visible = false;
                if (IsCappedCollection.Checked == true)
                {
                    trCappedCollectionSize.Visible = true;
                }
                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myCreateDatabaseModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        #endregion Database

        #region Collection

        protected void lnkbtnCreateCollectionOpen_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                txtCreateCollectionName.Text = "";
                txtCreateCollectionCappedCollectionSize.Text = "";
                chkCreateCollectionIsCapped.Checked = false;
                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myCreateCollectionModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void lnkbtnRestoreCollection_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                txtRestoreCollectionName.Text = "";
                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myRestoreCollectionModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCreateCollection_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                if (txtCreateCollectionName.Text.Trim() == "")
                {
                    this.Master.ErrorMessage = "Please enter collection name";
                    return;
                }
                long cappedsize = 0;
                if (chkCreateCollectionIsCapped.Checked == true)
                {
                    if (txtCreateCollectionCappedCollectionSize.Text == "")
                    {
                        this.Master.ErrorMessage = "Please enter Capped Collection Size";
                        return;
                    }
                    cappedsize = Convert.ToInt64(txtCreateCollectionCappedCollectionSize.Text);
                }

                _collectionBO.CreateCollection(lblDatabaseName.Text, txtCreateCollectionName.Text, chkCreateCollectionIsCapped.Checked, cappedsize);
                LoadCollections(lblDatabaseName.Text);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnRestoreCollection_OnClick(object sender, System.EventArgs e)
        {
            CollectionBackupAndRestore collResObj = new CollectionBackupAndRestore();
            collResObj.MongoServerPath = WebConfigurationManager.AppSettings["MongoServerPath"];
            collResObj.MongoBackupRARPath = WebConfigurationManager.AppSettings["MongoBackupRARPath"];
            collResObj.MongoRestorePath = WebConfigurationManager.AppSettings["MongoRestorePath"];
            collResObj.WinRARPath = WebConfigurationManager.AppSettings["WinRARPath"];
            collResObj.BatchFilePath = WebConfigurationManager.AppSettings["BatchFilePath"];
            collResObj.LogFilePath = WebConfigurationManager.AppSettings["LogFilePath"];

            try
            {
                collResObj.DatabaseName = lblDatabaseName.Text;

                if (txtRestoreCollectionName.Text.Trim() == "")
                {
                    this.Master.ErrorMessage = "Please enter collection name";
                    return;
                }
                else
                {
                    collResObj.CollectionName = txtRestoreCollectionName.Text;
                }

                GenericResponse GR = new GenericResponse();
                GR = _collectionBO.RestoreCollection(collResObj);
                if (GR != null && GR.response_code == 0)
                {
                    this.Master.SuccessMessage = GR.response_message;
                    LoadCollections(lblDatabaseName.Text);
                }
                else
                {
                    if (GR.response_message != null && GR.response_message != "")
                        this.Master.ErrorMessage = GR.response_message;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnOpenDatabase_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                ddlCollectionFields.Items.Clear();
                ddlCollectionFields.Items.Add(new ListItem("Select a Field", "0"));

                //Get the button that raised the event
                Button btn = (Button)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string DatabaseName = ((Label)gvr.FindControl("lblDatabaseName")).Text.ToString();
                if (DatabaseName != null && DatabaseName != "")
                {
                    lblDatabaseName.Text = DatabaseName;
                    LoadCollections(DatabaseName);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        public void LoadCollections(string DatabaseName)
        {
            try
            {
                List<Collection> collectionList = new List<Collection>();
                collectionList = _collectionBO.SelectAllCollection(DatabaseName);
                if (collectionList != null && collectionList.Count > 0)
                {
                    gvwCollections.DataSource = collectionList;
                    gvwCollections.DataBind();

                    ltrlCollectionCount.Text = collectionList.Count.ToString() + " Collections Found";

                    divDatabases.Visible = false;
                    divCollections.Visible = true;
                    divManageCollections.Visible = false;
                }
                else
                {
                    this.Master.ErrorMessage = "Your Error Message here";
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCollectionsBack_OnClick(object sender, EventArgs e)
        {
            try
            {
                lblDatabaseName.Text = "";
                lblCollectionName.Text = "";

                divDatabases.Visible = true;
                divCollections.Visible = false;
                divManageCollections.Visible = false;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void lnkbtnManageCollectionBack_Click(object sender, EventArgs e)
        {
            try
            {
                lblCollectionName.Text = "";

                divDatabases.Visible = false;
                divCollections.Visible = true;
                divManageCollections.Visible = false;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCollectionBackupandDrop_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string CollectionName = ((Label)gvr.FindControl("lblCOllectionName")).Text.ToString();

                CollectionBackupAndRestore collResObj = new CollectionBackupAndRestore();
                collResObj.MongoServerPath = WebConfigurationManager.AppSettings["MongoServerPath"];
                collResObj.MongoBackupRARPath = WebConfigurationManager.AppSettings["MongoBackupRARPath"];
                collResObj.MongoBackupPath = WebConfigurationManager.AppSettings["MongoBackupPath"];
                collResObj.MongoRestorePath = WebConfigurationManager.AppSettings["MongoRestorePath"];
                collResObj.WinRARPath = WebConfigurationManager.AppSettings["WinRARPath"];
                collResObj.BatchFilePath = WebConfigurationManager.AppSettings["BatchFilePath"];
                collResObj.LogFilePath = WebConfigurationManager.AppSettings["LogFilePath"];
                collResObj.DatabaseName = lblDatabaseName.Text;
                collResObj.CollectionName = CollectionName;
                if (lblDatabaseName.Text != "" && CollectionName != null && CollectionName != "")
                {
                    GenericResponse GR = new GenericResponse();
                    GR = _collectionBO.BackupandDropCollection(collResObj);
                    if (GR != null && GR.response_code == 0)
                    {
                        this.Master.SuccessMessage = GR.response_message;
                        LoadCollections(lblDatabaseName.Text);
                    }
                    else
                    {
                        if (GR.response_message != null && GR.response_message != "")
                            this.Master.ErrorMessage = GR.response_message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCollectionDrop_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string CollectionName = ((Label)gvr.FindControl("lblCOllectionName")).Text.ToString();
                if (lblDatabaseName.Text != "" && CollectionName != null && CollectionName != "")
                {
                    _collectionBO.DropCollection(lblDatabaseName.Text, CollectionName);
                    LoadCollections(lblDatabaseName.Text);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnManageCollection_Click(object sender, EventArgs e)
        {
            try
            {
                divDatabases.Visible = false;
                divCollections.Visible = false;
                divManageCollections.Visible = true;
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                grdCollectionIndexes.DataSource = null;
                grdCollectionIndexes.DataBind();
                ddlCollectionFields.Items.Clear();
                ddlCollectionFields.Items.Add(new ListItem("Select a Field", "0"));

                Button btn = (Button)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string CollectionName = ((Label)gvr.FindControl("lblCOllectionName")).Text.ToString();
                if (CollectionName != null && CollectionName != "")
                {
                    lblCollectionName.Text = CollectionName;

                    DisplayDocuments(lblDatabaseName.Text, lblCollectionName.Text);

                    LoadIndexList(lblDatabaseName.Text, lblCollectionName.Text);

                    List<DocumentFields> docFields = new List<DocumentFields>();
                    docFields = _documentBO.GetFieldNames(lblDatabaseName.Text, lblCollectionName.Text);
                    if (docFields != null && docFields.Count > 0)
                    {
                        foreach (var item in docFields)
                        {
                            ddlCollectionFields.Items.Add(new ListItem(item.FieldName, item.FieldName));
                        }
                    }
                    else
                    {
                        this.Master.ErrorMessage = "Collection Fields Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        public void DisplayDocuments(string DatabaseName, string CollectionName)
        {
            try
            {
                //gvwCollectionDocuments.DataSource = _documentBO.FindDocumentsWithPagination(DatabaseName, CollectionName, "ProjectName", "WLController", "", "", "", 1);
                //gvwCollectionDocuments.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadIndexList(string DatabaseName, string CollectionName)
        {
            try
            {
                //Get Indexes In Collection
                List<CollectionIndexList> indexList = new List<CollectionIndexList>();
                indexList = _collectionBO.GetAllCollectionIndexes(DatabaseName, CollectionName);
                if (indexList != null && indexList.Count > 0)
                {
                    grdCollectionIndexes.DataSource = indexList;
                    grdCollectionIndexes.DataBind();
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void lnkButtonShowCollectionStats_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                string DatabaseName = lblDatabaseName.Text;
                string CollectionName = lblCollectionName.Text;
                if (DatabaseName != null && DatabaseName != "" && CollectionName != null && CollectionName != "")
                {
                    CollectionStats myObj = new CollectionStats();
                    myObj = _collectionBO.GetCollectionStats(DatabaseName, CollectionName);
                    if (myObj != null)
                    {
                        lblCollectionDocuments.Text = myObj.count.ToString();
                        lblCollectionAverageDocSize.Text = myObj.avgObjSize.ToString();
                        lblCollectionExtents.Text = "";
                        lblCollectionIndexCount.Text = myObj.nindexes.ToString();
                        lblCollectionPaddingFactor.Text = "";
                        lblCollectionPreAllocatedSize.Text = myObj.storageSize.ToString();
                        lblCollectionTotalDocSize.Text = myObj.size.ToString();
                        lblCollectionTotalIndexSize.Text = myObj.totalIndexSize.ToString();

                        ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myCollectionModal').modal('toggle');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnCreateIndex_Click(object sender, EventArgs e)
        {
            try
            {
                string SelectedFieldName = ddlCollectionFields.SelectedValue;
                if (SelectedFieldName == "0")
                {
                    this.Master.ErrorMessage = "Please select a field to create index";
                    return;
                }
                if (grdCollectionIndexes != null && grdCollectionIndexes.Rows.Count > 0)
                {
                    //Check whether index is already present
                    foreach (GridViewRow row in grdCollectionIndexes.Rows)
                    {
                        Label IndexName = (Label)row.FindControl("lblIndexName");
                        Label FieldName = (Label)row.FindControl("lblFieldName");
                        //for bound field
                        if (SelectedFieldName == FieldName.Text)
                        {
                            this.Master.ErrorMessage = "Index already exists for selected field";
                            return;
                        }
                    }
                }
                _collectionBO.CreateIndex(lblDatabaseName.Text, lblCollectionName.Text, ddlCollectionFields.SelectedValue, "");
                LoadIndexList(lblDatabaseName.Text, lblCollectionName.Text);
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnIndexDrop_OnClick(object sender, System.EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                GridViewRow gvr = (GridViewRow)btn.NamingContainer;
                string IndexName = ((Label)gvr.FindControl("lblIndexName")).Text.ToString();

                if (lblDatabaseName.Text != "" && lblCollectionName.Text != "" && IndexName != null && IndexName != "")
                {
                    _collectionBO.DropIndex(lblDatabaseName.Text, lblCollectionName.Text, IndexName);
                    LoadIndexList(lblDatabaseName.Text, lblCollectionName.Text);
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void chkCreateCollectionIsCapped_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                trCreateCollectionCappedCollectionSize.Visible = false;
                if (chkCreateCollectionIsCapped.Checked == true)
                {
                    trCreateCollectionCappedCollectionSize.Visible = true;
                }
                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myCreateCollectionModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        #endregion Collection

        #region Document

        protected void lnkbtnInsertDocumentOpen_OnClick(object sender, EventArgs e)
        {
            try
            {
                btnInsertDocumentAddRow.Visible = false;
                gvwInsertDocumentGrid.DataSource = null;
                gvwInsertDocumentGrid.DataBind();
                //Get Document Fields
                //If Document count is greater than 0 do not allow to create fields
                List<InsertDocument> docFieldList = new List<InsertDocument>();
                int SNo = 0;
                foreach (ListItem li in ddlCollectionFields.Items)
                {
                    if (li.Value != "0" && li.Value != "_id")
                    {
                        SNo = SNo + 1;
                        InsertDocument docFieldObj = new InsertDocument();
                        docFieldObj.SNo = SNo;
                        docFieldObj.FieldName = li.Value;
                        docFieldObj.FieldValue = "";
                        docFieldList.Add(docFieldObj);
                    }
                }
                if (docFieldList != null && docFieldList.Count > 0)
                {
                    hdnFieldsAlreadyPresent.Value = "1";
                    gvwInsertDocumentGrid.DataSource = docFieldList;
                    gvwInsertDocumentGrid.DataBind();

                    foreach (GridViewRow grv in gvwInsertDocumentGrid.Rows)
                    {
                        TextBox txtFieldName = (TextBox)grv.FindControl("txtFieldName");
                        txtFieldName.Enabled = false;
                    }
                }
                else
                {
                    btnInsertDocumentAddRow.Visible = true;
                }
                //If no document is found user can create fields

                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myInsertDocumentModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void lnkbtnInsertDocument_OnClick(object sender, EventArgs e)
        {
            try
            {
                MongoCommand cmd = new MongoCommand();
                foreach (GridViewRow grv in gvwInsertDocumentGrid.Rows)
                {
                    TextBox FieldName = (TextBox)grv.FindControl("txtFieldName");
                    TextBox FieldValue = (TextBox)grv.FindControl("txtFieldValue");

                    if (FieldName.Text == "")
                    {
                        this.Master.ErrorMessage = "Field Name cannot be empty";
                        return;
                    }
                    if (FieldValue.Text == "")
                    {
                        this.Master.ErrorMessage = "Field Value cannot be empty";
                        return;
                    }
                    cmd.Parameters.Add(new MongoParameter(FieldName.Text, FieldValue.Text));
                }
                if (_documentBO.InsertDocument(cmd, lblDatabaseName.Text, lblCollectionName.Text))
                {
                    this.Master.SuccessMessage = "Document inserted successfully";
                    return;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnInsertDocumentAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                List<InsertDocument> docFieldList = new List<InsertDocument>();
                int LastSerialNo = 0;
                InsertDocument docFieldObj = new InsertDocument();
                foreach (GridViewRow grv in gvwInsertDocumentGrid.Rows)
                {
                    docFieldObj = new InsertDocument();
                    Label SNo = (Label)grv.FindControl("lblSerialNo");
                    TextBox FieldName = (TextBox)grv.FindControl("txtFieldName");
                    TextBox FieldValue = (TextBox)grv.FindControl("txtFieldValue");

                    docFieldObj.SNo = Convert.ToInt32(SNo.Text);
                    docFieldObj.FieldName = FieldName.Text;
                    docFieldObj.FieldValue = FieldValue.Text;
                    docFieldList.Add(docFieldObj);
                    LastSerialNo = Convert.ToInt32(SNo.Text);
                }

                docFieldObj = new InsertDocument();
                docFieldObj.SNo = LastSerialNo + 1;
                docFieldObj.FieldName = "";
                docFieldObj.FieldValue = "";
                docFieldList.Add(docFieldObj);

                gvwInsertDocumentGrid.DataSource = docFieldList;
                gvwInsertDocumentGrid.DataBind();

                ClientScript.RegisterStartupScript(GetType(), "Show", "<script> $('#myInsertDocumentModal').modal('toggle');</script>");
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        #endregion Document

        protected void btnGetDocumentCount_Click(object sender, EventArgs e)
        {
            try
            {
                lblCollectionDocumentCount.Text = _documentBO.GetTotalDocumentsInCollectionCount(lblDatabaseName.Text, lblCollectionName.Text).ToString();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void gvwDatabaseNames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnOpenDatabase = ((Button)(e.Row.FindControl("btnOpenDatabase")));
                LinkButton btnDatabaseDrop = ((LinkButton)(e.Row.FindControl("btnDatabaseDrop")));

                btnDatabaseDrop.Visible = false;

                if (_appSettings.DeleteAccess == true)
                {
                    btnDatabaseDrop.Visible = false;
                }
            }
        }
        protected void gvwCollections_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnManageCollection = ((Button)(e.Row.FindControl("btnManageCollection")));
                LinkButton btnCollectionBackupandDrop = ((LinkButton)(e.Row.FindControl("btnCollectionBackupandDrop")));
                LinkButton btnCollectionDrop = ((LinkButton)(e.Row.FindControl("btnCollectionDrop")));

                btnManageCollection.Visible = false;
                btnCollectionBackupandDrop.Visible = false;
                btnCollectionDrop.Visible = false;

                if (_appSettings.UpdateAccess == true)
                {
                    btnManageCollection.Visible = true;
                }

                if (_appSettings.DeleteAccess == true)
                {
                    btnCollectionDrop.Visible = true;
                }
                if (_appSettings.ExportAccess == true && _appSettings.DeleteAccess == true)
                {
                    btnCollectionBackupandDrop.Visible = true;
                }
                if (_appSettings.UpdateAccess == true && _appSettings.DeleteAccess == true)
                {
                    btnManageCollection.Visible = true;
                }
            }
        }
        protected void grdCollectionIndexes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnIndexDrop = ((LinkButton)(e.Row.FindControl("btnIndexDrop")));

                btnIndexDrop.Visible = false;

                if (_appSettings.CreateAccess == true)
                {
                    btnIndexDrop.Visible = false;
                }
            }
        }

        public void EnableDisableAccess()
        {
            try
            {
                lnkbtnCreateDatabase.Visible = false;
                lnkbtnCreateCollectionOpen.Visible = false;
                lnkbtnRestoreCollection.Visible = false;
                lnkbtnInsertDocumentOpen.Visible = false;
                lnkbtnCreateIndex.Visible = false;

                if (_appSettings.CreateAccess == true)
                {
                    lnkbtnCreateDatabase.Visible = true;
                    lnkbtnCreateCollectionOpen.Visible = true;
                    lnkbtnRestoreCollection.Visible = true;
                    lnkbtnInsertDocumentOpen.Visible = true;
                    lnkbtnCreateIndex.Visible = true;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }
    }
}