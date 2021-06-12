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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace MyWidgets
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool canMove = true;
        public MainWindow()
        {
            InitializeComponent();

            win.Background = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));

            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();

            SetWeather();

            System.Windows.Threading.DispatcherTimer timer2 = new System.Windows.Threading.DispatcherTimer();
            timer2.Interval = new TimeSpan(0, 1, 0);
            timer2.Tick += new EventHandler(timer2_Tick);
            timer2.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Time.Content = DateTime.Now.ToString("HH:mm:ss tt");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            SetWeather();
        }
        private void SetWeather()
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?q=Omsk&units=metric&appid=29d4bb2ef21b0a14eb3dd311743befb7";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            WeatherResponse weatherResponse = JsonConvert.DeserializeObject<WeatherResponse>(response);
            Weather.Content = weatherResponse.Main.Temp + "°C";
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                if(canMove)
                {
                    DragMove();
                }
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                if (canMove)
                {
                    canMove = false;
                }
                else
                {
                    canMove = true;
                }
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta < 0)
            {
                win.Background = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));
            }
            if (e.Delta > 0)
            {
                win.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            }
        }
    }
}
