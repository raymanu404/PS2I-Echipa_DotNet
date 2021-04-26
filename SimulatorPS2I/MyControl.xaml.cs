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

namespace SimulatorPS2I
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    
   
    public partial class MyControl : UserControl
    {
        ViewModelBoiler _vmb;

        public MyControl()
        {
            InitializeComponent();
            _vmb = new ViewModelBoiler();

        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_S1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_S2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_S3(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_S4(object sender, RoutedEventArgs e)
        {

        }
    }
}
