<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthSetup.aspx.cs" Inherits="MongoAdmin.AuthSetup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />

    <title>Mongo Admin - Login</title>

    <!-- Bootstrap core CSS-->
    <link href="template/vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet" />

    <!-- Custom fonts for this template-->
    <link href="template/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />

    <!-- Custom styles for this template-->
    <link href="template/css/sb-admin.css" rel="stylesheet" />

</head>
<body class="bg-dark">
    <form runat="server">
        <div class="row" style="padding-left: 1em;">
            <div class="col-xs-12">
                <div class="alert alert-danger" id="divErrorMessage" style="display: none">
                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label><strong>!</strong>
                </div>
                <div class="alert alert-success" id="divSuccessMessage" style="display: none">
                    <asp:Label ID="lblSuccessMessage" runat="server"></asp:Label><strong>!</strong>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="card card-login mx-auto mt-5">
                <div class="card-header">Login</div>
                <div class="card-body">
                    <div class="form-group">
                        <div class="form-label-group">
                            <asp:TextBox ID="txtAdminUserId" CssClass="form-control" runat="server" placeholder="Admin User Id"></asp:TextBox>
                            <label for="inputEmail">Mongo Admin User Id</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-label-group">
                            <asp:TextBox ID="txtAdminPassword" TextMode="Password" CssClass="form-control" runat="server" placeholder="Admin Password"></asp:TextBox>
                            <label for="inputEmail">Mongo Admin Password</label>
                        </div>
                    </div>
                    <asp:Button ID="btnSetupAuthDB" runat="server" Text="Setup Auth DB" OnClick="btnSetupAuthDB_Click" />
                    <a class="btn btn-primary btn-block" href="index.html">Login</a>
                </div>
            </div>
        </div>

        <!-- Bootstrap core JavaScript-->
        <script src="template/vendor/jquery/jquery.min.js"></script>
        <script src="template/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

        <!-- Core plugin JavaScript-->
        <script src="template/vendor/jquery-easing/jquery.easing.min.js"></script>
    </form>
</body>
</html>
