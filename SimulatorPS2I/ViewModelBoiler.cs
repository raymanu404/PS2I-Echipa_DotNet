using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SimulatorPS2I
{

    public enum ButtonState
    {
        Off,
        S5,
        S0,
        On
    }
    public enum ProcessState
    {
        Off,
        Filling,
        Emptying,
        BlinkOn,
        BlinkOff,
        On
    }
    class ViewModelBoiler : INotifyPropertyChanged
    {
        int current_level = 0;
        public event PropertyChangedEventHandler PropertyChanged;
        private System.Timers.Timer _timer = new System.Timers.Timer();
        private ProcessState _nextstate;
        private bool _setNextState = false;
        private bool _isSendingData = false;
        private BackgroundWorker _worker = new BackgroundWorker();
        
        //declarare Sender
        
        
        public float capacitate;
        public float debMaxP1;
        public float debMaxP2;
        public float nivel_curent;
        public int prag1;
        public int prag2;
        public int prag3;
        public int prag4;
        public int prag5;
        public bool getParameters;
        private float valueOfP;

        private float timp_nivel;

        public void Init()
        {
            if (_isSendingData)
            {
                //_sender = new Comm.Sender("127.0.0.1", 3000);
            }
            getParameters = false;
            current_level = 0;
            capacitate = 0;
            valueOfP = 0;
            _timer.Elapsed += _timer_Elapsed;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
            
        }

        public void GetConditions(float capacitate, float debMaxP1, float debMaxP2, float nivel_curent, int prag1, int prag2, int prag3, int prag4, int prag5)
        {
            this.capacitate = capacitate;
            this.debMaxP1 = debMaxP1;
            this.debMaxP2 = debMaxP2;
            this.nivel_curent = nivel_curent;
            this.prag1 = prag1;
            this.prag2 = prag2;
            this.prag3 = prag3;
            this.prag4 = prag4;
            this.prag5 = prag5;

            ForceNextState(ProcessState.On);
            getParameters = true;
        }
        public void SendData()
        {
            if (_isSendingData)
            {
                //_sender.Send(Convert.ToByte(_currentStateOfTheProcess));
            }
            
        }
        
        #region Simulator

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void RaiseTimerEvent(ProcessState NextStateOfTheProcess, int TimeInterval)
        {
            if (!_setNextState)
            {
                _setNextState = true;
                _nextstate = NextStateOfTheProcess;
                _timer.Interval = TimeInterval;
                _timer.Start();
            }
        }

        public void ForceNextState(ProcessState NextState)
        {
            CurrentStateOfTheProcess = NextState;
        }

        private ProcessState _currentStateOfTheProcess = ProcessState.Off;
        public ProcessState CurrentStateOfTheProcess
        {
            get => _currentStateOfTheProcess;
            set
            {
                // urmatoarele doua linii de cod opresc timerul deoarece a avut loc o tranzitie de stare
                _setNextState = false;
                _timer.Stop();

                // dupa care se actualizeaza starea curenta si se trimit actualizarile de stare mai departe
                _currentStateOfTheProcess = value;
                SendData();
            }
        }
        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CurrentStateOfTheProcess = _nextstate;
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e)
        {

            while (true)
            {
                ProcessNextState(CurrentStateOfTheProcess);
                System.Threading.Thread.Sleep(100);
            }
        }

        public void ProcessNextState(ProcessState CurrentState)
        {
            switch (CurrentState)
            {
                case ProcessState.Off:

                    Level0();
                    current_level = 0;
                    RaiseTimerEvent(ProcessState.Off, 2000);
                    break;

                case ProcessState.On:
                    if (this.nivel_curent >= this.prag1 && this.nivel_curent < (this.prag1 + this.prag2))
                    {
                        Level1();
                        current_level = 1;
                    }
                    else if (this.nivel_curent >= this.prag2 && this.nivel_curent < (this.prag1 + this.prag2 + this.prag3))
                    {
                        Level2();
                        current_level = 2;
                    }
                    else if (this.nivel_curent >= this.prag3 && this.nivel_curent < (this.prag1 + this.prag2 + this.prag3 + this.prag4))
                    {
                        Level3();
                        current_level = 3;
                    }
                    else if (this.nivel_curent >= this.prag4 && this.nivel_curent < (this.prag1 + this.prag2 + this.prag3 + this.prag4 + this.prag5))
                    {
                        Level4();
                        current_level = 4;
                    }
                    RaiseTimerEvent(ProcessState.On, 1000);
                    break;

                case ProcessState.Filling:
                    //trebuie pentru P1 
                    
                    if (this.P1Value == 0)
                        this.P1Value = 1;
                    if (this.P2Value == 0.1)
                    {
                        this.valueOfP = (float)this.P1Value;
                    }
                    Console.WriteLine("P1 : " + this.P1Value + " \nP2: " + this.P2Value);
                    Console.WriteLine("suma : " + (valueOfP) );
                    switch (current_level)
                    {
                        case 0:
                            Level0();

                            if(nivel_curent <= capacitate)
                            {
                                if(valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                timp_nivel = (float)((this.valueOfP * this.debMaxP1 * this.prag1) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                                //RaiseTimerEvent(ProcessState.Filling, 2000);
                               
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }
                           
                            break;
                        case 1:
                            Level1();
                            if (nivel_curent <= capacitate)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                this.nivel_curent += this.prag1;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP1 * this.prag2) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                                //RaiseTimerEvent(ProcessState.Filling,2000);
                               
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }

                            break;
                        case 2:
                            Level2();
                            if (nivel_curent <= capacitate)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                this.nivel_curent += this.prag2;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP1 * this.prag2) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                                //RaiseTimerEvent(ProcessState.Filling, (int)timp_nivel);
                               
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }  
                            
                            break;
                        case 3:
                            Level3();
                            if (nivel_curent <= capacitate)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                this.nivel_curent += this.prag3;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP1 * this.prag3) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                                //RaiseTimerEvent(ProcessState.Filling, (int)timp_nivel);
                               

                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }
                        
                            break;
                        case 4:
                            Level4();
                            if (nivel_curent <= capacitate)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                this.nivel_curent += this.prag4;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP1 * this.prag4) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                                //RaiseTimerEvent(ProcessState.Filling, (int)timp_nivel);
                               
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }
                         
                            break;
                        case 5:
                            Level5();
                            if (nivel_curent <= capacitate)
                            {
                                this.nivel_curent += this.prag5;
                                RaiseTimerEvent(ProcessState.BlinkOn, 500);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                            }

                            break;

                    }
                   
                    //Console.WriteLine("nivel curent : "+this.nivel_curent + " cap max : " + this.capacitate);
                    if ( current_level != 6)
                    {
                        current_level++;
                        RaiseTimerEvent(ProcessState.Filling, 1000);
                    }
                    
                    break;
                case ProcessState.Emptying:
                    if (this.P2Value == 0)
                        this.P2Value = 1;
                    if (this.P1Value == 0.1)
                    {
                        this.valueOfP = (float)this.P2Value;
                    }
                    Console.WriteLine("\nP1 : " + this.P1Value + " \nP2: " + this.P2Value);
                    Console.WriteLine("\n suma : " + (P1Value - P2Value));
                    if (current_level == 6)
                    {
                        current_level--;
                    }
                    switch (current_level)
                    {
                        case 5:
                            Level5();
                            if(nivel_curent >= 0)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                nivel_curent -= this.prag5;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP2 * this.prag5) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                            }
                                                     
                            break;
                        case 4:
                            Level4();
                            if (nivel_curent >= 0)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                nivel_curent -= this.prag4;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP2 * this.prag4) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                            }

                            break;
                        case 3:
                            Level3();

                            if (nivel_curent >= 0)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                nivel_curent -= this.prag3;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP2 * this.prag3) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                            }

                            break;
                        case 2:

                            Level2();
                            if (nivel_curent >= 0)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                nivel_curent -= this.prag2;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP2 * this.prag2) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                            }

                            break;
                        case 1:
                            Level1();
                            if (nivel_curent >= 0)
                            {
                                if (valueOfP < 0)
                                {
                                    valueOfP *= (-1);
                                }
                                nivel_curent -= this.prag1;
                                timp_nivel = (float)((this.valueOfP * this.debMaxP2 * this.prag1) / 1000);
                                System.Threading.Thread.Sleep((int)timp_nivel);
                            }
                            else
                            {
                                RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                            }

                            break;
                        case 0:
                            Level0();
                            RaiseTimerEvent(ProcessState.Off, 1000);
                            break;
                    }
                    //Console.WriteLine("nivel curent : " + this.nivel_curent + " cap max : " + this.capacitate);
                    if (current_level != 0)
                    {
                        current_level--;
                        RaiseTimerEvent(ProcessState.Emptying, 1000);
                    }                             
                    break;
                case ProcessState.BlinkOn:
                    if(this.P1Value < this.P2Value)
                    {
                        valueOfP =(float)(P1Value - P2Value);
                        Console.WriteLine("este umplereee");
                        RaiseTimerEvent(ProcessState.Filling, 1000);
                       
                    }else if (this.P1Value > this.P2Value)
                    {
                        valueOfP = (float)(P2Value - P1Value);
                        Console.WriteLine("este golireee");
                        RaiseTimerEvent(ProcessState.Emptying, 1000);
                    }
                    else
                    {
                        valueOfP = 1;
                        switch (current_level)
                        {
                            case 5:
                                Level5();
                                break;
                            case 4:
                                Level4();
                                break;
                            case 3:
                                Level3();
                                break;
                            case 2:
                                Level2();
                                break;
                            case 1:
                                Level1();
                                break;
                        }
                        RaiseTimerEvent(ProcessState.BlinkOff, 1000);
                    }                    
                    break;

                case ProcessState.BlinkOff:
                    switch (current_level)
                    {
                        case 5:
                            IsB5 = false; IsLevel5 = true;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 4:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 3:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 2:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 1:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = false; IsLevel1 = true;
                            break;
                    }
                    RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                    break;

            }

        }

       
        #endregion

        #region UI_updates
        //pentru Potentiometrul P1 umplere
        private double _p1Value;    
        public double P1Value
        {
            get
            {
                return _p1Value;
            }
            set
            {
                _p1Value = value;
                OnPropertyChanged(nameof(P1Value));
            }
        }

        //pentru Potentiometrul P2 golire
        private double _p2Value;
        public double P2Value
        {
            get
            {
                return _p2Value;
            }
            set
            {
                _p2Value = value;
                OnPropertyChanged(nameof(P2Value));
            }
        }

        //pentru senzorul B5     
        private bool _isB5;
        public bool IsB5
        {
            get
            {
                return _isB5;
            }
            set
            {
                _isB5 = value;
                OnPropertyChanged(nameof(IsB5Visible));

            }
        }

        public System.Windows.Visibility IsB5Visible
        {
            get
            {
                if (_isB5)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        //pentru senzorul B4
        private bool _isB4;
        public bool IsB4
        {
            get
            {
                return _isB4;
            }
            set
            {
                _isB4 = value;
                OnPropertyChanged(nameof(IsB4Visible));

            }
        }

        public System.Windows.Visibility IsB4Visible
        {
            get
            {
                if (_isB4)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }


        //pentru senzorul B3
        private bool _isB3;
        public bool IsB3
        {
            get
            {
                return _isB3;
            }
            set
            {
                _isB3 = value;
                OnPropertyChanged(nameof(IsB3Visible));

            }
        }

        public System.Windows.Visibility IsB3Visible
        {
            get
            {
                if (_isB3)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }


        //pentru senzorul B2
        private bool _isB2;
        public bool IsB2
        {
            get
            {
                return _isB2;
            }
            set
            {
                _isB2 = value;
                OnPropertyChanged(nameof(IsB2Visible));

            }
        }

        public System.Windows.Visibility IsB2Visible
        {
            get
            {
                if (_isB2)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }


        //pentru senzorul B1
        private bool _isB1;
        public bool IsB1
        {
            get
            {
                return _isB1;
            }
            set
            {
                _isB1 = value;
                OnPropertyChanged(nameof(IsB1Visible));

            }
        }

        public System.Windows.Visibility IsB1Visible
        {
            get
            {
                if (_isB1)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        //pentru level 1
        private bool _isLevel1;
        public bool IsLevel1
        {
            get
            {
                return _isLevel1;
            }
            set
            {
                _isLevel1 = value;
                OnPropertyChanged(nameof(IsLevel1Visible));

            }
        }

        public System.Windows.Visibility IsLevel1Visible
        {
            get
            {
                if (_isLevel1)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        //pentru level 2
        private bool _isLevel2;
        public bool IsLevel2
        {
            get
            {
                return _isLevel2;
            }
            set
            {
                _isLevel2 = value;
                OnPropertyChanged(nameof(IsLevel2Visible));

            }
        }

        public System.Windows.Visibility IsLevel2Visible
        {
            get
            {
                if (_isLevel2)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        //pentru level 3
        private bool _isLevel3;
        public bool IsLevel3
        {
            get
            {
                return _isLevel3;
            }
            set
            {
                _isLevel3 = value;
                OnPropertyChanged(nameof(IsLevel3Visible));

            }
        }

        public System.Windows.Visibility IsLevel3Visible
        {
            get
            {
                if (_isLevel3)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        //pentru level 4
        private bool _isLevel4;
        public bool IsLevel4
        {
            get
            {
                return _isLevel4;
            }
            set
            {
                _isLevel4 = value;
                OnPropertyChanged(nameof(IsLevel4Visible));

            }
        }

        public System.Windows.Visibility IsLevel4Visible
        {
            get
            {
                if (_isLevel4)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }
        //pentru level 5
        private bool _isLevel5;
        public bool IsLevel5
        {
            get
            {
                return _isLevel5;
            }
            set
            {
                _isLevel5 = value;
                OnPropertyChanged(nameof(IsLevel5Visible));

            }
        }

        public System.Windows.Visibility IsLevel5Visible
        {
            get
            {
                if (_isLevel5)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Hidden;
                }
            }
        }

        #endregion

        #region Levels
        private void Level5()
        {
            IsB5 = true; IsLevel5 = true;
            IsB4 = true; IsLevel4 = true;
            IsB3 = true; IsLevel3 = true;
            IsB2 = true; IsLevel2 = true;
            IsB1 = true; IsLevel1 = true;

        }

        private void Level4()
        {
            IsB5 = false; IsLevel5 = false;
            IsB4 = true; IsLevel4 = true;
            IsB3 = true; IsLevel3 = true;
            IsB2 = true; IsLevel2 = true;
            IsB1 = true; IsLevel1 = true;
        }

        private void Level3()
        {
            IsB5 = false; IsLevel5 = false;
            IsB4 = false; IsLevel4 = false;
            IsB3 = true; IsLevel3 = true;
            IsB2 = true; IsLevel2 = true;
            IsB1 = true; IsLevel1 = true;
        }

        private void Level2()
        {
            IsB5 = false; IsLevel5 = false;
            IsB4 = false; IsLevel4 = false;
            IsB3 = false; IsLevel3 = false;
            IsB2 = true; IsLevel2 = true;
            IsB1 = true; IsLevel1 = true;
        }

        private void Level1()
        {
            IsB5 = false; IsLevel5 = false;
            IsB4 = false; IsLevel4 = false;
            IsB3 = false; IsLevel3 = false;
            IsB2 = false; IsLevel2 = false;
            IsB1 = true; IsLevel1 = true;

        }

        private void Level0()
        {
            IsB5 = false; IsLevel5 = false;
            IsB4 = false; IsLevel4 = false;
            IsB3 = false; IsLevel3 = false;
            IsB2 = false; IsLevel2 = false;
            IsB1 = false; IsLevel1 = false;
        } 
        #endregion

    }
  
}

