<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AnulacionXCodigo.aspx.vb" Inherits="AnulacionXCodigo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>MLZ - SE 1.0</title>
    <link rel="shortcut icon" href="Ico/lomas.ico" />




    <!-- Then include bootstrap js -->



    <link href="Css/bootstrap.css" rel="stylesheet" />
    <link href="Css/poncho.css" rel="stylesheet" />
    <link href="Css/font-awesome.css" rel="stylesheet" />
    <link href="Css/jquery.smartmenus.bootstrap.css" rel="stylesheet" />
    <link href="Css/select.css" rel="stylesheet" />
    <link href="Css/datatables.bootstrap.css" rel="stylesheet" />
    <link href="Css/angular-bootstrap-toggle.min.css" rel="stylesheet" />
    <link href="Css/Site.css" rel="stylesheet" />
    <link href="Css/jquery-ui.css" rel="stylesheet" type="text/css" />

    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/VentanaModal.js" type="text/javascript"></script>
    <script src="js/jquery-1.11.3.min.js" type="text/javascript"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/VentanaModal.js" type="text/javascript"></script>
    <!-- endbuild -->




    <style type="text/css">
        @import url("fonts/geometria/stylesheet.css");

        body, td, th {
            font-family: "geometria";
            color: #0066A7;
        }
    </style>


</head>

<header>        
        <nav class="navbar navbar-top navbar-default" role="navigation">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#main-navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar" ></span>
                    </button>
                </div>
          
                <div class="collapse navbar-collapse" id="main-navbar-collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <li><a  runat="server" id="btnCerrarSesion" class="navbar-link">Cerrar Sesion</a></li>
                      </ul>  
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="Principal.aspx" class="navbar-link">Principal</a></li>
                      </ul>
                          <ul class="nav navbar-nav navbar-right">
                        <li><a href="Usuarios.aspx" class="navbar-link">Usuarios</a></li>
                      </ul>
                    <ul class="nav navbar-nav navbar-right">
                        <li><a href="ConsultaTurnos.aspx" class="navbar-link">Consulta Turnos</a></li>
                      </ul>
                       <ul class="nav navbar-nav navbar-right">
                        <li><a href="PersonasAtendidas.aspx" class="navbar-link">Personas Atendidas</a></li>
                      </ul>
                     <ul class="nav navbar-nav navbar-right">
                        <li><a href="PersonasenEspera.aspx" class="navbar-link"> Personas en Espera</a></li>
                      </ul>
                </div>
            </div>
        </nav>
    </header>

<body>

       <form id="Form1" class="form-horizontal" role="form" runat="server">
       
        <div class="form-group space-vert">
            <span class="col-md-2 control-label"></span>
            <div class="col-md-8">
                <div class="form-group row">

                    <div class="col-md-12">
                        <div class="col-md-12">
                                <h4 class="videotit">
                                   TURNOS <span>ONLINE </span>
                                </h4>
                        <div id="linea-videos">
                        </div>
                        </div>
                    </div>
               
                </div>
              
            
 
                           
                <div class="form-group row">
                    <div class="col-md-1">
                    </div>
                    <div class="col-md-6">
                         <label for="inputValue">Ingrese el Código de Anulación</label>
                        <asp:TextBox ID="txtCodigoAnulacion" runat="server" Class="form-control"></asp:TextBox>
                    </div>                    
                    
                    </div>
                       <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="label3" runat="server" Text="Se ha anulado el siguiente turno:" Font-Bold="True"></asp:Label>
                    </div>
                           </div>   
                  <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="Label1" runat="server" Text="Trámite" Font-Bold="True"></asp:Label>
                    </div>
                           </div>    
                  <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno" Font-Bold="True"></asp:Label>
                    </div>
                           </div>    
                 <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblTipoTramite" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div>    
                  <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblFecha" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div>    
                 <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblNombre" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div>   
                 <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblTipoDoc" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div> 
                   <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblDocumento" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div> 
                  <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblmens" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div> 
                  <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblerror" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div> 
                <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-10">
                    <asp:Label ID="lblSobreTurno" Visible="false" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                           </div> 
               
                <div class="form-group row">
                    <div class="col-md-1">
                    
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="Button2" runat="server" Text="Volver" CssClass="btn btn-primary" Width="100%"></asp:Button>

                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="cmdAnulaTurno" runat="server" Text="Anular Turno" Width="100%" CssClass="btn btn-primary"></asp:Button>

                    </div>
                </div>
                   </div>
    
               </div>
               <!--Sing Up-->

         

           <div id="errorModal" class="modal" tabindex="-1" role="dialog" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h1 id="H1">Informacion</h1>
                            </div>
                            <div class="modal-body">
                                <asp:Label ID="lblMensajeError" Text="hubo un problema en el sistema, intente mas tarde." runat="server"></asp:Label>
                            </div>
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                            </div>

                        </div>
                    </div>
                </div>

            
    </form>



<div>
    
</div>

</body>
</html>

