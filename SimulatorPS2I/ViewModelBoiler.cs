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

        public void Init()
        {
            if (_isSendingData)
            {
                //_sender = new Comm.Sender("127.0.0.1", 3000);
            }
            current_level = 0;
            _timer.Elapsed += _timer_Elapsed;
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerAsync();
        }

        public void SendData()
        {
            if (_isSendingData)
            {
                //_sender.Send(Convert.ToByte(_currentStateOfTheProcess));
            }
        }

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
            // idea de baza a simulatorului este urmatoarea: backgroundworker-ul evalueza tot la 100 de milisecunde starea curenta a procesului 
            // si seteaza in viewmodel variabilele care actualizeaza UI-ul dupa care utilizand RaiseTimerEvent(NextProcessState, 2000) determina o tranzitie de stare peste un
            // interval de timp specificat de al doilea paramteru al acestei metode

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

                    IsB5 = false;
                    IsB4 = false;
                    IsB3 = false;
                    IsB2 = false;
                    IsB1 = false;
                    current_level = 0;
                    RaiseTimerEvent(ProcessState.Off, 2000);
                    break;

                case ProcessState.On:
                    IsB5 = true;
                    IsB4 = true;
                    IsB3 = true;
                    IsB2 = true;
                    IsB1 = true;

                    RaiseTimerEvent(ProcessState.On, 2000);
                    break;

                case ProcessState.Filling:
                    if (current_level == 0)
                    {
                        current_level++;
                    }

                     switch (current_level) {
                        case 1:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = false;
                            IsB2 = false;
                            IsB1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 2:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = false;
                            IsB2 = true;
                            IsB1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 4:
                            IsB5 = false;
                            IsB4 = true;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            current_level++;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 5:
                            IsB5 = true;
                            IsB4 = true;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            System.Threading.Thread.Sleep(1000);
                            break;
                    
                     }


                    if (current_level == 5)
                    {
                        RaiseTimerEvent(ProcessState.On, 1000);
                    }

                    Console.WriteLine("current level : " + current_level);
                    RaiseTimerEvent(ProcessState.Filling, 1000);
                    break;              
                case ProcessState.Emptying:


                    switch (current_level)
                    {
                        case 5:
                            IsB5 = true;
                            IsB4 = true;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 4:
                            IsB5 = false;
                            IsB4 = true;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 3:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = true;
                            IsB2 = true;
                            IsB1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 2:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = false;
                            IsB2 = true;
                            IsB1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 1:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = false;
                            IsB2 = false;
                            IsB1 = true;
                            current_level--;
                            System.Threading.Thread.Sleep(1000);
                            break;
                        case 0:
                            IsB5 = false;
                            IsB4 = false;
                            IsB3 = false;
                            IsB2 = false;
                            IsB1 = false;
                            System.Threading.Thread.Sleep(1000);
                            break;
                    }
                    if (current_level == 0)
                    {
                        RaiseTimerEvent(ProcessState.Off, 1000);
                    }
                    Console.WriteLine("current_level  : " + current_level);
                    RaiseTimerEvent(ProcessState.Emptying, 1000);
                    break;

            }

        }


        private void nextLevel(int current_level,bool flag)
        {
            


            //switch (current_level)
            //{
            //    case 0:
            //        IsB5 = false;
            //        IsB4 = false;
            //        IsB3 = false;
            //        IsB2 = false;
            //        IsB1 = false;
            //        RaiseTimerEvent(ProcessState.Off, 1000);
            //        break;
            //    case 1:
            //        IsB5 = false;
            //        IsB4 = false;
            //        IsB3 = false;
            //        IsB2 = false;
            //        IsB1 = true;

            //        if (flag)
            //        {
            //            current_level++;
            //        }
            //        else
            //            current_level--;
            //        System.Threading.Thread.Sleep(1000);
            //        break;
            //    case 2:
            //        IsB5 = false;
            //        IsB4 = false;
            //        IsB3 = false;
            //        IsB2 = true;
            //        IsB1 = true;

            //        if (flag)
            //        {
            //            current_level++;
            //        }
            //        else
            //            current_level--;
            //        System.Threading.Thread.Sleep(1000);
            //        break;
            //    case 3:
            //        IsB5 = false;
            //        IsB4 = false;
            //        IsB3 = true;
            //        IsB2 = true;
            //        IsB1 = true;

            //        if (flag)
            //        {
            //            current_level++;
            //        }
            //        else
            //            current_level--;
            //        System.Threading.Thread.Sleep(1000);
            //        break;
            //    case 4:
            //        IsB5 = false;
            //        IsB4 = true;
            //        IsB3 = true;
            //        IsB2 = true;
            //        IsB1 = true;

            //        if (flag)
            //        {
            //            current_level++;
            //        }
            //        else
            //            current_level--;
            //        System.Threading.Thread.Sleep(1000);
            //        break;
            //    case 5:
            //        IsB5 = true;
            //        IsB4 = true;
            //        IsB3 = true;
            //        IsB2 = true;
            //        IsB1 = true;

            //        if (flag)
            //        {
            //            current_level++;
            //        }
            //        else
            //            current_level--;
            //        System.Threading.Thread.Sleep(1000);
            //        break;

            //}
           
            
           
           

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


        #region UI_updates
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
        #endregion
    }

}

