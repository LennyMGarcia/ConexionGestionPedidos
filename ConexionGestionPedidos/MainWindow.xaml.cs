using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //le da accedo a los archivos de configuracion par apliaciones como SQL obiene los datos de los [] y los manda a la configuracion y el .ConecctionString establece la cadena conexion
            //Pide la conexxion de la base de datos
            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            //Utiliza el string miconexion para la coneccion a sql
            //es como decirle a la aplicacion que va a realizar consultas sql con lo especficado en el connectionString que es la base de datos de gestio pedidos
            //utiliza la conexion desde la base de datos
            miConexionSql = new SqlConnection(miConexion);

            MuestraClientes();

            muestrTodosPedidos();
            
        }
        private void MuestraClientes()
        {
            try { 
            //Devuleve la info de la tabla cliente, pero hay que especificar la tabla ya que devuelve registro por lo que hay que almacenar en una structura de c#
            string consulta = "SELECT * FROM CLIENTE";
            //se usa para rellenar un datatable o dataset, esto lo hace proporcionando una conexion y los datos para rellenar la misma
            //mira utilizando esta conexion me ejecutas esta consulta
            SqlDataAdapter miAdpatadorSql = new SqlDataAdapter(consulta, miConexionSql);
            //usando este adatador vas a hacer lo siguiente,
            using (miAdpatadorSql)
            {
                DataTable clientesTabla = new DataTable();
                //isando lo que tengo la consulta que se le pidio a la conexion especificada, llena los datos con lo que se consiguio de la consulta en una tabla
                miAdpatadorSql.Fill(clientesTabla);
                //Especifica la informacion que se quiere ver, osea reperesenta la informacion seleccionada
                ListaClientes.DisplayMemberPath = "nombre";
                //Selecciona un elemento especfico basado en el id, especifica la ruta de acceso
                ListaClientes.SelectedValuePath = "Id";
                //Especifica de donde viene la informacion a la lista ya que anteriormente no se especificaba
                ListaClientes.ItemsSource = clientesTabla.DefaultView;
            }

        }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }

        private void MuestraPedidos()
        {
            try
            {
                //Devuleve la info de la tabla cliente, pero hay que especificar la tabla ya que devuelve registro por lo que hay que almacenar en una structura de c#
                string consulta = "SELECT * FROM Pedido P INNER JOIN Cliente C ON C.Id = P.cCliente " +
                    " WHERE C.Id=@lClienteId";
                //Permite ejecutar una inttruccion sql con parametros, representa un procedimiento almacenado o una instruccion T-Sql
                SqlCommand sqlcomando = new SqlCommand(consulta, miConexionSql);
                //se usa para rellenar un datatable o dataset, esto lo hace proporcionando una conexion y los datos para rellenar la misma
                //mira utilizando esta conexion me ejecutas esta consulta
                //Va a recibir los registros de la instruccion parametrica de sqlcommand
                SqlDataAdapter miAdpatadorSql2 = new SqlDataAdapter(sqlcomando);
                //usando este adatador vas a hacer lo siguiente,
                using (miAdpatadorSql2)
                {
                    //Obtiene la informacion de la consulta con su parametro a traves del sqlcommand, utiliza el dato agregado en el @ClienteId a traves del valor seleccionado(Cliente seleccionado)
                    // se le pasa el nombre del parametro y luego de donde viene

                    sqlcomando.Parameters.AddWithValue("@lClienteId", ListaClientes.SelectedValue);
                    DataTable PedidosTabla = new DataTable();
                    //isando lo que tengo la consulta que se le pidio a la conexion especificada, llena los datos con lo que se consiguio de la consulta en una tabla
                    miAdpatadorSql2.Fill(PedidosTabla);
                    //Especifica la informacion que se quiere ver, osea reperesenta la informacion seleccionada
                    PedidosClientes.DisplayMemberPath = "fechaPedido";
                    //Selecciona un elemento especfico basado en el id, especifica la ruta de acceso
                    PedidosClientes.SelectedValuePath = "Id";
                    //Especifica de donde viene la informacion a la lista ya que anteriormente no se especificaba
                    PedidosClientes.ItemsSource = PedidosTabla.DefaultView;
                }
            }
            catch(Exception e)
            {
                //Muestra el error en texot
                MessageBox.Show(e.ToString());
            }
        }
        private void muestrTodosPedidos()
        {
            try
            {
                //el concact sirve para agrupar muchos datos en um solo campo por asi decirlo y el AS es el nombre virtual que se le dara al compo para rescatar luego la informacion
                //concat recibe los otrod valores como argumentos y no como datos, si agregamos el aterisco podemos anadir todos lo campos pero solo mostrar los datos de infocompleta gracias al displaymeberpath
                String consulta = "SELECT *, Concat(p.cCliente,' - ',p.fechaPedido,' - ',p.formaPago) AS InfoCompleta FROM Pedido p";

                SqlDataAdapter miAdapatadorSql = new SqlDataAdapter(consulta, miConexionSql);

                using (miAdapatadorSql)
                {
                    DataTable pedidosTabla = new DataTable();

                    miAdapatadorSql.Fill(pedidosTabla);

                    TodosPEdidos.DisplayMemberPath = "InfoCompleta";
                    TodosPEdidos.SelectedValuePath = "Id";
                    TodosPEdidos.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch
            {

            }
        
        }

        SqlConnection miConexionSql;

      /*  private void ListaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedidos();
        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {//pasar el texto que es object a string, muestra el id gracias  a que lo agregamos como valor seleccionado
            //MessageBox.Show(TodosPEdidos.SelectedValue.ToString());
            //pasa el id del pedido seleccionado por eso el @
            string consulta = "Delete from Pedido where Id=@PedidoId";

            SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();
            //Nombre del parametro y de donde viene el parametro, agrega el valor para mostrar o ayuda a seleccionar el valor a mostrar como comando
            miSqlCommand.Parameters.AddWithValue("@PedidoId", TodosPEdidos.SelectedValue);
            //Se usa para usar aquellos que no muestran datos
            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();
            //llama al metodo para llamarlo nuevamente
            muestrTodosPedidos();

           
            MessageBox.Show("El Registro se elimino exitosamente");




        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            //pasar el texto que es object a string, muestra el id gracias  a que lo agregamos como valor seleccionado
            //MessageBox.Show(TodosPEdidos.SelectedValue.ToString());
            //pasa el id del pedido seleccionado por eso el @
            string consulta = "Insert into Cliente (nombre) values (@nombre)";

            SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();
            //Nombre del parametro y de donde viene el parametro, agrega el valor para mostrar o ayuda a seleccionar el valor a mostrar como comando
            //se toma el texto y lo agrega a la base de datos
            miSqlCommand.Parameters.AddWithValue("@nombre",InsertaCLiente.Text);
            //Se usa para usar aquellos que no muestran datos
            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();
            //llama al metodo para llamarlo nuevamente
            MuestraClientes();


            MessageBox.Show("El Registro se guardado exitosamente");


        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            //pasar el texto que es object a string, muestra el id gracias  a que lo agregamos como valor seleccionado
            //MessageBox.Show(TodosPEdidos.SelectedValue.ToString());
            //pasa el id del pedido seleccionado por eso el @
            string consulta = "Delete from Cliente where Id=@ClienteId";

            SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();
            //Nombre del parametro y de donde viene el parametro, agrega el valor para mostrar o ayudZZa a seleccionar el valor a mostrar como comando
            miSqlCommand.Parameters.AddWithValue("@ClienteId", ListaClientes.SelectedValue);
            //Se usa para usar aquellos que no muestran datos
            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();
            //llama al metodo para llamarlo nuevamente
            MuestraClientes();

           
            InsertaCLiente.Clear();


            MessageBox.Show("El Registro se elimino exitosamente");

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        { //se utiliza el constructor como medio de transporte para pasar el id seleccionado
            //se castea el valor para que el valor seleccionado se convierta a entero
            //esto hace que la ventana emergente rescate el id seleccionado para usarlo como id en la otra ventana gracias a la variable z, despues de haberlo almacenado se almacena en la consulta update
            winActualizar actualizar = new winActualizar((int)ListaClientes.SelectedValue);

            actualizar.Show();
            
            try
            {
                //Devuleve la info de la tabla cliente, pero hay que especificar la tabla ya que devuelve registro por lo que hay que almacenar en una structura de c#
                string consulta = "SELECT nombre FROM CLIENTE where Id = @ClId";

                SqlCommand miSqlcommand = new SqlCommand(consulta, miConexionSql);
                //se usa para rellenar un datatable o dataset, esto lo hace proporcionando una conexion y los datos para rellenar la misma
                //mira utilizando esta conexion me ejecutas esta consulta
                //me ejecutas el comando con el adaotadir
                SqlDataAdapter miAdpatadorSql = new SqlDataAdapter(miSqlcommand);
                //usando este adatador vas a hacer lo siguiente,
                using (miAdpatadorSql)
                {
                   
                    miSqlcommand.Parameters.AddWithValue("ClId", ListaClientes.SelectedValue);

                    DataTable clientestabla = new DataTable();
                    //rellename ka tabla clientes tabla
                    miAdpatadorSql.Fill(clientestabla);
                    //rescatame el campo nombre de la fila 0
                    //el texto de este cuadro de texto es igual a lo de arriba pasdo a texto
                    actualizar.CuadroCliente.Text = clientestabla.Rows[0]["nombre"].ToString();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //showdialog muestra la ventana en primer plano y show se puede presionar otras ventanas, ademas no muestra el flujo hacia abajo hasta que termine la execucio
           /* actualizar.ShowDialog();

            MuestraClientes();*/
        }
        private void ListaClientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MuestraPedidos();
        }
        //Todo lo que se programa aqui  lo ejecutara siempre y cuando la ventana este en primer plano y este en foco, como al ejecutar primera vez la aplicacion o llamar a otra ventana
        //la ventana de actualizar al desparecer la princpial toma el foco y desencadena el metodo o evento
        private void Window_Activated(object sender, EventArgs e)
        {
            //
            MuestraClientes();
        }
    }
}
