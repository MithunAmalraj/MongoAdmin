using MongoAdmin.BO;
using MongoAdmin.Logger;
using MongoAdmin.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MongoAdmin
{
    public partial class MongoQuery : BasePage
    {
        private Logging _logObj = new Logging();
        private DatabaseBO _databaseBO;
        private CollectionBO _collectionBO;
        private DocumentBO _documentBO;

        public MongoQuery()
        {
            _databaseBO = new DatabaseBO(_appSettings);
            _collectionBO = new CollectionBO(_appSettings);
            _documentBO = new DocumentBO(_appSettings);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.MaintainScrollPositionOnPostBack = true;
                if (!IsPostBack)
                {
                    if (_appSettings.MongoDBName == "admin")
                    {
                        LoadAllDatabases();
                    }
                    else
                    {
                        LoadCurrentDatabase();
                    }
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

        public void LoadAllDatabases()
        {
            try
            {
                ddlDatabase.Items.Clear();
                ddlDatabase.Items.Add(new ListItem("Select a Database", "0"));
                ddlCollectionName.Items.Clear();
                ddlCollectionName.Items.Add(new ListItem("Select a Collection", "0"));
                ddlLogFields.Items.Clear();
                ddlLogFields.Items.Add(new ListItem("Select a Field", "0"));

                grdDynamicDocuments.DataSource = null;
                grdDynamicDocuments.DataBind();
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                txtFieldValue.Text = "";
                List<Database> databaseList = new List<Database>();
                databaseList = _databaseBO.SelectAllDatabase();
                if (databaseList != null && databaseList.Count > 0)
                {
                    foreach (var item in databaseList)
                    {
                        ddlDatabase.Items.Add(new ListItem(item.DatabaseName, item.DatabaseName));
                    }
                }
                else
                {
                    this.Master.ErrorMessage = "Databases Not Found";
                }
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
                ddlDatabase.Items.Clear();
                ddlDatabase.Items.Add(new ListItem("Select a Database", "0"));
                ddlCollectionName.Items.Clear();
                ddlCollectionName.Items.Add(new ListItem("Select a Collection", "0"));
                ddlLogFields.Items.Clear();
                ddlLogFields.Items.Add(new ListItem("Select a Field", "0"));

                grdDynamicDocuments.DataSource = null;
                grdDynamicDocuments.DataBind();
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                txtFieldValue.Text = "";
                string DBName = _databaseBO.GetCurrentDatabase();
                if (DBName != null && DBName != "")
                {
                    ddlDatabase.Items.Add(new ListItem(DBName, DBName));
                    ddlDatabase.SelectedValue = DBName;
                    lblDatabaseName.Text = DBName;
                    List<Collection> collectionList = new List<Collection>();
                    collectionList = _collectionBO.SelectAllCollection(DBName);
                    if (collectionList != null && collectionList.Count > 0)
                    {
                        foreach (var item in collectionList)
                        {
                            ddlCollectionName.Items.Add(new ListItem(item.CollectionName, item.CollectionName));
                        }

                        ltrlCollectionCount.Text = collectionList.Count.ToString() + " Collections Found";
                        divCollections.Visible = true;
                    }
                    else
                    {
                        this.Master.ErrorMessage = "Collections Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void ddlDatabase_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                ddlCollectionName.Items.Clear();
                ddlCollectionName.Items.Add(new ListItem("Select a Collection", "0"));
                ddlLogFields.Items.Clear();
                ddlLogFields.Items.Add(new ListItem("Select a Field", "0"));

                grdDynamicDocuments.DataSource = null;
                grdDynamicDocuments.DataBind();
                lblCollectionName.Text = "";
                lblCollectionDocumentCount.Text = "";
                txtFieldValue.Text = "";

                string DatabaseName = ddlDatabase.SelectedValue;
                if (DatabaseName != null && DatabaseName != "")
                {
                    lblDatabaseName.Text = DatabaseName;
                    List<Collection> collectionList = new List<Collection>();
                    collectionList = _collectionBO.SelectAllCollection(DatabaseName);
                    if (collectionList != null && collectionList.Count > 0)
                    {
                        foreach (var item in collectionList)
                        {
                            ddlCollectionName.Items.Add(new ListItem(item.CollectionName, item.CollectionName));
                        }

                        ltrlCollectionCount.Text = collectionList.Count.ToString() + " Collections Found";

                        divCollections.Visible = true;

                        //HtmlGenericControl li = new HtmlGenericControl("li");
                        //olbreadcrumb.Controls.Add(li);

                        //HtmlGenericControl anchor = new HtmlGenericControl("label");
                        ////anchor.Attributes.Add("href", "MongoQuery.aspx");
                        //anchor.InnerText = " / Collection";

                        //li.Controls.Add(anchor);
                    }
                    else
                    {
                        this.Master.ErrorMessage = "Collections Not Found";
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void ddlCollectionName_OnSelectedIndexChanged(object sender, System.EventArgs e)
        {
            ddlLogFields.Items.Clear();
            ddlLogFields.Items.Add(new ListItem("Select a Field", "0"));

            grdDynamicDocuments.DataSource = null;
            grdDynamicDocuments.DataBind();
            lblCollectionName.Text = "";
            lblCollectionDocumentCount.Text = "";
            txtFieldValue.Text = "";
            try
            {
                lblCollectionName.Text = "";
                //Get the button that raised the event
                string CollectionName = ddlCollectionName.SelectedValue;
                if (CollectionName != null && CollectionName != "")
                {
                    lblCollectionName.Text = CollectionName;
                    List<DocumentFields> docFields = new List<DocumentFields>();
                    docFields = _documentBO.GetFieldNames(lblDatabaseName.Text, lblCollectionName.Text);
                    if (docFields != null && docFields.Count > 0)
                    {
                        foreach (var item in docFields)
                        {
                            ddlLogFields.Items.Add(new ListItem(item.FieldName, item.FieldName));
                        }
                    }
                    else
                    {
                        this.Master.ErrorMessage = "Collection Fields Not Found";
                    }
                    divQuery.Visible = true;
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnFindDocument_Click(object sender, System.EventArgs e)
        {
            try
            {
                grdDynamicDocuments.DataSource = null;
                grdDynamicDocuments.DataBind();

                if (lblDatabaseName.Text == "")
                {
                    this.Master.ErrorMessage = "Please select Database";
                    return;
                }
                if (lblCollectionName.Text == "")
                {
                    this.Master.ErrorMessage = "Please select Collection";
                    return;
                }

                int PageNumber = 1;
                if (txtPageNumber.Text.Trim() == "")
                {
                    PageNumber = 1;
                }
                else
                {
                    PageNumber = Convert.ToInt32(txtPageNumber.Text.Trim());
                }
                int PageCount = 500;
                if (txtPageCount.Text.Trim() == "")
                {
                    PageCount = 1;
                }
                else
                {
                    PageCount = Convert.ToInt32(txtPageCount.Text.Trim());
                }
                MongoCommand cmd = new MongoCommand();
                if (lblMultipleFieldWithValue.Text != null && lblMultipleFieldWithValue.Text != "")
                {
                    string[] fieldvalues = lblMultipleFieldWithValue.Text.Split('|');
                    if (fieldvalues != null && fieldvalues.Length > 0)
                    {
                        foreach (var item in fieldvalues)
                        {
                            var itemsplit = item.Split(':');

                            bool MatchExact = true;
                            if (itemsplit[2] == "NOT MATCH EXACT")
                                MatchExact = false;

                            if (itemsplit[3] == "ASC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, true));
                            else if (itemsplit[3] == "DESC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, false));
                            else
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact));
                        }
                    }
                }
                else
                {
                    if (ddlLogFields.SelectedValue != "0")
                    {
                        bool MatchExact = true;
                        if (ddlMatchExactValue.SelectedValue == "NOT MATCH EXACT")
                            MatchExact = false;
                        if (rbSortAscending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, true));
                        else if (rbSortDescending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, false));
                        else
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact));
                    }
                }

                DataSet ds = _documentBO.GetDocumentsWithPagination(cmd, lblDatabaseName.Text, lblCollectionName.Text, PageCount, PageNumber);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable result = ds.Tables[0];
                    if (result != null && result.Rows.Count > 0)
                    {
                        //Binding gridview from dynamic object
                        lblgrdDynamicDocumentsCount.Text = result.Rows.Count.ToString() + " record(s) found";
                        grdDynamicDocuments.DataSource = result;
                        grdDynamicDocuments.DataBind();
                    }
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
                divDatabases.Visible = true;
                divCollections.Visible = false;
                divQuery.Visible = false;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnExportAsExcel_Click(object sender, EventArgs e)
        {
            try
            {
                int PageNumber = 1;
                if (txtPageNumber.Text.Trim() == "")
                {
                    PageNumber = 1;
                }
                else
                {
                    PageNumber = Convert.ToInt32(txtPageNumber.Text.Trim());
                }
                int PageCount = 500;
                if (txtPageCount.Text.Trim() == "")
                {
                    PageCount = 1;
                }
                else
                {
                    PageCount = Convert.ToInt32(txtPageCount.Text.Trim());
                }

                MongoCommand cmd = new MongoCommand();
                if (lblMultipleFieldWithValue.Text != null && lblMultipleFieldWithValue.Text != "")
                {
                    string[] fieldvalues = lblMultipleFieldWithValue.Text.Split('|');
                    if (fieldvalues != null && fieldvalues.Length > 0)
                    {
                        foreach (var item in fieldvalues)
                        {
                            var itemsplit = item.Split(':');

                            bool MatchExact = true;
                            if (itemsplit[2] == "NOT MATCH EXACT")
                                MatchExact = false;

                            if (itemsplit[3] == "ASC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, true));
                            else if (itemsplit[3] == "DESC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, false));
                            else
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact));
                        }
                    }
                }
                else
                {
                    if (ddlLogFields.SelectedValue != "0")
                    {
                        bool MatchExact = true;
                        if (ddlMatchExactValue.SelectedValue == "NOT MATCH EXACT")
                            MatchExact = false;
                        if (rbSortAscending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, true));
                        else if (rbSortDescending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, false));
                        else
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact));
                    }
                }

                DataSet ds = _documentBO.GetDocumentsWithPagination(cmd, lblDatabaseName.Text, lblCollectionName.Text, PageCount, PageNumber);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable result = ds.Tables[0];
                    if (result != null && result.Rows.Count > 0)
                    {
                        string CSVString = DataTableToCSV(result, ',');
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition",
                         "attachment;filename=MongoQueryResultExport.csv");
                        Response.Charset = "";
                        Response.ContentType = "application/text";
                        //append new line
                        Response.Output.Write(CSVString);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnExportAsXLSX_Click(object sender, EventArgs e)
        {
            try
            {
                int PageNumber = 1;
                if (txtPageNumber.Text.Trim() == "")
                {
                    PageNumber = 1;
                }
                else
                {
                    PageNumber = Convert.ToInt32(txtPageNumber.Text.Trim());
                }
                int PageCount = 500;
                if (txtPageCount.Text.Trim() == "")
                {
                    PageCount = 1;
                }
                else
                {
                    PageCount = Convert.ToInt32(txtPageCount.Text.Trim());
                }

                MongoCommand cmd = new MongoCommand();
                if (lblMultipleFieldWithValue.Text != null && lblMultipleFieldWithValue.Text != "")
                {
                    string[] fieldvalues = lblMultipleFieldWithValue.Text.Split('|');
                    if (fieldvalues != null && fieldvalues.Length > 0)
                    {
                        foreach (var item in fieldvalues)
                        {
                            var itemsplit = item.Split(':');

                            bool MatchExact = true;
                            if (itemsplit[2] == "NOT MATCH EXACT")
                                MatchExact = false;

                            if (itemsplit[3] == "ASC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, true));
                            else if (itemsplit[3] == "DESC")
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact, true, false));
                            else
                                cmd.Parameters.Add(new MongoParameter(itemsplit[0], itemsplit[1], MatchExact));
                        }
                    }
                }
                else
                {
                    if (ddlLogFields.SelectedValue != "0")
                    {
                        bool MatchExact = true;
                        if (ddlMatchExactValue.SelectedValue == "NOT MATCH EXACT")
                            MatchExact = false;
                        if (rbSortAscending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, true));
                        else if (rbSortDescending.Checked == true)
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact, true, false));
                        else
                            cmd.Parameters.Add(new MongoParameter(ddlLogFields.SelectedValue, txtFieldValue.Text, MatchExact));
                    }
                }

                DataSet ds = _documentBO.GetDocumentsWithPagination(cmd, lblDatabaseName.Text, lblCollectionName.Text, PageCount, PageNumber);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable result = ds.Tables[0];
                    if (result != null && result.Rows.Count > 0)
                    {
                        GridView GVWGrid = new GridView();
                        GVWGrid.DataSource = result;
                        GVWGrid.DataBind();
                        Response.ClearContent();
                        Response.Buffer = true;
                        Response.AddHeader("Content-disposition", string.Format("attachement; filename={0}", "MongoQueryResultExport.xls"));
                        Response.ContentType = "application/ms-excel";
                        StringWriter sw1 = new StringWriter();
                        HtmlTextWriter html1 = new HtmlTextWriter(sw1);
                        GVWGrid.RenderControl(html1);
                        Response.Write(sw1.ToString());
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        public string DataTableToCSV(DataTable datatable, char seperator)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(datatable.Columns[i]);
                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
                foreach (DataRow dr in datatable.Rows)
                {
                    for (int i = 0; i < datatable.Columns.Count; i++)
                    {
                        sb.Append(dr[i].ToString());

                        if (i < datatable.Columns.Count - 1)
                            sb.Append(seperator);
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
            return "";
        }

        protected void btnAddField_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlLogFields.SelectedValue == "0")
                {
                    this.Master.ErrorMessage = "Please select a Field";
                    return;
                }
                string ASCorDESC = "NONE";
                if (rbSortAscending.Checked == true)
                    ASCorDESC = "ASC";
                else if (rbSortDescending.Checked == true)
                    ASCorDESC = "DESC";

                if (lblMultipleFieldWithValue.Text != "")
                    lblMultipleFieldWithValue.Text = lblMultipleFieldWithValue.Text + "|" + ddlLogFields.SelectedValue + ":" + txtFieldValue.Text + ":" + ddlMatchExactValue.SelectedValue + ":" + ASCorDESC;
                else
                    lblMultipleFieldWithValue.Text = ddlLogFields.SelectedValue + ":" + txtFieldValue.Text + ":" + ddlMatchExactValue.SelectedValue + ":" + ASCorDESC;
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void btnFieldClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblgrdDynamicDocumentsCount.Text = "";
                lblMultipleFieldWithValue.Text = "";
                txtFieldValue.Text = "";
                grdDynamicDocuments.DataSource = null;
                grdDynamicDocuments.DataBind();
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

        protected void ddlLogFields_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtFieldValue.Text = "";
            }
            catch (Exception ex)
            {
                _logObj.LogError("MongoAdmin", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, null, ex.Message, _appSettings);
            }
        }

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

        public void EnableDisableAccess()
        {
            try
            {
                lnkbtnFindDocument.Visible = false;
                lnkbtnExportAsExcel.Visible = false;
                lnkbtnExportAsXLSX.Visible = false;

                if (_appSettings.ReadAccess == true)
                {
                    lnkbtnFindDocument.Visible = true;

                    if (_appSettings.ExportAccess == true)
                    {
                        lnkbtnExportAsExcel.Visible = true;
                        lnkbtnExportAsXLSX.Visible = true;
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