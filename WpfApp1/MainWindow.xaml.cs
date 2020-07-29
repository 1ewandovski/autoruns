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
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LogonAutoRuns = new Logon();
            ServicesAutoRuns = new Services();
            TasksAutoRuns = new Tasks();
            IEAutoRuns = new Internet_Explorer();
            WinlogonAutoRuns = new Winlogon();
            toolTip1 = new ToolTip();
            this.result.ToolTip = toolTip1;
            FilterFlag = true;
            FilWinFlag = true;
        }

        private Logon LogonAutoRuns;
        private Services ServicesAutoRuns;
        private Tasks TasksAutoRuns;
        private Internet_Explorer IEAutoRuns;
        private Winlogon WinlogonAutoRuns;
        private ToolTip toolTip1;
        private bool FilterFlag;
        private bool FilWinFlag;
        //private Brush MessageColor { get; set; }

        private void Button_Click_Logon(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = LogonAutoRuns.DumpV1(this.FilterFlag);
            //this.result.Foreground
            //this.result.
        }

        private void Button_Click_Services(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = ServicesAutoRuns.DumpV2(false, this.FilWinFlag);
            //this.result.Items[0]
        }

        private void Button_Click_Drivers(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = ServicesAutoRuns.DumpV2(true, this.FilWinFlag);
        }

        private void Button_Click_Tasks(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = TasksAutoRuns.DumpV1(this.FilterFlag);
        }

        private void ListBox_SelectedIndexChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.result.SelectedItems.Count > 0)
            {
                this.toolTip1.IsEnabled = true;
                this.toolTip1.Content = this.result.Items[this.result.SelectedIndex].ToString();
            }
            else
            {                
                this.toolTip1.IsEnabled = false;
            }
        }

        private void Button_Click_IE(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = IEAutoRuns.DumpV1(this.FilterFlag);
        }

        private void Button_Click_Filter(object sender, RoutedEventArgs e)
        {
            this.FilterFlag = !this.FilterFlag;
        }

        private void Button_Click_FilWindows(object sender, RoutedEventArgs e)
        {
            this.FilWinFlag = !this.FilWinFlag;
        }

        private void Button_Click_Winlogon(object sender, RoutedEventArgs e)
        {
            this.result.ItemsSource = WinlogonAutoRuns.DumpV1(this.FilterFlag);
        }
    }
    
}
