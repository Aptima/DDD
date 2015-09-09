
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Aptima.Asim.DDD.ScenarioController
{
    public class Metronome : IDisposable
    {
        private static Metronome _instance = null;
        private int _updateIncrement;//each "tick" takes this many ms, and updates the time slice by that much.
        private int _timeSlice; //number of milliseconds that the simulation has processed. "current time"
        private double _speedFactor = 1.0; //affects rate of sleep.
        private Thread _timerThread = null;
        private static object _threadLock = new object();
        private bool _isTicking = false;
        private static bool _isRunnable = false;
        public delegate void SendTimeTick(object time);
        private SendTimeTick _delegate = null;

        public bool GetRunnable()
        {
            lock (_threadLock)
            { 
                return _isRunnable; 
            }
        }
        protected void SetRunnable(bool value)
        {
            lock (_threadLock)
            {
                _isRunnable = value;
                if (!value)
                {
                    _delegate = null;
                }
            }
        }
        public void SetCallback(SendTimeTick ev)
        {
            lock (_threadLock)
            {
                _delegate = ev;
            }
        }
        public int UpdateIncrement
        {
            get
            {
                lock (_threadLock)
                { return _updateIncrement; }
            }
            set
            {
                lock (_threadLock)
                { _updateIncrement = value; }
            }
        }
        public int TimeSlice
        {
            get
            {
                lock (_threadLock)
                { return _timeSlice; }
            }
        }

        private void SetTimeSlice(int value)
        {
            lock (_threadLock)
            {
                _timeSlice = value;
            }
        }
        private void SetIsTicking(bool value)
        {
            lock (_threadLock)
            {
                _isTicking = value;
            }
        }
        private bool GetIsTicking()
        {
            lock (_threadLock)
            {
                return _isTicking;
            }
        }
        public double SpeedFactor
        {
            get
            {
                lock (_threadLock)
                { return _speedFactor; }
            }
            set
            {
                lock (_threadLock)
                { _speedFactor = value; }
            }
        }

        public static Metronome GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Metronome();
            }

            return _instance;
        }

        private Metronome()
        {
            _updateIncrement = 0;
            _timeSlice = 0;
            _speedFactor = 1.0;
            _isTicking = false;
            _isRunnable = false;
            _timerThread = new Thread(new ThreadStart(BeginTimerThread));
        }
        private Metronome(int updateIncrement, int timeSlice, double speedFactor, bool isTicking)
        {
            _updateIncrement = updateIncrement;
            _timeSlice = timeSlice;
            _speedFactor = speedFactor;
            _isTicking = isTicking;
            _isRunnable = false;
            _timerThread = new Thread(new ThreadStart(BeginTimerThread));
        }

        public void Pause()
        {
            SetIsTicking(false);
        }

        public void Resume()
        {//used to start as well
            SetIsTicking(true);
        }

        public void Reset()
        {
            SetIsTicking(false);
            SetTimeSlice(0);
        }

        public void Start()
        {
            SetRunnable(true);
            if (_timerThread == null)
            {
                _timerThread = new Thread(new ThreadStart(BeginTimerThread));
            }
            if (!_timerThread.IsAlive)
            {
                _timerThread = new Thread(new ThreadStart(BeginTimerThread));
                SetTimeSlice(0);
                _timerThread.Start();
            }
            //Resume();
        }
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these 
            // operations, as well as in your methods that use the resource.
            if (!_disposed)
            {
                if (disposing)
                {
                    CleanupMetronome();
                }

                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);  
            
        }

        public void CleanupMetronome()
        {
            SetRunnable(false);
            Thread.Sleep(1500); //give thread enough time to gracefully stop.
            if (_timerThread != null)
            {
                if (_timerThread.IsAlive)
                {
                    _timerThread.Abort();
                }
                _timerThread = null;
            }
            _instance = null;
        }
        
        private void BeginTimerThread()
        {
            //SetRunnable(true);
            int updateIncrement = 0;
            double speedFactor = 0.0d;
#if TimerTest
            DateTime current = DateTime.Now;
            DateTime previous = DateTime.Now;
#endif
            while (GetRunnable())
            {
                while (!GetIsTicking())
                {
                    Thread.Sleep(1000);
                    if (!GetRunnable())
                    {
                        return;
                        //break;
                    }
                }
                lock (_threadLock)
                {
                    updateIncrement = _updateIncrement;
                    speedFactor = _speedFactor;
                    _timeSlice = _timeSlice + updateIncrement;
#if TimerTest
                    current = DateTime.Now;
                    Console.WriteLine("Time: {0:+hh.mm.ss.ffffzz}", current - previous);
                    previous = current;
#endif
                    if (_delegate != null)
                    {
                        Thread t = new Thread(new ParameterizedThreadStart(_delegate));
                        t.Start(_timeSlice);
                        ///_delegate(_timeSlice);
                        //Console.WriteLine("time: {0:+hh.mm.ss.ffffzz}", ts.ToString());
                    }
                }

                Thread.Sleep((int)(updateIncrement / speedFactor));
                continue;


            }
            //if (_timerThread != null)
            //{
            //    if (_timerThread.IsAlive)
            //    {
            //        _timerThread.Abort();
            //    }
            //}
            try
            {
                Coordinator.debugLogger.Writeline("Metronome", "Timer Thread Ended", "general");
            }
            catch (Exception E)
            { 
            //suppress
            }
        }

    }
}
