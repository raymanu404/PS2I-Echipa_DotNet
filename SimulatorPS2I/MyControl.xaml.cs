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
        public float cap;
        public float debMaxP1;
        public float debMaxP2;
        public float nivCurent;
        public int prag1;
        public int prag2;
        public int prag3;
        public int prag4;
        public int prag5;
        public bool[] isEnabledButton = new bool[9];

        public double valueOfP1;
        public double valueOfP2;

        public MyControl()
        {
            InitializeComponent();
            _vmb = new ViewModelBoiler();
            this.DataContext = _vmb;
          
            this.SubmitButton.IsEnabled = false;
            this.P1Scrollbar.IsEnabled = false;
            this.P2Scrollbar.IsEnabled = false;

            this.InitCondButton.IsEnabled = false;
            this.FillButton.IsEnabled = false;
            this.EmptyButton.IsEnabled = false;
            this.MantainButton.IsEnabled = false;
                                
        }

        private void Button_Click_Start(object sender, RoutedEventArgs e)
        {
         
            _vmb.Init();
            this.InitCondButton.IsEnabled = true;

        }

        private void Button_Click_S1(object sender, RoutedEventArgs e)
        {
            
            if (this.SubmitButton.IsEnabled)
            {

                //aici trimitem conditiile initiale catre ViewModel
                this.InitCondLabel.Content = "";
                _vmb.GetConditions();
                this.FillButton.IsEnabled = true;
                this.EmptyButton.IsEnabled = true;
                this.MantainButton.IsEnabled = true;
            }
            else
            {
                this.InitCondLabel.Content = "Introduceti Datele initiale";
            }
           
        }

        private void Button_Click_S2(object sender, RoutedEventArgs e)
        {
            _vmb.ForceNextState(ProcessState.Filling); //umplere 

            //trebuie pasat valoarea din P1 dar si din P2 , iar umplerea/ golirea sa se faca in functie de p1 si p2
            this.P1Scrollbar.IsEnabled = true;
            this.P2Scrollbar.IsEnabled = false;
        }

        private void Button_Click_S3(object sender, RoutedEventArgs e)
        {
            _vmb.ForceNextState(ProcessState.Emptying); //golire
            this.P1Scrollbar.IsEnabled = false;
            this.P2Scrollbar.IsEnabled = true;
        }
        private void Button_Click_S4(object sender, RoutedEventArgs e)
        {
            _vmb.ForceNextState(ProcessState.BlinkOn);   //stop sau mentinere

        }
        private void Button_Submit(object sender, RoutedEventArgs e)
        {

            //in aceasta metoda vom trimite datele prin intermediul unui tcp catre un server
            this.cap = float.Parse(capacitate.Text);
            this.debMaxP1 = float.Parse(debitMaxP1.Text);
            this.debMaxP2 = float.Parse(debitMaxP2.Text);
            this.nivCurent = float.Parse(nivelCurent.Text);
            this.prag1 = Int32.Parse(pragB1.Text);
            this.prag2 = Int32.Parse(pragB2.Text);
            this.prag3 = Int32.Parse(pragB3.Text);
            this.prag4 = Int32.Parse(pragB4.Text);
            this.prag5 = Int32.Parse(pragB5.Text);
         
                     
        }

        #region ValidareFormular
        private void Capacitate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!capacitate.Text.Equals("") && capacitate.Text.Length != 0 && verificaCifre(capacitate.Text.ToCharArray()))
            {
                isEnabledButton[0] = true;
                CapacitateError.Content = "";
            }
            else
            {
                isEnabledButton[0] = false;
                CapacitateError.Content = "Invalid";
            }
            submitEnabled(isEnabledButton);
        }

        private void DebitMaxP1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!debitMaxP1.Text.Equals("") && debitMaxP1.Text.Length != 0 && verificaCifre(debitMaxP1.Text.ToCharArray()))
            {
                isEnabledButton[1] = true;
                DebitMaxP1Error.Content = "";
            }
            else
            {
                isEnabledButton[1] = false;
                DebitMaxP1Error.Content = "Invalid";
            }

            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }

        }

        private void DebitMaxP2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!debitMaxP2.Text.Equals("") && debitMaxP2.Text.Length != 0 && verificaCifre(debitMaxP2.Text.ToCharArray()))
            {
                isEnabledButton[2] = true;
                DebitMaxP2Error.Content = "";
            }
            else
            {
                isEnabledButton[2] = false;
                DebitMaxP2Error.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void NivelCurent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!nivelCurent.Text.Equals("") && nivelCurent.Text.Length != 0 && verificaCifre(nivelCurent.Text.ToCharArray()))
            {
                isEnabledButton[3] = true;
                NivelCurentError.Content = "";
            }
            else
            {
                isEnabledButton[3] = false;
                NivelCurentError.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void PragB1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!pragB1.Text.Equals("") && pragB1.Text.Length != 0 && verificaCifre(pragB1.Text.ToCharArray()))
            {
                isEnabledButton[4] = true;
                PragB1Error.Content = "";
            }
            else
            {
                isEnabledButton[4] = false;
                PragB1Error.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void PragB2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!pragB2.Text.Equals("") && pragB2.Text.Length != 0 && verificaCifre(pragB2.Text.ToCharArray()))
            {
                isEnabledButton[5] = true;
                PragB2Error.Content = "";
            }
            else
            {
                isEnabledButton[5] = false;
                PragB2Error.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void PragB3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!pragB3.Text.Equals("") && pragB3.Text.Length != 0 && verificaCifre(pragB3.Text.ToCharArray()))
            {
                isEnabledButton[6] = true;
                PragB3Error.Content = "";
            }
            else
            {
                isEnabledButton[6] = false;
                PragB3Error.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void PragB4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!pragB4.Text.Equals("") && pragB4.Text.Length != 0 && verificaCifre(pragB4.Text.ToCharArray()))
            {
                isEnabledButton[7] = true;
                PragB4Error.Content = "";
            }
            else
            {
                isEnabledButton[7] = false;
                PragB4Error.Content = "Invalid";
            }
            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private void PragB5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!pragB5.Text.Equals("") && pragB5.Text.Length != 0 && verificaCifre(pragB5.Text.ToCharArray()))
            {
                isEnabledButton[8] = true;
                PragB5Error.Content = "";
            }
            else
            {
                isEnabledButton[8] = false;
                PragB5Error.Content = "Invalid";
            }

            if (submitEnabled(isEnabledButton))
            {
                SubmitButton.IsEnabled = true;
            }
            else
            {
                SubmitButton.IsEnabled = false;
            }
        }

        private bool verificaCifre(char[] v)
        {
            int nr = 0;
            for (int i = 0; i < v.Length; i++)
            {
                if (
                    v[i] == '0' ||
                    v[i] == '1' ||
                    v[i] == '2' ||
                    v[i] == '3' ||
                    v[i] == '4' ||
                    v[i] == '5' ||
                    v[i] == '6' ||
                    v[i] == '7' ||
                    v[i] == '8' ||
                    v[i] == '9')
                    nr++;
            }
            if (nr == v.Length)
                return true;
            return false;
        }
        private bool submitEnabled(bool[] v)
        {
            int count = 0;
            for (int i = 0; i < v.Length; i++)
            {
                if (!v[i]) return false;
                else count++;

            }
            if (count == v.Length)
                return true;
            return false;
        }
        #endregion
       
        private void P1Scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valueOfP1 = _vmb.P1Value * 100;
            Console.WriteLine(valueOfP1);
        }

        private void P2Scrollbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            valueOfP2 = _vmb.P2Value * 100;
            Console.WriteLine(valueOfP2);
        }
    }
}
