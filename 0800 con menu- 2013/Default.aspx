<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>


<!------ Include the above in your HEAD tag ---------->


<!--Pulling Awesome Font -->
<link href="css/bootstrap.min.css" rel="stylesheet" id="bootstrap-css">
<link href="css/Loguin.css" rel="stylesheet">
<link href="css/font-awesome.css" rel="stylesheet">

<body runat="server">
    <form id="LD" runat="server">

        <!------ Include the above in your HEAD tag ---------->


        <!--Pulling Awesome Font -->


        <div class="container">
            <div class="row">
                <div class="col-md-offset-5 col-md-3">
                    <div class="form-login">
                        <asp:ImageMap ID="ImageMap2" runat="server" ImageAlign="NotSet" ImageUrl="~/Imagenes/logo.png"></asp:ImageMap>
                        <h4>Consola de Turnos</h4>

                        <asp:TextBox ID="txtUsuario" runat="server" class="form-control input-sm chat-input" placeholder="Usuario"></asp:TextBox>
                        </br>
           
                        <asp:TextBox ID="txtContraseña" runat="server" class="form-control input-sm chat-input" TextMode="Password" placeholder="Contraseña"></asp:TextBox>

                        </br>
           
                        <div class="wrapper">

                            <span class="group-btn">


                                <asp:Button ID="btnAceptar" class="btn btn-primary btn-md" runat="server" Text="login" />
                                </br>
                  </br>
                 
                                <asp:Label ID="lblMensajeError" runat="server" Visible="false" CssClass="alert-danger" Text=""></asp:Label>

                            </span>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </form>
</body>
