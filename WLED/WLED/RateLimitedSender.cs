using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WLED
{
    //When updating a brightness slider, only send 4 API calls per second as to not overload network or WLED light
    class RateLimitedSender
    {
        private static Timer timer;
        private static WLEDDevice target;
        static string toSend;
        static bool alreadySent = true;

        static RateLimitedSender()
        {
            timer = new Timer(250);
            timer.Elapsed += OnWaitPeriodOver;
        }

        public static void SendAPICall(WLEDDevice t, string call)
        {
            if (timer.Enabled)
            {
                //Save to send once waiting period over
                target = t;
                toSend = call;
                alreadySent = false;
                return;
            }
            timer.Start();
            t?.SendAPICall(call);
            alreadySent = true;
        }

        private static void OnWaitPeriodOver(Object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            if (!alreadySent)
            {
                target?.SendAPICall(toSend);
                alreadySent = true;
                timer.Start();
            }
        }
    }
}
