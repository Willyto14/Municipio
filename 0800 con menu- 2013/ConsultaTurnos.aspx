    <%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConsultaTurnos.aspx.vb" Inherits="ConsultaTurnos" %>

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
            <script src="js/PonerFechaDeHoy.js" type="text/javascript"></script>
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
      <form id="Form1" class="form-horizontal" role="form" runat="server" enctype="multipart/form-data">
                                                      

            <asp:HiddenField ID="HiddenField2" runat="server" />

            <asp:ScriptManager runat="server" ID="ScriptManager" EnableScriptGlobalization="true" />
            <div class="form-group space-vert">
                <span class="col-md-2 control-label"></span>
                <div class="col-md-8">
                    <div class="form-group row">

                        <div class="col-md-12">
                            <div class="col-md-12">
                                    <h4 class="videotit">
                                       CONSULTA <span>TURNOS </span>
                                    </h4>
                            <div id="linea-videos">
                            </div>
                            </div>
                        </div>
               
                    </div>
    
        </div>
                </div>
            
        <div class="form-group space-vert">
 
            <div class="col-md-12" ">
           
                <div class="form-group row">
                </div>
                  
                       <div class="form-group row">
                       <div class="col-md-2">
                  
                        </div>
                        <div class="col-md-5">
                     
                           <asp:Label ID="Label1" runat="server" Text=" Turnos Cargados a partir de la Fecha"></asp:Label>
                            <asp:Label ID="lblFechaInicio" runat="server" Text="Fecha Hora"></asp:Label>
                           </div>
                          <div class="col-md-5">
                        
                           <asp:Label ID="Label2" runat="server" Text=" Turnos Cargados a partir de la Fecha"></asp:Label>
                            <asp:Label ID="lblFecTopEN" runat="server" Text="Hasta Fecha Hora"></asp:Label>
                           </div>
                           </div>
               
                    <div class="form-group row">
                       <div class="col-md-2">
                  
                        </div>
                        <div class="col-md-2">
                            <label for="inputValue">Fecha Desde</label>
                            <asp:TextBox ID="txtFechaD" runat="server" CssClass="form-control" Height="50px"  Font-Size="15px" placeholder="Fecha Desde" TextMode="Date" min="1" ReadOnly="false" />
                        </div>
                        <div class="col-md-2">
                            <label for="inputValue">Fecha Hasta</label>
                               <asp:TextBox ID="txtFechaH" runat="server" CssClass="form-control" Height="50px"  Font-Size="15px" placeholder="Fecha Hasta" TextMode="Date" min="1" ReadOnly="false" />
                        </div>
                   
      
                  
                  
         
                <div class="col-md-2">
                            <label for="inputValue">Dni</label>
                               <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" Height="50px"  Font-Size="15px" placeholder="Dni" TextMode="Number" min="1" ReadOnly="false" />
                        </div>
                                  <div class="col-md-2">
                            <label for="inputValue">Seleccione el trámite</label>
                                 <asp:DropDownList ID="ddlTipoTramite" runat="server" Height="50px"   Font-Size="15px"  Class="form-control" ></asp:DropDownList>
                       </div>
    </div>

                    <div class="form-group row">
                         <div class="col-md-3">
                  <div class="col-md-10">
                    <asp:Label ID="lblSobreTurno" Visible="false" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                        </div> 
                        <div class="col-md-3">
                            
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Width="100%" CssClass="btn btn-primary" ValidateRequestMode="Enabled" />
                           
                        </div>
                            <div class="col-md-3">
                            <asp:Button ID="btnExportar" runat="server" Width="100%" Text="Exportar" CssClass="btn btn-primary"  ></asp:Button>

                        </div>
                     
                    </div>
          
                <div class="form-group row">
                       <div class="col-md-1">
                  
                        </div>
                        <div class="col-md-3">
                               <asp:Label ID="lblerror" runat="server"></asp:Label>
                         </div>
                     <div class="col-md-3">
                               <asp:Label ID="lblPregunta" runat="server"></asp:Label>
                         </div>
                     <div class="col-md-3">
                               <asp:Label ID="lblmens" runat="server"></asp:Label>
                         </div>
                            </div>
                     
             

                    <div class="form-group row">
                        <div class="col-md-3">
                            <asp:Label ID="lblLugar" runat="server" Text="Label" Visible="False"></asp:Label>
                            <asp:Label ID="lblTipoNum" runat="server" Text="Label" Visible="False"></asp:Label>
                        </div>
                        <div class="col-md-3">
                            <asp:Button ID="btnVolver" runat="server" Text="Volver" CssClass="btn btn-primary" Width="130px" Visible="False"></asp:Button>

                        </div>
                             
                        <div class="col-md-3">
                               <input id="cmdConfirmaTurno1"  runat="server" type="button" value="Confirmar Turno" onclick="javascript: ocultar();"  class="btn btn-primary" visible="False" />
                        

                        </div>
                    </div>
      



                      <div class="form-group row">
                       
                        <div class="col-md-12">
                        
                            <script type="text/javascript">
                                $(function () { $('#Grilla').footable(); });

                            </script>
                     

                            


                                    <asp:GridView ID="GRTurnos" Class="filtrar" runat="server" Width="500px" 
                                        HorizontalAlign="Center"
                                        AllowSorting="True" 
                                        ItemStyle-HorizontalAlign="Center"
                                        DataKeyNames="Origen"
                                        OnRowCommand="GRTurnos_RowCommand" AutoGenerateColumns="False" AllowPaging="True" CssClass="table table-hover table-striped" PageSize="50" >
                                        <HeaderStyle Font-Bold="True" HorizontalAlign="Center" CssClass="headerClass" />
                                        <Columns>
                                   

                                       

                              
                                      
                                <asp:BoundField DataField="Origen" HeaderText="Origen" SortExpression="Origen" />
                                <asp:BoundField DataField="Tr&#225;mite" HeaderText="Tr&#225;mite" SortExpression="Tr&#225;mite" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" />
                                <asp:BoundField DataField="Hora" HeaderText="Hora" SortExpression="Hora" />
                                <asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
                                <asp:BoundField DataField="Mail" HeaderText="Mail" SortExpression="Mail" />
                                <asp:BoundField DataField="Ap.Nom" HeaderText="Ap.Nom" SortExpression="Ap.Nom" />
                                <asp:BoundField DataField="Tel.Contacto" HeaderText="Tel.Contacto" SortExpression="Tel.Contacto" />
                                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                                <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                                <asp:BoundField DataField="Expedido por" HeaderText="Expedido por" SortExpression="Expedido por" />
                                <asp:BoundField DataField="Confirmado" HeaderText="Confirmado" SortExpression="Confirmado" Visible="False" />
                                <asp:BoundField DataField="Sobre Turno" HeaderText="Sobre Turno" SortExpression="Sobre Turno" />         
                                     <asp:TemplateField HeaderText="Confirmar" Visible="False">
                                      <ItemTemplate>
                                        <asp:ImageButton ID="imgestado" runat="server"  
                                          CommandName="CF" 
                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                          Text="Confirmar Turno"  />
                                      </ItemTemplate> 
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>                            
                                    <asp:TemplateField HeaderText="Eliminar">
                                      <ItemTemplate>
                                        <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/Imagenes/cancelar.jpg"  
                                          CommandName="ET" 
                                          CommandArgument="<%# CType(Container,GridViewRow).RowIndex %>"
                                          Text="Eliminar Turno"  />
                                      </ItemTemplate> 
                                     </asp:TemplateField>                            
                                


                                        </Columns>
                                    </asp:GridView>
                                      <asp:Button ID="btnConfirmar" runat="server" Text="Button" Height="0px" Width="0px" Visible="False" />
                         <asp:Button ID="cmdEliminar" runat="server" CausesValidation="False" EnableViewState="False"
                            Font-Bold="True" ForeColor="Gray" Text="Eliminar" Width="100px" style="left: 200px; position: absolute; top: -170px" Visible="False" />
                                                

                      
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

      
      </div>
      <div id="confirmacionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <asp:Label ID="Label5" runat="server" Text="Confirmación" style="font-size: 25px;"></asp:Label>

                        </div>
                        <asp:UpdatePanel ID="upEdit" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HfUpdateID" runat="server" />
                                    <table class="table">
                                        <tr>

                                            <td><asp:Label ID="lblMensajeConfirmacion" runat="server" Text="Label"></asp:Label></td>
                                          </tr>
                               
                                    </table>
                                </div>
                            
                                <div class="modal-footer">
                                    <asp:Button ID="btnAceptrarEliminar" runat="server" Text="Aceptar" CssClass="btn btn-primary" ValidateRequestMode="Enabled" />
                                    <button class="btn btn-primary" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                                </div>
                            </ContentTemplate>

                        </asp:UpdatePanel>

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


            <div id="imprimirModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <asp:Label ID="Label3" runat="server" Text="Exportar" style="font-size: 25px;"></asp:Label>

                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                              
                                        <div class="modal-body">
                                             
                             
                                                                    
                                            <asp:ImageButton ID="btnPdf" runat="server"   Style=" width: 120px;"  ImageUrl="~/Imagenes/pdf.png" />
                                            <asp:ImageButton ID="btnExcel" runat="server"   Style=" width: 110px;" ImageUrl="~/Imagenes/excel-icon.jpg" />
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="Button1" runat="server" Text="Aceptar" CssClass="btn btn-primary" ValidateRequestMode="Enabled" />
                                
                                </div>
                            </ContentTemplate>

                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>

        <div id="pdfModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" style="position: fixed; margin-top: 1px; font-size: 10px;" aria-hidden="true">
                    <div class="modal-dialog" style="width: 530px;">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                           <asp:Label ID="Label6" runat="server" Text="PDF" style="font-size: 25px;"></asp:Label>
                            </div>
                         
                            <div class="modal-header">



                                <div id="dialog" style="z-index: 1050; width: auto; min-height: 0px; height: 500px;" class="ui-dialog-content ui-widget-content">
                                     <object   id="htmlPdf" visible="true" data="<% =htmlPdf%>" type="application/pdf" width="500px" height="500px" internalinstanceid="6">If you are unable to view file, you can download from <a href="  <% =htmlPdf%>">here</a> or download <a target="_blank" href="http://get.adobe.com/reader/">Adobe PDF Reader</a> to view the file.</object>
                                                                                
                                </div>
                            </div>

                  
                    </div>
                </div>
  
             </div>
           
            <div id="excelModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" style="position: fixed; margin-top: 100px;" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <asp:Label ID="Label4" runat="server" Text="Excel" style="font-size: 25px;"></asp:Label>
                        </div>

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="modal-body">
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    <table class="table table-bordered table-hover">
                                        <tr>
                                            <td>Desde</td>
                                            <td>
                                                <asp:TextBox ID="txtFechaDExpo" Enabled="false" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" CssClass="form-control" placeholder="Nombre" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Hasta</td>
                                            <td>
                                                <asp:TextBox ID="txtFechaHExpo" Enabled="false" Width="100%" padding=" 14px 15px" Font-Size="16px" Height="30px" runat="server" CssClass="form-control" placeholder="Apellido" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Edtado</td>
                                            <td>
                                                <asp:RadioButtonList ID="rdbTurnos" runat="server">
                                                          <asp:ListItem Value="1" Selected="True">Disponible</asp:ListItem>
                                                          <asp:ListItem Value="2">Tomados</asp:ListItem>
                                                          <asp:ListItem Value="3">Todos</asp:ListItem>
                                                          <asp:ListItem Value="4">FechaReserva</asp:ListItem>
                                                      </asp:RadioButtonList>
                                        
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

                                    <asp:LinkButton ID="btnAceptarEXCModal" runat="server" Text="Aceptar" CssClass="btn btn-info" ValidateRequestMode="Enabled" />
                                    <button class="btn btn-info" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                                   
                                </div>

                            </ContentTemplate>

                         





                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </form>
    </body>
    </html>
