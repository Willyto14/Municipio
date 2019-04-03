<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PersonasAtendidas.aspx.vb" Inherits=" PersonasAtendidas" %>

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

    <form id="Form1" class="form-horizontal" role="form" runat="server">


        <div class="form-group space-vert">
            <span class="col-md-2 control-label"></span>
            <div class="col-md-8">

                <div class="form-group row">

                    <div class="col-md-12">
                        <div class="col-md-12">
                            <h4 class="videotit">Búsqueda de Personas Atendidas
                            </h4>
                            <div id="linea-videos">
                            </div>
                        </div>
                    </div>

                </div>
                
                <asp:DropDownList ID="ddlTramite" placeholder="Tramite" runat="server" Class="form-control" AutoPostBack="True" Visible="False"></asp:DropDownList>

                <div class="form-group row">
                    <div class="col-md-4">
                        <label for="inputValue" visible="False">Fecha Desde (opcional):</label>
                        <asp:TextBox ID="txtFechaDesde" placeholder="Fecha" runat="server" Class="form-control" ValidationGroup="Validar" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label for="inputValue" visible="False">Fecha Hasta (opcional):</label>
                        <asp:TextBox ID="txtFechaHasta" placeholder="Fecha" runat="server" Class="form-control" ValidationGroup="Validar" TextMode="Date"></asp:TextBox>

                    </div>
                    <div class="col-md-4">
                        <label for="inputValue" style="font-size: medium; font-family: Verdana">Lugar:</label>
                        <asp:DropDownList ID="ddlLugar" runat="server" Class="form-control" AutoPostBack="True" />
                    </div>
                </div>
                
                <div class="form-group row">
                    <div class="col-md-4">
                        <label for="inputValue" visible="False">Búsqueda por documento (Opcional):</label>
                        <asp:TextBox ID="txtDocumento" placeholder="Ingrese sólo números" runat="server" Class="form-control" OnKeyPress="return SoloNumeros(event)"></asp:TextBox>
                    </div>
                    <div class="col-md-4">

                        <asp:RadioButtonList ID="rdLug" runat="server" Font-Bold="true" AutoPostBack="true">
                            <asp:ListItem Text="Lugar" Value="0" Selected="True">
                            </asp:ListItem>
                            <asp:ListItem Text="Lugar y Sector" Value="1">
                            </asp:ListItem>
                            <asp:ListItem Text="Lugar, Sector y Tipo número" Value="2">
                            </asp:ListItem>

                        </asp:RadioButtonList>

                    </div>
                    <div class="col-md-4">
                        <asp:Label runat="server" ID="lblSecDrop" Text="Sector:" for="inputValue" Visible="false" Style="font-size: medium; font-family: Verdana" Font-Bold="true" />
                        <asp:DropDownList ID="ddlSector" runat="server" Class="form-control" AutoPostBack="True" Visible="false" />
                    </div>
                </div>
                
                <div class="form-group row">

                    <div class="col-md-4">
                        <asp:Label runat="server" ID="lblTNUCod" Text="Código:" for="inputValue" Style="font-size: medium; font-family: Verdana" Font-Bold="true" Visible="false" />
                        <asp:DropDownList ID="ddlCod" runat="server" Class="form-control" Visible="false" />
                    </div>

                </div>
                
                <div class="form-group row">
                    <div class="col-md-4">
                        <asp:Button Width="140px" ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" ValidateRequestMode="Enabled" />
                    </div>
                    <div class="col-md-4">
                        <asp:Button Width="140px" ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-primary" ValidateRequestMode="Enabled" Visible="False" />
                    </div>
                  
                </div>
       
                        
               



        </div>

            <div id="gvNumeroModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <asp:Button ID="Button3" CssClass="close" runat="server" Text="x" />
                            <p>Información</p>
                        </div>
                        <div class="modal-body">
                            <asp:GridView ID="gvNumero" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                    <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                    <asp:BoundField DataField="Sector" HeaderText="Lugar" SortExpression="Lugar" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="Numero" HeaderText="Numero" SortExpression="Numero" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#gvNumero').footable();
                                });
                            </script>
                            <div class="modal-footer">
                                <asp:Button ID="Button4" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>

                <div id="gvSinDNILugModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="Button1" CssClass="close" runat="server" Text="x" />
                                <p>Información</p>
                            </div>
                            <div class="modal-body">
                              
                                
                                    <asp:GridView ID="gvSinDNILug" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                       <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#gvSinDNILug').footable();
                                });
                            </script>
              
                                <div class="modal-footer">
             
                                     <asp:ImageButton ID="btnExcelSinDNILug" runat="server"   Style=" width: 40px;" ImageUrl="~/Imagenes/excel-icon.jpg" />
                                     <asp:Button ID="Button10" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                                     <asp:Label ID="lblExcelSinDNILug" runat="server" Visible="false" Text="Label"></asp:Label>
                                      </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="gvConDNILugModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="Button6" CssClass="close" runat="server" Text="x" />
                                <p>Información</p>
                            </div>
                            <div class="modal-body">

                                      <asp:GridView ID="gvConDNILug" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                           <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DNI" HeaderText="DNI" SortExpression="DNI" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#gvConDNILug').footable();
                                });
                            </script>

                                <div class="modal-footer">
                                            <asp:ImageButton ID="btnExcelConDNILug" runat="server"   Style=" width: 40px;" ImageUrl="~/Imagenes/excel-icon.jpg" />
                                    <asp:Button ID="Button7" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                               <asp:Label ID="lblExcelConDNILug" runat="server" Visible="false" Text="Label"></asp:Label>
                                   
                                     </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div id="gvTotalesModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="Button8" CssClass="close" runat="server" Text="x" />
                                <p>Información</p>
                            </div>
                            <div class="modal-body">

      <asp:GridView ID="gvTotales" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                           <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                        <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" NullDisplayText="0" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DNI" HeaderText="DNI" SortExpression="DNI" NullDisplayText="0" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#gvTotales').footable();
                                });
                            </script>

                                <div class="modal-footer">
                                      <asp:ImageButton ID="btnExcelTotales" runat="server"   Style=" width: 40px;" ImageUrl="~/Imagenes/excel-icon.jpg" />
                                    <asp:Button ID="Button9" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                                <asp:Label ID="lblExcelTotales" runat="server" Visible="false" Text="Label"></asp:Label>
                                                                     </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div id="gvSecSinDNIModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:Button ID="Button5" CssClass="close" runat="server" Text="x" />
                                <p>Información</p>
                            </div>
                            <div class="modal-body">

      <asp:GridView ID="gvSecSinDNI" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                      <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                                        <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" NullDisplayText="0" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#gvSecSinDNI').footable();
                                });
                            </script>

                                <div class="modal-footer">
                                          <asp:ImageButton ID="btnExcelSecSinDNI" runat="server"   Style=" width: 40px;" ImageUrl="~/Imagenes/excel-icon.jpg" />
                                    <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                                <asp:Label ID="lblExcelSecSinDNI" runat="server" Visible="false" Text="Label"></asp:Label>
                                      </div>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="form-group row">
                    <div class="col-md-4">
                        <asp:Button Width="140px" ID="btnDetalle" runat="server" Text="+Detalle" CssClass="btn btn-primary" ValidateRequestMode="Enabled" Visible="False" />
                    </div>
                </div>

                <div>

                    <br />



                    <asp:GridView ID="GvTurnos" CssClass="footable" runat="server" Style="margin-bottom: 4px;">
                                <Columns>
                                  <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                            <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" />
                            <asp:BoundField DataField="Letra" HeaderText="Letra" SortExpression="Letra" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Numero" HeaderText="Numero" SortExpression="Numero" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
                                </Columns>
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>

                            <link href="css/footable.min.css"
                                rel="stylesheet" type="text/css" />
                            <script type="text/javascript" src="js/footable.min.js"></script>
                            <script type="text/javascript">
                                $(function () {
                                    $('#GvTurnos').footable();
                                });
                            </script>

        
                    <!--Sing Up-->
                </div>

            </div>
      

        <div style="width: 90%; margin-right: 5%; margin-left: 5%; text-align: center">


            <br />
            <br />


            <div class="row row-filtros">
            </div>
            <hr />


        </div>

        <div id="mensajeModal" runat="server" class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="Cierramensaje" CssClass="close" runat="server" Text="x" />
                        <p>Información</p>
                    </div>
                    <div class="modal-body">
                        <p class="parrafo-error">
                            <asp:Label ID="lblmensaje" runat="server" Text="Label"></asp:Label>
                        </p>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrar" CssClass="btn btn-primary" runat="server" Text="Cerrar" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

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

