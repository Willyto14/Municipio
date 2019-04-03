<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PersonasenEspera.aspx.vb" Inherits="PersonasenEspera" %>

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

                        
                        <asp:Dropdownlist ID="ddlTramite" placeholder="Tramite"  runat="server" CssClass="form-control" AutoPostBack="True" Visible="False"></asp:Dropdownlist>
             
                
                 <div class="form-group row">
                     <div class="col-md-5" >
                        <label for="inputValue" style="font-size:medium; font-family:Verdana">Lugar</label>
                        <asp:Dropdownlist ID="ddlLugar" placeholder="Lugar"  runat="server" CssClass="form-control" AutoPostBack="True"></asp:Dropdownlist>
                        <asp:CheckBox id="chkTodosL" CssClass="checkbox-inline"   runat="server"  Text="Todos"  Font-Bold="True" AutoPostBack="True" ></asp:CheckBox>
                     </div>
                          
                     <div class="col-md-5">
                        <label for="inputValue" style="font-size:medium; font-family:Verdana">Sector</label>
                        <asp:Dropdownlist ID="ddlSector" placeholder="Sector"  runat="server" CssClass="form-control" AutoPostBack="True" ></asp:Dropdownlist>
                        <asp:CheckBox id="chkTodosS"  CssClass="checkbox-inline"   runat="server"  Text="Todos"  Font-Bold="True" AutoPostBack="True" ></asp:CheckBox>
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

                 <div>
                     
                     <asp:GridView ID="grTotales" runat="server" AutoGenerateColumns="False" GridLines="None"  CellPadding="20" CellSpacing="1" style="border-radius:4px;"
                            AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" PageSize="100" >
                        <Columns>
                            <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                            <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" SortExpression="Cantidad" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Derivados" HeaderText="Derivados" SortExpression="Derivados" ItemStyle-HorizontalAlign="Center" />        
                        </Columns>
                     
                    </asp:GridView>

                </div>

                <br />
                <br />
                <div class="form-group row">
                    <div class="col-md-4">
                        <asp:Button Width="140px" ID="btnDetalle" runat="server" Text="Detalle" CssClass="btn btn-primary" ValidateRequestMode="Enabled" Visible="False" />
                    </div>
                </div>



                 <div id="detalleModal" class="modal" tabindex="-1" role="dialog" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h1 id="H1">Detalle</h1>
                        </div>
                        <div class="modal-body">
                <div>

                   
                    <asp:GridView ID="GRTurnos" runat="server" AutoGenerateColumns="False" GridLines="None"  CellPadding="20" CellSpacing="1"
                            AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" PageSize="30" >

                        
                        <Columns>
                            <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                            <asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
                            <asp:BoundField DataField="Sector" HeaderText="Sector" SortExpression="Sector" />
                            <asp:BoundField DataField="Letra" HeaderText="Letra" SortExpression="Letra" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Numero" HeaderText="Numero" SortExpression="Numero" ItemStyle-HorizontalAlign="Center"/>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />                
                        </Columns>
                              <%--  <RowStyle BackColor="#F7F6F3" Font-Size="Medium" ForeColor="#333333" Width="100%" Height="50px" />
                                <FooterStyle BackColor="#050620" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#0066A7" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#E2DED6" BorderColor="LightSteelBlue" Font-Bold="True"
                                    ForeColor="#333333" />
                                <HeaderStyle BackColor="#0066A7" Font-Bold="True" ForeColor="White" HorizontalAlign="left" Font-Names="Verdana" Height="50px" />
                                <EditRowStyle BackColor="#660033" HorizontalAlign="Center" Font-Names="Verdana" />
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <AlternatingRowStyle Wrap="True" />--%>
                    </asp:GridView>
          
                    
          

                <!--Sing Up-->
            </div>
                         
                        </div>
                            <div class="modal-footer">
                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                            </div>
                       
                    </div>
                </div>
            </div>






       
                
                
                
                
                 </div>
            </div>
                  </div>
        </div>

        <div id="mensajeModal" runat="server"  class="modal fade " tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="Cierramensaje"  cssclass="close" runat="server" Text="x" /> 
                        <p >Información</p>
                    </div>
                    <div class="modal-body">
                        <p class="parrafo-error">
                            <asp:Label ID="lblmensaje" runat="server" Text="Label"></asp:Label></p>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrar" CssClass="btn btn-primary" runat="server"  Text="Cerrar" />
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

