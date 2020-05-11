// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.
using CoolParking.BL.Interfaces;
using System.Timers;

namespace CoolParking.BL.Services
{
    public class TimerService : ITimerService
    {
        Timer timer = new Timer();
        public double Interval { get { return timer.Interval; } set { timer.Interval = value; } }

        public event ElapsedEventHandler Elapsed
        {
            add { timer.Elapsed += value; }
            remove { timer.Elapsed -= value; }
        }

        public void Dispose()
        {
            timer.Dispose();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
