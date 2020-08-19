using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIstemaRestaurant2
{
    /// <summary>
    /// Lógica de interacción para MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        DataTable tabla;
        public MenuAdmin()
        {
            InitializeComponent();

        }



        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                ClsBDatos conexion = new ClsBDatos();

                SqlConnection cnn = conexion.AbriConexion();
                string consulta = "select * from TBL_RestProductos";
                SqlCommand cmd = new SqlCommand(consulta, cnn);
                SqlDataReader leerDatos = cmd.ExecuteReader();
                tabla = new DataTable();
                tabla.Columns.Add("ID");
                tabla.Columns.Add("Producto");
                tabla.Columns.Add("Descripcion");
                tabla.Columns.Add("Precio", typeof(int));

                while (leerDatos.Read())
                {
                    string id = leerDatos["id"].ToString();
                    string producto = leerDatos["producto"].ToString();
                    string descripcion = leerDatos["descripcion"].ToString();
                    string precio = leerDatos["precio"].ToString();
                    tabla.Rows.Add(id, producto, descripcion, precio);
                }

                grilla.ItemsSource = tabla.CreateDataReader();

                conexion.CerrarConexion();

            }
            catch (Exception)
            {

                MessageBox.Show("Error en conexion a base de datos", "Sistema Restaurant", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult opcion = MessageBox.Show("¿Realmente desea salir de la aplicacion?", "SistemaRestaurant", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClsBDatos conexion = new ClsBDatos();
                SqlConnection cnn = conexion.AbriConexion();
                string consulta = "select * from TBL_RestProductos";
                SqlCommand cmd = new SqlCommand(consulta, cnn);
                SqlDataReader leerDatos = cmd.ExecuteReader();
                tabla = new DataTable();
                tabla.Columns.Add("ID");
                tabla.Columns.Add("Producto");
                tabla.Columns.Add("Descripcion");
                tabla.Columns.Add("Precio", typeof(int));
                while (leerDatos.Read())
                {
                    string id = leerDatos["id"].ToString();
                    string producto = leerDatos["producto"].ToString();
                    string descripcion = leerDatos["descripcion"].ToString();
                    string precio = leerDatos["precio"].ToString();
                    tabla.Rows.Add(id, producto, descripcion, precio);
                }

                grilla.ItemsSource = tabla.CreateDataReader();
                conexion.CerrarConexion();

            }
            catch (Exception)
            {

                MessageBox.Show("Error en conexion a base de datos", "Sistema Restaurant", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnIngresar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClsBDatos conexion = new ClsBDatos();
                SqlConnection cnn = conexion.AbriConexion();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Rest_InsertaProducto_SP";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_producto", txtProducto.Text);
                cmd.Parameters.AddWithValue("@in_descripcion", txtDesc.Text);
                cmd.Parameters.AddWithValue("@in_precio", int.Parse(txtPrecio.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Producto Insertado Correctamente", "Producto", MessageBoxButton.OK, MessageBoxImage.Information);
                conexion.CerrarConexion();
            }
            catch (Exception)
            {
                MessageBox.Show("Error de conexion", "Producto", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }

        private static readonly Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
        private static bool SePermiteTexto(string text)
        {
            return ! regex.IsMatch(text);
        }
        private void txtPrecio_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !SePermiteTexto(e.Text);
            
        }
    }
    
    
}
