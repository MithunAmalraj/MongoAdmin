<%@ Page Title="Mongo Query" MaintainScrollPositionOnPostback="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MongoQuery.aspx.cs" Inherits="MongoAdmin.MongoQuery" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Breadcrumbs-->
    <ol id="olbreadcrumb" class="breadcrumb" runat="server">
        <li class="breadcrumb-item">
            <a href="MongoQuery.aspx">Query</a>
        </li>
        <li class="breadcrumb-item">Databases</li>
    </ol>
    <div id="divDatabases" runat="server">
        <div class="card mb-3">
            <div class="card-header">
                <i class="fas fa-database"></i>
                Databases
            </div>
            <asp:DropDownList ID="ddlDatabase" runat="server" class="form-control" OnSelectedIndexChanged="ddlDatabase_OnSelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
    </div>
    <hr />
    <div id="divCollections" runat="server" visible="false">
        <div class="card mb-3">
            <div class="card-header">
                <i class="fas fa-database"></i>
                <asp:Label ID="lblDatabaseName" Text="" runat="server"></asp:Label>
                &nbsp;&nbsp;&nbsp; Collections -
                <asp:Literal ID="ltrlCollectionCount" runat="server" Text=""></asp:Literal>
            </div>
            <asp:DropDownList ID="ddlCollectionName" runat="server" class="form-control" OnSelectedIndexChanged="ddlCollectionName_OnSelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </div>
    </div>
    <hr />
    <div id="divQuery" runat="server" visible="false">
        <div class="form-group">
            <div class="row">
                <div class="col-md-12">
                    <h3>
                        <asp:Label ID="lblCollectionName" Text="" runat="server"></asp:Label>
                        -
                        <asp:Button ID="btnGetDocumentCount" runat="server" Text="Get Documents Count" CssClass="btn btn-info" OnClick="btnGetDocumentCount_Click" />
                        <asp:Label ID="lblCollectionDocumentCount" Text="" runat="server"></asp:Label>
                        Documents Found
                    </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlLogFields" runat="server" class="form-control">
                        <asp:ListItem Text="Select a Field" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtFieldValue" runat="server" placeholder="Enter Value For The Field" class="form-control">
                    </asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlMatchExactValue" runat="server" class="form-control">
                        <asp:ListItem Text="Match Exact" Value="MATCH EXACT"></asp:ListItem>
                        <asp:ListItem Text="Do Not Match Exact" Value="NOT MATCH EXACT"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:RadioButton ID="rbSortAscending" runat="server" GroupName="QuerySort" Text="Asc" />
                    <asp:RadioButton ID="rbSortDescending" runat="server" GroupName="QuerySort" Text="Desc" />
                    <asp:RadioButton ID="rbSortNone" runat="server" GroupName="QuerySort" Text="None" Checked="true" />
                </div>
                <div class="col-md-1">
                    <asp:TextBox ID="txtPageCount" CssClass="form-control" ToolTip="Page Size" runat="server" Text="500" placeholder="Page Size" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                </div>
                <div class="col-md-1">
                    <asp:TextBox ID="txtPageNumber" CssClass="form-control" ToolTip="Page Number" runat="server" Text="1" placeholder="Page Number" onkeypress="if(event.keyCode<48 || event.keyCode>57)event.returnValue=false;"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <h4 style="background-color: yellow; border-radius: 5px;">
                        <asp:Label ID="lblMultipleFieldWithValue" runat="server" Text=""></asp:Label></h4>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-2">
                    <asp:LinkButton ID="lnkbtnAddField" runat="server" class="form-control btn btn-info" OnClick="btnAddField_Click"><i class="fa fa-tags"></i>&nbsp;Add Field</asp:LinkButton>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="lnkbtnFieldClear" runat="server" class="form-control btn btn-info" OnClick="btnFieldClear_Click"><i class="fa fa-eraser"></i>&nbsp;Clear</asp:LinkButton>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="lnkbtnFindDocument" runat="server" class="form-control btn btn-primary" OnClick="btnFindDocument_Click"><i class="fa fa-search"></i>&nbsp;Find</asp:LinkButton>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="lnkbtnExportAsExcel" runat="server" class="form-control btn btn-success" OnClick="btnExportAsExcel_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export CSV</asp:LinkButton>
                </div>
                <div class="col-md-2">
                    <asp:LinkButton ID="lnkbtnExportAsXLSX" runat="server" class="form-control btn btn-success" OnClick="btnExportAsXLSX_Click"><i class="fa fa-file-excel-o"></i>&nbsp;Export XLS</asp:LinkButton>
                </div>
            </div>
            <br />
            <h4>
                <asp:Label ID="lblgrdDynamicDocumentsCount" runat="server" Text=""></asp:Label></h4>
            <div class="row" style="overflow: auto; height: 350px;">
                <div class="col-md-12">
                    <asp:GridView ID="grdDynamicDocuments" CaptionAlign="Left" class="table table-bordered" GridLines="Both" HeaderStyle-BackColor="#1bc0a7" BorderColor="#1bc0a7"
                        ShowHeaderWhenEmpty="true" HeaderStyle-ForeColor="White" runat="server">
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    <%-- <script>
        function ShowProgress() {
            $(".loading").show();
        }
        $(".loading").hide();
    </script>--%>
</asp:Content>

