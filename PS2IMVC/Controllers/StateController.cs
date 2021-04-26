using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace PS2IMVC.Controllers
{
    public class StateController : Controller
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
        public class ViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private System.Timers.Timer _timer =new System.Timers.Timer();
            private ProcessState _nextstate;
            private bool _setNextState = false;

            public void ProcessNextState(ProcessState CurrentState)
            {
                switch (CurrentState)
                {
                    case ProcessState.Off:
                        
                        RaiseTimerEvent(ProcessState.Off, 2000);

                        break;
                    case ProcessState.On:
                        RaiseTimerEvent(ProcessState.Off, 2000);
                        break;
                    case ProcessState.Filling:

                        RaiseTimerEvent(ProcessState.Off, 2000);
                        break;
                    case ProcessState.Emptying:


                        RaiseTimerEvent(ProcessState.Off, 2000);                
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
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}