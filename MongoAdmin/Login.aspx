<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" EnableSessionState="True" Inherits="MongoAdmin.Login" %>

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
    <style>
        body {
            background-image: url('img/mongodb.png');
            /* Full height */
            height: 100%;
            /* Center and scale the image nicely */
            background-position: center;
            background-repeat: no-repeat;
            background-size: cover;
        }

        input[type=radio] {
            display: none;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form runat="server">
        <div class="row" style="padding-left: 1em;">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="alert alert-danger" id="divErrorMessage" style="display: none">
                    <asp:Label ID="lblErrorMessage" runat="server"></asp:Label><strong>!</strong>
                </div>
                <div class="alert alert-success" id="divSuccessMessage" style="display: none">
                    <asp:Label ID="lblSuccessMessage" runat="server"></asp:Label><strong>!</strong>
                </div>
            </div>
        </div>
        <div class="container">
            <div id="demo" class="carousel slide" data-ride="carousel" data-interval="false" >
                <!-- Indicators -->
                <ul class="carousel-indicators">
                    <li data-target="#demo" data-slide-to="0" class="active"></li>
                    <li data-target="#demo" data-slide-to="1"></li>
                </ul>
                <!-- The slideshow -->
                <div class="carousel-inner">
                    <div class="carousel-item active">
                        <div id="divApplicationLogin" runat="server" defaultbutton="lnkbtnApplicationLogin">
                            <div class="card card-login mx-auto mt-5">
                                <div class="card-header">Application Login</div>
                                <div class="card-body">
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtApplicationUserName" CssClass="form-control" runat="server" placeholder="Admin User Name"></asp:TextBox>
                                            <label for="txtApplicationUserName">Application User Name</label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtApplicationPassword" TextMode="Password" CssClass="form-control" runat="server" placeholder="Admin Password"></asp:TextBox>
                                            <label for="txtApplicationPassword">Application Password</label>
                                        </div>
                                    </div>
                                    <asp:LinkButton ID="lnkbtnApplicationLogin" CssClass="btn btn-primary btn-block" runat="server" OnClick="lnkbtnApplicationLogin_Click">Login</asp:LinkButton>
                                    <%--  <div class="text-center">
                        <asp:LinkButton ID="lnkbtnSetupAuthentication" runat="server" CssClass="d-block small" PostBackUrl="~/AuthSetup.aspx">Setup Authentication</asp:LinkButton>
                        <a class="d-block small" href="forgot-password.html">Forgot Password?</a>
                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="carousel-item">
                        <div id="divMongoLogin" runat="server" defaultbutton="lnkbtnMongoLogin">
                            <div class="card card-login mx-auto mt-5">
                                <div class="card-header">Mongo Login</div>
                                <div class="card-body">
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtMongoServer" CssClass="form-control" runat="server" placeholder="Mongo Server"></asp:TextBox>
                                            <label for="txtMongoServer">Mongo Server</label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtMongoPort" CssClass="form-control" runat="server" placeholder="Mongo Port"></asp:TextBox>
                                            <label for="txtMongoPort">Mongo Port</label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtMongoDBName" CssClass="form-control" runat="server" placeholder="Mongo Database"></asp:TextBox>
                                            <label for="txtMongoDBName">Mongo Database</label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtMongoUserName" CssClass="form-control" runat="server" placeholder="Mongo User Name"></asp:TextBox>
                                            <label for="txtMongoUserName">Mongo User Name</label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-label-group">
                                            <asp:TextBox ID="txtMongoPassword" TextMode="Password" CssClass="form-control" runat="server" placeholder="Mongo Password"></asp:TextBox>
                                            <label for="txtMongoPassword">Mongo Password</label>
                                        </div>
                                    </div>
                                    <asp:LinkButton ID="lnkbtnMongoLogin" CssClass="btn btn-primary btn-block" runat="server" OnClick="lnkbtnMongoLogin_Click">Login</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br /> <br /> <br />
                </div>
                <!-- Left and right controls -->
                <a class="carousel-control-prev" href="#demo" data-slide="prev">
                    <span class="carousel-control-prev-icon"></span>
                </a>
                <a class="carousel-control-next" href="#demo" data-slide="next">
                    <span class="carousel-control-next-icon"></span>
                </a>
            </div>
        </div>

        <!-- Bootstrap core JavaScript-->
        <script src="template/vendor/jquery/jquery.min.js"></script>
        <script src="template/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

        <!-- Core plugin JavaScript-->
        <script src="template/vendor/jquery-easing/jquery.easing.min.js"></script>
        <script type="text/javascript">
            //Common For All Screens
            if ($('#<%= lblErrorMessage.ClientID %>').html() != "") {
                $("#divErrorMessage").css("display", "block");

                setTimeout(function () {
                    $('#divErrorMessage').fadeOut('slow');
                }, 10000);
            }

            if ($('#<%= lblSuccessMessage.ClientID %>').html() != "") {
                $("#divSuccessMessage").css("display", "block");

                setTimeout(function () {
                    $('#divSuccessMessage').fadeOut('slow');
                }, 10000);
            }
        </script>
    </form>
</body>
</html>
