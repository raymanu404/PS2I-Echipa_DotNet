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
        public float capacitate;

        public void Init()
        {
            if (_isSendingData)
            {
                //_sender = new Comm.Sender("127.0.0.1", 3000);
            }
            
            current_level = 0;
            capacitate = 0;
            _timer.Elapsed += _timer_Elapsed;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
            
        }

        public void GetConditions()
        {

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

                    IsB5 = false; IsLevel5 = false;
                    IsB4 = false; IsLevel4 = false;
                    IsB3 = false; IsLevel3 = false;
                    IsB2 = false; IsLevel2 = false;
                    IsB1 = false; IsLevel1 = false;
                    current_level = 0;
                    RaiseTimerEvent(ProcessState.Off, 2000);
                    break;

                case ProcessState.On:
                    IsB5 = true; IsLevel5 = true;
                    IsB4 = true; IsLevel4 = true;
                    IsB3 = true; IsLevel3 = true;
                    IsB2 = true; IsLevel2 = true;
                    IsB1 = true; IsLevel1 = true;

                    RaiseTimerEvent(ProcessState.On, 2000);
                    break;

                case ProcessState.Filling:
                    switch (current_level)
                    {
                        case 0:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = false; IsLevel1 = false;
                            current_level++;
                            System.Threading.Thread.Sleep(2000);
                            break;
                        case 1:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = true; IsLevel1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 2:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 4:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 5:
                            IsB5 = true; IsLevel5 = true;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            System.Threading.Thread.Sleep(1000);
                            break;

                    }
                    if (current_level == 5)
                    {
                        RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                    }

                    RaiseTimerEvent(ProcessState.Filling, 1000);
                    break;
                case ProcessState.Emptying:

                    switch (current_level)
                    {
                        case 5:
                            IsB5 = true; IsLevel5 = true;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 4:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 2:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 1:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = true; IsLevel1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 0:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = false; IsLevel1 = false;
                            System.Threading.Thread.Sleep(2000);
                            break;
                    }
                    if (current_level == 0)
                    {
                        RaiseTimerEvent(ProcessState.Off, 1000);
                    }

                    RaiseTimerEvent(ProcessState.Emptying, 1000);
                    break;
                case ProcessState.BlinkOn:
                    switch (current_level)
                    {
                        case 5:
                            IsB5 = true; IsLevel5 = true;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 4:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = true; IsLevel4 = true;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 3:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = true; IsLevel3 = true;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 2:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = true; IsLevel2 = true;
                            IsB1 = true; IsLevel1 = true;
                            break;
                        case 1:
                            IsB5 = false; IsLevel5 = false;
                            IsB4 = false; IsLevel4 = false;
                            IsB3 = false; IsLevel3 = false;
                            IsB2 = false; IsLevel2 = false;
                            IsB1 = true; IsLevel1 = true;
                            break;
                    }

                    _setNextState = false;
                    _timer.Stop();
                    RaiseTimerEvent(ProcessState.BlinkOff, 1000);
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

                    _setNextState = false;
                    _timer.Stop();
                    RaiseTimerEvent(ProcessState.BlinkOn, 1000);
                    break;

            }

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
    }

}

