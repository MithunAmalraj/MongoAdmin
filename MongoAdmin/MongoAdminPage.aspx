<%@ Page Title="Mongo Admin" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MongoAdminPage.aspx.cs" Inherits="MongoAdmin.MongoAdminPage" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divDatabases" runat="server">
        <div class="card mb-3">
            <div class="card-header">
                <div class="row">
                    <div class="pad1left pad1right">
                        <i class="fa fa-database"></i>
                        Available Databases
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnCreateDatabase" runat="server" class="form-control btn btn-primary" OnClick="btnCreateDatabaseOpen_OnClick"><i class="fa fa-database"></i> Create Database</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="btnDatabaseBack" runat="server" class="form-control btn btn-secondary" PostBackUrl="~/Dashboard.aspx"> <i class="fa fa-step-backward"></i> Back To Home</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="card-body" style="overflow: auto; height: 350px;">
                <div class="table-responsive">
                    <asp:GridView ID="gvwDatabaseNames" AutoGenerateColumns="false" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                        ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server" OnRowDataBound="gvwDatabaseNames_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Button ID="btnOpenDatabase" runat="server" Text="Open Database" CssClass="btn btn-success" OnClick="btnOpenDatabase_OnClick" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Drop" Visible="false" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDatabaseDrop" runat="server" class="btn btn-warning" OnClick="btnDatabaseDrop_OnClick" OnClientClick="if ( ! DatabaseDeleteConfirmation()) return false;" meta:resourcekey="BtnDatabaseDeleteResource1"> <i class="fa fa-remove"></i>&nbsp;&nbsp;Drop Database</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Database Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="lblDatabaseName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"DatabaseName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Size On Disk" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="lblSizeOnDisk" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SizeOnDisk")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            ------------------No Record Found--------------------
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <!-- The Modal -->
            <div class="modal fade" id="myCreateDatabaseModal">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 style="font-weight: bold;">Create Database</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <td><strong>Database Name</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtCreateDatabaseName" runat="server" placeholder="Enter Database Name"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Collection Name</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtCreateDatabaseCollectionName" runat="server" placeholder="Enter Collection Name"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><strong>IsCapped Collection</strong></td>
                                        <td>
                                            <asp:CheckBox ID="IsCappedCollection" Checked="false" CssClass="form-control" runat="server" OnCheckedChanged="IsCappedCollection_CheckedChanged" AutoPostBack="true" Text="Is Capped Collection" />
                                        </td>
                                    </tr>
                                    <tr id="trCappedCollectionSize" runat="server" visible="false">
                                        <td><strong>Capped Collection Size</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtCreateDatabaseCappedCollectionSize" runat="server" placeholder="Enter Capped Collection Size" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <asp:Button ID="btnCreateDatabase" runat="server" CssClass="form-control btn btn-primary" OnClick="btnCreateDatabase_OnClick" Text="Create Database" />
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divCollections" runat="server" visible="false">
        <div class="card mb-3">
            <div class="card-header">
                <div class="row">
                    <div class="pad1left pad1right">
                        <i class="fas fa-database"></i>
                        <asp:Label ID="lblDatabaseName" Text="" runat="server"></asp:Label>
                        -
                        <asp:Literal ID="ltrlCollectionCount" runat="server" Text=""></asp:Literal>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnCreateCollectionOpen" runat="server" class="form-control btn btn-primary" OnClick="lnkbtnCreateCollectionOpen_OnClick"><i class="fa fa-files-o"></i> Create Collection</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnRestoreCollection" runat="server" class="form-control btn btn-primary" OnClick="lnkbtnRestoreCollection_OnClick" ><i class="fa fa-files-o"></i> Restore Collection</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnShowDatabaseStats" runat="server" class="form-control btn btn-info" OnClick="lnkbtnShowDatabaseStats_OnClick"><i class="fa fa-bar-chart"></i> Show Database Stats</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="btnCollectionsBack" runat="server" class="form-control btn btn-secondary" OnClick="btnCollectionsBack_OnClick"> <i class="fa fa-step-backward"></i> Back To Databases</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="card-body" style="overflow: auto; height: 350px;">
                <div class="table-responsive">
                    <asp:HiddenField ID="hdnFieldsAlreadyPresent" runat="server" Value="0" />
                    <asp:GridView ID="gvwCollections" AutoGenerateColumns="false" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                        ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server" OnRowDataBound="gvwCollections_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Query" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Button ID="btnManageCollection" runat="server" Text="Open Collection" CssClass="btn btn-success" OnClick="btnManageCollection_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Backup and Drop" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnCollectionBackupandDrop" runat="server" class="btn btn-warning" OnClick="btnCollectionBackupandDrop_OnClick" OnClientClick="if ( ! CollectionBackupDeleteConfirmation()) return false;" meta:resourcekey="BtnCollectionDeleteResource1"> <i class="fa fa-remove"></i>&nbsp;&nbsp;Backup & Drop Collection</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Drop" Visible="false" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnCollectionDrop" runat="server" class="btn btn-warning" OnClick="btnCollectionDrop_OnClick" OnClientClick="if ( ! CollectionDeleteConfirmation()) return false;" meta:resourcekey="BtnCollectionDeleteResource1"> <i class="fa fa-remove"></i>&nbsp;&nbsp;Drop Collection</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Collection Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="lblCOllectionName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"CollectionName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            ------------------No Record Found--------------------
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
            <!-- The Modal -->
            <div class="modal fade" id="myCreateCollectionModal">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 style="font-weight: bold;">Create Collection</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <td><strong>Collection Name</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtCreateCollectionName" runat="server" placeholder="Enter Collection Name"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td><strong>IsCapped Collection</strong></td>
                                        <td>
                                            <asp:CheckBox ID="chkCreateCollectionIsCapped" Checked="false" CssClass="form-control" runat="server" OnCheckedChanged="chkCreateCollectionIsCapped_CheckedChanged" AutoPostBack="true" Text="Is Capped Collection" />
                                        </td>
                                    </tr>
                                    <tr id="trCreateCollectionCappedCollectionSize" runat="server" visible="false">
                                        <td><strong>Capped Collection Size</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtCreateCollectionCappedCollectionSize" runat="server" placeholder="Enter Capped Collection Size" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <asp:Button ID="btnCreateCollection" runat="server" CssClass="form-control btn btn-primary" OnClick="btnCreateCollection_OnClick" Text="Create Collection" />
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="myRestoreCollectionModal">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 style="font-weight: bold;">Restore Collection</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <td><strong>Collection Name</strong></td>
                                        <td>
                                            <asp:TextBox ID="txtRestoreCollectionName" runat="server" placeholder="Enter Collection Name"></asp:TextBox></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <asp:Button ID="btnRestoreCollection"  runat="server" CssClass="form-control btn btn-primary" OnClick="btnRestoreCollection_OnClick" Text="Restore Collection" />
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- The Modal -->
            <div class="modal fade" id="myDatabaseModal">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 style="font-weight: bold;">Database Stats</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <!-- Modal body -->
                        <div class="modal-body">
                            <table class="table table-bordered table-striped">
                                <tbody>
                                    <tr>
                                        <td><strong>Collections (incl. system.namespaces)</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseCollectionCount" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Data Size</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseDataSize" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Storage Size</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseStorageSize" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Avg Obj Size #</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseAverageObjSize" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Objects #</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseObjectsCount" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Extents #</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseExtents" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Indexes #</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseIndexes" runat="server"></asp:Literal></td>
                                    </tr>
                                    <tr>
                                        <td><strong>Index Size</strong></td>
                                        <td>
                                            <asp:Literal ID="lblDatabaseIndexSize" runat="server"></asp:Literal></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divManageCollections" runat="server" visible="false">
        <div class="card mb-3">
            <div class="card-header">
                <div class="row">
                    <div class="pad1left pad1right">
                        <asp:Label ID="lblCollectionName" Text="" runat="server"></asp:Label>
                        -
                        <asp:Button ID="btnGetDocumentCount" runat="server" Text="Get Documents Count" CssClass="btn btn-info" OnClick="btnGetDocumentCount_Click" />
                        <asp:Label ID="lblCollectionDocumentCount" Text="" runat="server"></asp:Label>
                        Documents Found
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnInsertDocumentOpen" runat="server" class="form-control btn btn-primary" OnClick="lnkbtnInsertDocumentOpen_OnClick"><i class="fa fa-file"></i> Insert Document</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkButtonShowCollectionStats" runat="server" class="form-control btn btn-info" OnClick="lnkButtonShowCollectionStats_OnClick"><i class="fa fa-bar-chart"></i> Show Collection Stats</asp:LinkButton>
                    </div>
                    <div class="pad1left pad1right">
                        <asp:LinkButton ID="lnkbtnManageCollectionBack" runat="server" class="form-control btn btn-secondary" OnClick="lnkbtnManageCollectionBack_Click"><i class="fa fa-step-backward"></i>&nbsp;Back To Collections</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="card-body" style="overflow: auto; height: 350px;">
                <!-- The Modal -->
                <div class="modal fade" id="myInsertDocumentModal">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 style="font-weight: bold;">Insert Document</h4>
                                &nbsp;&nbsp;
                                <asp:Button ID="btnInsertDocumentAddRow" runat="server" CssClass="btn btn-secondary" Visible="false" Text="Add Column" OnClick="btnInsertDocumentAddRow_Click" />
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <asp:GridView ID="gvwInsertDocumentGrid" AutoGenerateColumns="false" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                                    ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNo" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSerialNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"SNo")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFieldName" runat="server" placeholder="Enter Field Name" Text='<%#DataBinder.Eval(Container.DataItem,"FieldName")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Field Value" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFieldValue" runat="server" placeholder="Enter Field Value" Text='<%#DataBinder.Eval(Container.DataItem,"FieldValue")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        ------------------No Record Found--------------------
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <asp:Button ID="lnkbtnInsertDocument" runat="server" CssClass="form-control btn btn-primary" OnClick="lnkbtnInsertDocument_OnClick" Text="Insert Document" />
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- The Modal -->
                <div class="modal fade" id="myCollectionModal">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <!-- Modal Header -->
                            <div class="modal-header">
                                <h4 style="font-weight: bold;">Collection Stats</h4>
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                            </div>
                            <!-- Modal body -->
                            <div class="modal-body">
                                <table class="table table-bordered table-striped">
                                    <tbody>
                                        <tr>
                                            <td>
                                                <strong>Documents</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionDocuments" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Total doc size</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionTotalDocSize" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Average doc size</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionAverageDocSize" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Pre-allocated size</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionPreAllocatedSize" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Indexes</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionIndexCount" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Total index size</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionTotalIndexSize" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Padding factor</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionPaddingFactor" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <strong>Extents</strong>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCollectionExtents" runat="server"></asp:Label></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <!-- Modal footer -->
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <h3>Documents</h3>
                    <asp:GridView ID="gvwCollectionDocuments" CaptionAlign="Left" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                        ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server">
                    </asp:GridView>
                </div>
                <div class="col-md-12">
                    <h3>Indexes</h3>
                    <div id="divManageIndex" runat="server">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlCollectionFields" runat="server" class="form-control">
                                        <asp:ListItem Text="Select a Field" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <asp:LinkButton ID="lnkbtnCreateIndex" runat="server" class="form-control btn btn-primary" OnClick="btnCreateIndex_Click"><i class="fa fa-check"></i>&nbsp;Create Index</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <asp:GridView ID="grdCollectionIndexes" AutoGenerateColumns="false" CaptionAlign="Left" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                        ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server"  OnRowDataBound="grdCollectionIndexes_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Drop" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnIndexDrop" runat="server" class="btn btn-warning" OnClick="btnIndexDrop_OnClick" OnClientClick="if ( ! IndexDeleteConfirmation()) return false;" meta:resourcekey="BtnIndexDeleteResource1"> <i class="fa fa-remove"></i>&nbsp;&nbsp;Drop Index</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Index Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="lblIndexName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"IndexName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Field Name" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="lblFieldName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"FieldName")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function DatabaseDeleteConfirmation() {
            return confirm("Are you sure you want to drop this database?");
        }
        function CollectionBackupDeleteConfirmation() {
            return confirm("Are you sure you want to backup and drop this collection?");
        }
        function CollectionDeleteConfirmation() {
            return confirm("Are you sure you want to drop this collection?");
        }
        function IndexDeleteConfirmation() {
            return confirm("Are you sure you want to drop this index?");
        }
    </script>
</asp:Content>

