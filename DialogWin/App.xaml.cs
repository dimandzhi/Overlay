using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DialogWin
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void GoNow(object sender, StartupEventArgs e)
        {
            MainWindow mw = new MainWindow(e.Args);
            mw.Show();
            //e.Args;
            //base.OnStartup(e);
        }
    }
}
