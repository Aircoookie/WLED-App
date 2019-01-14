using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WLED
{
    class RateLimitedSender
    {
        private static Timer timer;
        private static WLEDDevice target;
        static string toSend;
        static bool alreadySent = true;

        static RateLimitedSender()
        {
            timer = new Timer(200);
            timer.Elapsed += OnWaitPeriodOver;
        }

        public static void SendAPICall(WLEDDevice t, string call)
        {
            alreadySent = false;
            if (timer.Enabled)
            {
                //save to send when waiting period over
                target = t;
                toSend = call;
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
