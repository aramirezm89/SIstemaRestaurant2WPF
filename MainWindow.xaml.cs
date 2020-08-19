using OpenQA.Selenium;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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


namespace SIstemaRestaurant2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
             MessageBoxResult opcion = MessageBox.Show("¿Realmente desea salir de la aplicacion?", "SistemaRestaurant", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (opcion == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Ingresar();
        }

        public void Ingresar()
        {
            try
            {

                ClsBDatos conexion = new ClsBDatos();
                SqlConnection cnn = conexion.AbriConexion();
                SqlCommand cmd = cnn.CreateCommand();
                cmd.CommandText = "Rest_BuscaUsuario_SP";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@in_usuario", txtUsuario.Text);
                cmd.Parameters.AddWithValue("@in_pass", txtPass.Password);
                SqlDataReader LeerDatos = cmd.ExecuteReader(); //lee lo datos que hay en la tabla SQL

                if ((LeerDatos.Read() == true && LeerDatos["tipo"].Equals("administrador")))
                {
                    MessageBox.Show("Bienvenido(a)" + " " + LeerDatos["nombre"], "Acceso",MessageBoxButton.OK,MessageBoxImage.Information);
                    MenuAdmin frm = new MenuAdmin();
                    frm.Show();
                    this.Hide();
                   
                }
                else if (LeerDatos["tipo"].Equals("usuario"))
                {
                    MessageBox.Show("Bienvenido(a)" + " " + LeerDatos.GetString(1), "Acceso", MessageBoxButton.OK,MessageBoxImage.Information);
                  
                }

            }
            catch (Exception)
            {


                MessageBox.Show("Usuario no existe", "Acceso", MessageBoxButton.OK, MessageBoxImage.Error);
                txtUsuario.Clear();
                txtPass.Clear();
                txtUsuario.Focus();
            }
        }

        private void txtPass_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtUsuario.Text.Equals("") || txtPass.Password.Equals(""))
                {
                    MessageBox.Show("Debe ingresar usuario y contraseña", "Login", MessageBoxButton.OK,MessageBoxImage.Error);
                    txtUsuario.Focus();
                }
                else
                {
                    Ingresar();

                }
            }
        }
    }

}

