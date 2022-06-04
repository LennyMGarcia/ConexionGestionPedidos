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
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ConexionGestionPedidos
{
    /// <summary>
    /// Lógica de interacción para winActualizar.xaml
    /// </summary>
    public partial class winActualizar : Window
    {
        SqlConnection miConexionSql;
        //se declara aqui para ser reconocida tanto dentro del constructor como fuera
        private int z;
        //se utiliza el constructor como medio de transporte para pasar el id seleccionado
        public winActualizar(int elID)
        {
            InitializeComponent();
            z = elID;
            //le da accedo a los archivos de configuracion par apliaciones como SQL obiene los datos de los [] y los manda a la configuracion y el .ConecctionString establece la cadena conexion
            //Pide la conexxion de la base de datos
            string miConexion = ConfigurationManager.ConnectionStrings["ConexionGestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            //Utiliza el string miconexion para la coneccion a sql
            //es como decirle a la aplicacion que va a realizar consultas sql con lo especficado en el connectionString que es la base de datos de gestio pedidos
            //utiliza la conexion desde la base de datos
           miConexionSql = new SqlConnection(miConexion);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //pasar el texto que es object a string, muestra el id gracias  a que lo agregamos como valor seleccionado
            //MessageBox.Show(TodosPEdidos.SelectedValue.ToString());
            //pasa el id del pedido seleccionado por eso el @
            string consulta = "Update cliente set nombre = @nombre where Id =" + z;

            SqlCommand miSqlCommand = new SqlCommand(consulta, miConexionSql);

            miConexionSql.Open();
            //Nombre del parametro y de donde viene el parametro, agrega el valor para mostrar o ayuda a seleccionar el valor a mostrar como comando
            //se toma el texto y lo agrega a la base de datos
            miSqlCommand.Parameters.AddWithValue("@nombre", CuadroCliente.Text);
            //Se usa para usar aquellos que no muestran datos
            miSqlCommand.ExecuteNonQuery();

            miConexionSql.Close();
            //llama al metodo para llamarlo nuevamente
            //EK this es hace referencia a la clase actualiza como si fuera un objeto
            this.Close();

            MessageBox.Show("El Registro se actualizado exitosamente");
        }
    }
}
