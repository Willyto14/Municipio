<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Usuarios.aspx.vb" Inherits="Usuarios" %>


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


<body runat="server">
    <form id="LD" runat="server">

        <asp:HiddenField ID="HiddenField2" runat="server" />

        <asp:ScriptManager runat="server" ID="ScriptManager" EnableScriptGlobalization="true" />
        <div class="form-group space-vert">
            <div class="col-md-12">
                <div class="col-md-12">
                    <h4 class="videotit" style="font-size: 25px;">Usuarios
                    </h4>
                    <div id="linea-videos">
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="form-group row">
                </div>
                <div class="form-group row">
                    <div class="col-md-3">
                    </div>
                   
                     <div class="col-md-2">
                        <asp:TextBox ID="txtDniBUS" runat="server" CssClass="form-control" placeholder="DNI" />
                    </div>

                    
                     <div class="col-md-2">
                        <asp:TextBox ID="txtNombreBUS" runat="server" CssClass="form-control" placeholder="Nombre" />
                    </div>

                
                   




                </div>
                <div class="form-group row">
                    <div class="col-md-5">
                    </div>
                    <div class="col-md-5">
                        <asp:Button ID="btnAltaModal" runat="server" Text="Nuevo Usuario" OnClick="btnAltaModal_Click" CssClass="btn btn-info" />
                    </div>


                </div>

                <div class="form-group row">
                    <div class="col-md-12">
                        <asp:UpdatePanel ID="upCrudGrid" runat="server">

                            <ContentTemplate>


                                <asp:GridView ID="dgvUsuarios" Class="filtrar" runat="server" Width="940px"
                                    HorizontalAlign="Center"
                                    AllowSorting="True" ItemStyle-Width="40px"
                                    ItemStyle-HorizontalAlign="Center"
                                    DataKeyNames="CO_DNI"
                                    OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-hover table-striped">
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Center" CssClass="headerClass" />
                                    <Columns>
                                        <asp:ButtonField CommandName="detalleRecord" ItemStyle-Width="10%" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Detalle" HeaderText="Detalles">
                                            <ControlStyle CssClass="btn btn-info"></ControlStyle>
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>

                                        <asp:ButtonField CommandName="editarRecord" ItemStyle-Width="10%" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Editar" HeaderText="Editar">
                                            <ControlStyle CssClass="btn btn-success"></ControlStyle>
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>

                                        <asp:ButtonField CommandName="eliminarRecord" ItemStyle-Width="10%" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Baja\Alta" HeaderText="Eliminar" Visible="true">
                                            <ControlStyle CssClass="btn btn-danger"></ControlStyle>
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:ButtonField CommandName="turnoRecord" ItemStyle-Width="10%" ControlStyle-CssClass="btn btn-info" ButtonType="Button" Text="Turno" HeaderText="Turno" Visible="true">
                                            <ControlStyle CssClass="btn btn-info"></ControlStyle>
                                            <ItemStyle Width="10%" />
                                        </asp:ButtonField>
                                        <asp:BoundField DataField="CO_DNI" ReadOnly="True" HeaderText="DNI" SortExpression="CO_DNI"></asp:BoundField>
                                        <asp:BoundField DataField="CO_Nombre" ReadOnly="True" HeaderText="Nombre" SortExpression="CO_Nombre"></asp:BoundField>
                                        <asp:BoundField DataField="CO_BajaLogica" ReadOnly="True" HeaderText="Baja Logica" SortExpression="CO_BajaLogica"></asp:BoundField>
                                        <asp:BoundField DataField="Usr_EsAdministrador" ReadOnly="True" HeaderText="Administrador" SortExpression="Usr_EsAdministrador"></asp:BoundField>
                                        <asp:BoundField DataField="CO_SobreTurno" ReadOnly="True" HeaderText="Sobreturnos" SortExpression="CO_SobreTurno"></asp:BoundField>



                                    </Columns>
                                </asp:GridView>


                            </ContentTemplate>

                            <Triggers>

                                 <asp:AsyncPostBackTrigger ControlID="txtDniBUS" EventName="TextChanged" />
                                 <asp:AsyncPostBackTrigger ControlID="txtNombreBUS" EventName="TextChanged" />
                              
                                


                            </Triggers>

                        </asp:UpdatePanel>
                    </div>
                    <asp:Label ID="lblID" runat="server" Visible="false" Text=""></asp:Label>
                </div>
            </div>
        </div>




        <div style="width: 90%; margin-right: 5%; margin-left: 5%; text-align: center">


            <br />
            <br />


            <div class="row row-filtros">
            </div>
            <hr />

       
              <div id="errorModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="errorModalLabel">Borrar Usuario</h3>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:Label ID="lblMensajeErrorModal" runat="server" Text="Label"></asp:Label>
                                    <asp:HiddenField ID="HiddenField4" runat="server" />
                                </div>
                                <div class="modal-footer">
                                
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                                </div>
                            </ContentTemplate>
                     
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div id="detalleModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>

                            <h3 id="myModalLabel">Detalle</h3>

                        </div>

                        <div class="modal-body">

                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">

                                <ContentTemplate>

                                    <asp:DetailsView ID="DetailsView1" runat="server" CssClass="table table-bordered table-hover" BackColor="White" ForeColor="Black" FieldHeaderStyle-Wrap="false" FieldHeaderStyle-Font-Bold="true" FieldHeaderStyle-BackColor="LavenderBlush" FieldHeaderStyle-ForeColor="Black" BorderStyle="Groove" AutoGenerateRows="False">

                                        <Fields>


                                      


                                        <asp:BoundField DataField="CO_DNI"  HeaderText="DNI" ></asp:BoundField>
                                        <asp:BoundField DataField="CO_Nombre"  HeaderText="Nombre"></asp:BoundField>
                                        <asp:BoundField DataField="CO_BajaLogica"  HeaderText="Baja Logica" ></asp:BoundField>
                                        <asp:BoundField DataField="Usr_EsAdministrador"  HeaderText="Administrador" ></asp:BoundField>
                                        <asp:BoundField DataField="CO_SobreTurno" HeaderText="Sobreturnos"></asp:BoundField>

                                        </Fields>

                                    </asp:DetailsView>

                                </ContentTemplate>

                                <Triggers>

                                    <asp:AsyncPostBackTrigger ControlID="dgvUsuarios" EventName="RowCommand" />


                                </Triggers>

                            </asp:UpdatePanel>

                            <div class="modal-footer">

                                <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>

                            </div>

                        </div>

                    </div>

                </div>
            </div>

            <div id="altaModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="addModalLabel">Nuevo Usuario</h3>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    <table class="table table-bordered table-hover">
                                         <tr>
                                            <td>Dni</td>
                                            <td>
                                                <asp:TextBox ID="txtDniINS" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" CssClass="form-control" placeholder="DNI" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nombres</td>
                                            <td>
                                                <asp:TextBox ID="txtNombreINS" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" CssClass="form-control" placeholder="Nombre" />
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td>Contraseña</td>
                                            <td>
                                                <asp:TextBox ID="txtContraseñaINS" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" TextMode="Password" runat="server" CssClass="form-control" placeholder="Contraseña" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Reitere Contraseña</td>
                                            <td>
                                                <asp:TextBox ID="txtContraseñaRepetirINS" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" TextMode="Password" runat="server" CssClass="form-control" placeholder="Ritere Contraseña" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Administrador</td>
                                            <td>
                                                <asp:CheckBox ID="chkAdministradorINS" CssClass="form-control" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" />
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>Sobre Turno</td>
                                            <td>
                                                <asp:CheckBox ID="chkCChkSobreTurnoINS" CssClass="form-control" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblErrorInsertar" CssClass="danger" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="modal-footer">

                                    <asp:LinkButton ID="btnAltaAceptarModal" runat="server" Text="Aceptar" CssClass="btn btn-info" ValidateRequestMode="Enabled" />
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                                </div>

                            </ContentTemplate>

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAltaModal" EventName="Click" />

                            </Triggers>






                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>

            <div id="editarModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="updModalLabel">Editar Usuario</h3>
                        </div>

                        <asp:UpdatePanel ID="upAdd" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HfUpdateID" runat="server" />
                                    <table class="table table-bordered table-hover">
                                       
                                        <tr>
                                            <td>Nombre</td>
                                            <td>
                                            <asp:TextBox ID="txtNombreUPD" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" CssClass="form-control" placeholder="Nombre" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Administrador</td>
                                            <td>
                                                <asp:CheckBox ID="chkAdministradorUPD" CssClass="form-control" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sobre Turno</td>
                                            <td>
                                                <asp:CheckBox ID="chkCChkSobreTurnoUPD" CssClass="form-control" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Label ID="lblErrorModificar" CssClass="danger" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>

                                </div>
                                <div class="modal-footer">

                                    <asp:LinkButton ID="btnModificarAceptarModal" runat="server" Text="Modificar" CssClass="btn btn-info" ValidateRequestMode="Enabled" />
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                                </div>

                            </ContentTemplate>


                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>

            <div id="eliminarModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="delModalLabel">Borrar Usuario</h3>
                        </div>
                        <asp:UpdatePanel ID="upDel" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:Label ID="lblMensajeBorrar" runat="server" Text="Label"></asp:Label>
                                    <asp:HiddenField ID="HfDeleteID" runat="server" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnEliminarAceptarModal" runat="server" Text="Aceptar" CssClass="btn btn-info" />
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnEliminarAceptarModal" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div id="turnoModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" style="position: fixed; margin-top: 0px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h3 id="tuModalLabel">Turnos Asignados</h3>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HiddenField3" runat="server" />

                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                                        <ContentTemplate>


                                            <asp:GridView ID="gvMostrarTurno" Class="filtrar" runat="server"
                                                HorizontalAlign="Center"
                                                AllowSorting="True"
                                                ItemStyle-HorizontalAlign="Center"
                                                DataKeyNames="TTI_ID"
                                                AutoGenerateColumns="False" CssClass="table table-hover table-striped" PageSize="1">
                                                <HeaderStyle Font-Bold="True" HorizontalAlign="Center" CssClass="headerClass" />
                                                <Columns>

                                                    <asp:TemplateField HeaderText="Permiso">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkPermisoHabilitado" Text="SI" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="TTI_ID" ReadOnly="True" HeaderText="ID" SortExpression="TTI_ID"></asp:BoundField>
                                                    <asp:BoundField DataField="TTI_Descripcion" ReadOnly="True" HeaderText="Turno" SortExpression="TTI_Descripcion"></asp:BoundField>


                                                </Columns>
                                            </asp:GridView>


                                        </ContentTemplate>



                                    </asp:UpdatePanel>




                                </div>
                                <div class="modal-footer">

                                    <asp:LinkButton ID="btnTurnoAceptarModal" runat="server" Text="Aceptar" CssClass="btn btn-info" ValidateRequestMode="Enabled" />
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                                </div>

                            </ContentTemplate>

                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnTurnoAceptarModal" EventName="Click" />

                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
