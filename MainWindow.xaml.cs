//-----------------------------------------------------------
//File:   Prog5.cs
//Desc:   Battleship Server by Stone Champion
//-----------------------------------------------------------
using Battleship.Model;
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

namespace Battleship
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();
            
        }
        //Starts the server when the application loads.
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var server = new Server();

            Task.Run(()=> server.StartServer(Log));
            Log("Server started.");
            


        }
        //Logs messages to server console.
         void Log(string msg)
        {
            Dispatcher.Invoke(()=> serverMessages.Text += DateTime.Now + ": " + msg + "\n");
            Dispatcher.Invoke(()=> serverMessages.ScrollToEnd());

        }

    }
}
