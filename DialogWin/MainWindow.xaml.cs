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

namespace DialogWin
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double fps;

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        public MainWindow(string[] args)
        {
            InitializeComponent();
            AllocConsole();
            ShowWindow(GetConsoleWindow(), SW_HIDE);
            if (args.Length > 0) fps = Convert.ToDouble(args[0]);
            else fps = 29.97;
            Time.ValueChanged += Time_ValueChanged;
            Frames.ValueChanged += Frames_ValueChanged;
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("kernel32.dll")]
        public static extern Boolean AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern Boolean FreeConsole();

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(Frames.Value.ToString());
            Console.WriteLine(Time.Value.ToString());
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FreeConsole();
        }

        private void Frames_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Time.ValueChanged -= Time_ValueChanged;
            Frames.ValueChanged -= Frames_ValueChanged;
            Frames.Value = Math.Round(Convert.ToDouble(Frames.Value));
            Time.Value = Frames.Value * 1000 / fps;
            Time.Value =Math.Round(Convert.ToDouble(Time.Value), 2);
            Time.ValueChanged += Time_ValueChanged;
            Frames.ValueChanged += Frames_ValueChanged;
        }

        private void Time_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
           
            Frames.ValueChanged -= Frames_ValueChanged;
            Time.ValueChanged -= Time_ValueChanged;
            Time.Value = Math.Round(Convert.ToDouble(Time.Value / 10)) * 10;
            Frames.Value = Time.Value / fps;
            Frames.Value = Math.Round(Convert.ToDouble(Frames.Value), 2);
            Frames.ValueChanged += Frames_ValueChanged;
            Time.ValueChanged += Time_ValueChanged;
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                e.Handled = true;
            else if (e.Key.ToString().Length == 2)
            {
                if (!Char.IsDigit(e.Key.ToString(), 1))
                    e.Handled = true;
            }
            else if (e.Key.ToString().Length == 7)
            {
                if (!Char.IsDigit(e.Key.ToString(), 6))
                    e.Handled = true;
            }
            else e.Handled = true;
        }
    }
}
