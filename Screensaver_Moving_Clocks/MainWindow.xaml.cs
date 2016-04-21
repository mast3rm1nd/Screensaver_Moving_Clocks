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

using System.Threading;
using System.Diagnostics;

namespace Screensaver_Moving_Clocks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //static Label[] labels = new Label[10];
        

        static Label clocksLabel = new Label();
        static int pixelsShift = (int)(System.Windows.SystemParameters.PrimaryScreenWidth / 1.65);
        //static int movingSpeed = pixelsShift / 200;
        static int movingSpeed = 0;
        static int fontSize = (int)(System.Windows.SystemParameters.PrimaryScreenWidth / 10.5);

        public MainWindow()
        {
            InitializeComponent();

            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            InitializeLabels();
            StartMoving();             
        }

        async void StartMoving()
        {
            while(true)
            {
                await MoveLabelLeft();
                await MoveLabelRight();
            }            
        }




        void InitializeLabels()
        {             
            clocksLabel.VerticalAlignment = VerticalAlignment.Center;
            clocksLabel.HorizontalAlignment = HorizontalAlignment.Center;

            clocksLabel.Margin = new Thickness(0, 0, 0, 0); //left, top, right, bottom

            clocksLabel.Background = new SolidColorBrush(Colors.Black);
            clocksLabel.Foreground = new SolidColorBrush(Colors.White);
            clocksLabel.Content = "Test";
            //clocksLabel.FontFamily = new System.Windows.Media.FontFamily()
            clocksLabel.FontSize = fontSize;

            Grid_MainWindow.Children.Add(clocksLabel);
        }


        async Task MoveLabelLeft()
        {
              
            await Task.Run(() =>
            {
                int needToShift = pixelsShift;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    needToShift += Math.Abs((int)clocksLabel.Margin.Left);
                }));

                while (needToShift != 0)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        clocksLabel.Margin = new Thickness(clocksLabel.Margin.Left - 1, 0, 0, 0);

                        var time = "";

                        switch(Globals.edition)
                        {
                            case "": time = DateTime.Now.ToString("HH:mm:ss"); break;
                            case "bin": time = GetDateTimeNowWithBase(2); break;
                            case "hex": time = GetDateTimeNowWithBase(16); break;
                            default:
                                {
                                    int Base;

                                    if (int.TryParse(Globals.edition, out Base))
                                    {
                                        time = GetDateTimeNowWithBase(Base);
                                    }
                                }
                                break;
                        }

                        clocksLabel.Content = time;
                    }));

                    needToShift--;
                    Thread.Sleep(movingSpeed);
                }   
            });
        }

        static string GetDateTimeNowWithBase(int Base)
        {
            if(Base <= 2)
                return DateTime.Now.ToString("HH:mm:ss");

            var hours = DateTime.Now.Hour;
            var minutes = DateTime.Now.Minute;
            var seconds = DateTime.Now.Second;

            return String.Format("{0}:{1}:{2}",
                Convert.ToString(hours, Base).ToUpper().PadLeft(2, '0'),
                Convert.ToString(minutes, Base).ToUpper().PadLeft(2, '0'),
                Convert.ToString(seconds, Base).ToUpper().PadLeft(2, '0'));
        }



        async Task MoveLabelRight()
        {
            await Task.Run(() =>
            {
                int needToShift = pixelsShift;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    needToShift += Math.Abs((int)clocksLabel.Margin.Left);
                }));

                while (needToShift != 0)
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        clocksLabel.Margin = new Thickness(clocksLabel.Margin.Left + 1, 0, 0, 0);
                        var time = "";

                        switch (Globals.edition)
                        {
                            case "": time = DateTime.Now.ToString("HH:mm:ss"); break;
                            case "bin": time = GetDateTimeNowWithBase(2); break;
                            case "hex": time = GetDateTimeNowWithBase(16); break;
                            default:
                                {
                                    int Base;

                                    if (int.TryParse(Globals.edition, out Base))
                                    {
                                        time = GetDateTimeNowWithBase(Base);
                                    }
                                }
                                break;
                        }

                        clocksLabel.Content = time; 
                    }));

                    needToShift--;
                    Thread.Sleep(movingSpeed);
                }
            });
        }


        




        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var timePassed = DateTime.Now - Process.GetCurrentProcess().StartTime;
            if(timePassed.TotalSeconds > 8)
                this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var timePassed = DateTime.Now - Process.GetCurrentProcess().StartTime;
            if (timePassed.TotalSeconds > 5)
                this.Close();
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
