<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EntregaTurnos.aspx.vb" Inherits="EntregaTurnos" %>

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
     <script src="js/PonerFechaDeHoy.js" type="text/javascript"></script>
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


     <script type="text/javascript">
         function ocultar() {
             document.getElementById('cmdConfirmaTurno1').style.display = 'none';
         }
</script>
</head>
    <header>        
      <nav class="navbar navbar-top navbar-default" role="navigation" style="background-color: aliceblue;">
            <div class="container"  >
                <div class="navbar-header" >
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#main-navbar-collapse">
                        <span class="sr-only">Toggle navigation</span>
                        <span class="icon-bar" ></span>
                    </button>
                </div>
          
                <div class="collapse navbar-collapse" id="main-navbar-collapse" style="background-color: aliceblue;"  >
                    
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
                        <li><a href="ConsultaTurnos.aspx" class="navbar-link">Turnos</a></li>
                      </ul>
                       <ul class="nav navbar-nav navbar-right">
                        <li><a href="PersonasAtendidas.aspx" class="navbar-link">Personas Atendidas</a></li>
                      </ul>
                     <ul class="nav navbar-nav navbar-right">
                        <li><a href="PersonasenEspera.aspx" class="navbar-link">Personas en Espera</a></li>
                      </ul>
                   <ul class="nav navbar-nav navbar-right">
                        <li><a href="AnulacionXCodigo.aspx" class="navbar-link">Anulacion</a></li>
                      </ul>
                </div>
            </div>
        </nav>
    </header>
       
<body>

    <form id="Form1" class="form-horizontal" role="form" runat="server" enctype="multipart/form-data">
       
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
                    <div class="col-md-5">
                         
                        <asp:Label ID="Leyendainicio" runat="server" Text="Turnos disponibles a partir de la Fecha" Font-Bold="True"></asp:Label>
                        <br />
                        <asp:Label ID="lblFechaInicio" runat="server" Text="lblFechaInicio"></asp:Label>
                        <br />
                        <asp:Label ID="LeyendaFin" runat="server" Text="hasta la Fecha" Font-Bold="True"></asp:Label>
                        <br />
                        <asp:Label ID="lblFecTopEN" runat="server" Text="lblFecTopEN"></asp:Label>
                          <br />
                        <br />
                      <label for="inputValue" runat ="server" id ="lblSeleccion">Seleccione el Tipo de Trámite</label>
                        <asp:DropDownList ID="ddlSubTipoNumero"   runat="server" Class="form-control" AutoPostBack="True"></asp:DropDownList>
                        <br />

                            <asp:HyperLink ID="HPLDocumentacion" target="_blank" runat="server">Ver Documentación</asp:HyperLink><br />
                    <asp:Label ID="txtdetalletramite" runat="server" Text="Label" Font-Bold="True" ForeColor="Red" Font-Size="Smaller"></asp:Label>
  
                    </div>
              
                    <div class="col-md-6">

                         <label for="inputValue">Seleccione la Fecha</label>
                        
                          <asp:Calendar ID="calFechaTurno" runat="server" BackColor="White" BorderColor="Transparent" CellPadding="10" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="205px" Width="269px">
                                <SelectedDayStyle BackColor="LightGray" Font-Bold="True" ForeColor="White" BorderStyle="Solid" />
                                <TodayDayStyle BackColor="Menu" ForeColor="Black" />
                                <SelectorStyle BackColor="LightSteelBlue" />
                                <WeekendDayStyle BackColor="LightSlateGray" />
                                <OtherMonthDayStyle Font-Size="0pt" ForeColor="Black" />
                                <NextPrevStyle VerticalAlign="Bottom" />
                                <DayHeaderStyle BackColor="SteelBlue" Font-Bold="True" Font-Size="7pt" />
                                <TitleStyle BackColor="DarkGray" BorderColor="Black" Font-Bold="True" />
                                <DayStyle BackColor="LightGray" />
                            </asp:Calendar>
                     <asp:Label ID="lblFechaSeleccionada" runat="server" Text="Label"></asp:Label>
                    </div>

                </div>
                <div class="form-group row">

                    <div class="col-md-1">
                            
                        <asp:TextBox ID="txtDoc0800" runat="server" Class="form-control" Visible="False"></asp:TextBox><br />
                    </div>
                    <div class="col-md-3">
                     <label for="inputValue">Seleccione el horario</label>
                        <asp:DropDownList ID="ddlturnosDisponibles" Width="100%" runat="server" Class="form-control"></asp:DropDownList>

                    </div>
                      <div class="col-md-7">
                        <label for="inputValue">Apellido(s) y Nombre(s)</label>
                        <asp:TextBox ID="txtApNom" runat="server" Class="form-control"></asp:TextBox>
              </div>
                   
                </div>
                <div class="form-group row">

                    <div class="col-md-1">
                    </div>
                    <div class="col-md-3">
                        <label for="inputValue">Tipo de Documento</label>
                        <asp:DropDownList Width="100%" ID="ddlTipoDoc" runat="server" Class="form-control"></asp:DropDownList>
                        <asp:Label ID="lblmensaje" runat="server" Text="Seleccione el horario" Visible="False" Font-Bold="True"></asp:Label>
                    </div>
                    <div class="col-md-3">
                           <label for="inputValue">Numero</label>
                        <input id="txtDocumento" runat="server" onkeyup="var no_digito = /\D/g; 
                          this.value = this.value.replace(no_digito , '');"
                            type="text" class="form-control" name="" maxlength="8"  />
                    </div>
                      <div class="col-md-4">
                         <label for="inputValue">Teléfono de Contacto</label>
                        <asp:TextBox ID="txtTelContacto" runat="server" Class="form-control"></asp:TextBox>
               
                    </div>
                     
                             </div>
                <div class="form-group row">
                    <div class="col-md-1">
                    </div>
                  
                     <div class="col-md-5">
                             <label for="inputValue">Complete su E-Mail</label>
                        <asp:TextBox ID="txtmail" runat="server" Class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-5">
                        <label for="inputValue">Reitere su mail</label>
                        <asp:TextBox ID="txtrepitemail" runat="server" Class="form-control"></asp:TextBox>
                      
                    </div>
                    </div>
                    <div class="form-group row">
                    <div class="col-md-1">
                    </div>
                    <div class="col-md-8">
                        <br />
                        <asp:Label ID="lblerror" runat="server" Text="Label"></asp:Label>
                        <asp:Label ID="lblmens" runat="server" Text="Label"></asp:Label>
                      </div>
                    </div>
                <div class="form-group row">

                    <div class="col-md-1">
                        <asp:Button ID="Button1" runat="server" Text="Button" Visible="False" />
                    </div>
                 
                </div>

             

                <div class="form-group row">
                    <div class="col-md-3">
                        <asp:Label ID="lblLugar" runat="server" Text="Label" Visible="False"></asp:Label>
                        <asp:Label ID="lblTipoNum" runat="server" Text="Label" Visible="False"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-primary" Width="130px"></asp:Button>

                    </div>
                    <div class="col-md-3">
                           <input id="cmdConfirmaTurno1"  runat="server" type="button" value="Confirmar Turno" onclick="javascript: ocultar();"  class="btn btn-primary" />
                        

                    </div>
                </div>
                <div class="col-md-5">
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


</body>

</html>


