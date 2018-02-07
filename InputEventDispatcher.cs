using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KeyReplace
{
    public class InputEventDispatcher 
    {
        static InputEventDispatcher()
        {
            event_thread = new Thread(eventsender);
            event_thread.Start();
        }

        public static void DisposeIt()
        {
            event_thread.Abort();
        }

        static Thread event_thread = null;

        static List<INPUT> input_queue = new List<INPUT>();
        static List<INPUT> output_queue = new List<INPUT>();
        static object bufferlock = new object();

        public static void EnqueueEvents(INPUT[] inputs)
        {
            lock(bufferlock)
            {
                input_queue.AddRange(inputs);
                Monitor.Pulse(bufferlock);
            }
        }
        static Stopwatch lastEventTimer = new Stopwatch();
        static void eventsender()
        {
            while(true)
            {
                lock (bufferlock)
                {
                    if (input_queue.Count == 0)
                        Monitor.Wait(bufferlock); // Release and wait to be pulsed so we can re-acquire.

                    output_queue.AddRange(input_queue);
                    input_queue.Clear();
                }

                // Flush the output queue
                while(output_queue.Count != 0)
                {
                    // If the KeyUp bit is not set, then this is a KeyDown event.
                    bool isDown = (output_queue[0].Data.Keyboard.Flags & (uint)KeyboardFlag.KeyUp) == 0;

                    // If the last event was > 200ms ago, we dispatch the event immediately.
                    if(lastEventTimer.ElapsedMilliseconds < 200)
                    {
                        // Perform the delay
                        AccurateTimer.AccurateSleep(isDown ? RandSleepDown() : RandSleepUp());
                        // Todo - maybe dont delay the up events huh
                    }

                    // Dispatch the event actually now
                    InputSender.DispatchInput(new INPUT[] { output_queue[0] });

                    // Remove this ( we should really use a queue because this has to shift all those elements over )
                    output_queue.RemoveAt(0);

                    // Restart the 'last event timer'
                    lastEventTimer.Restart();
                }
            }
           
        }
        // Eh maybe i'll just get used to it.

        // But basically, 
        // we keep a queue of keyinputevents, and if it's a keypress event, add a down event, and an up event and delay it by the length.
        // if the queue is empty, we do not pre-delay it. otherwise we pre-delay it depending on the type of event.
        // now, the problem arises when we want to insert an event inbetween two events... this is confusing, should we sort the list? lol.

        // So the solution is to use a SortedList, by the outputtime function, which is a timestamp relative to the start of the application.

        // Each time we add an InputEvent, if the queue is empty, add it as-is. Otherwise:
        // - If the InputEvent is a KeyPress, add a down-event with a corresponding up event and a length delay. Move the 
        // - If the InputEvent is a KeyDown, add a down-event with a downkey delay
        // - If the InputEvent is a KeyUp, add an up-event with an upkey delay
        // Now, suppose your queue has a bunch of events in it. How do we figure out the 'last' time of the entire queue, to figure out 
        // what time to start counting at?

        // The solution is to introduce a queue_max_time variable.
        // If the queue is not empty and the current time > queue_max_time, then we'd have an issue.
        // How do we deal with events that are just stacked up and not executed?
        // We can store relative delays to the next event, but the issue arises when we need to append an event in-between events.

        // Options for storing time-stamps:
        // Option 1. Store relative time to next event only. 
        // Problem: how to insert events inbetween? Re-order events?
        //

        // Option 2. Store absolute times and on dequeue, calculate the difference. 
        // Problem: what if events stack up, and the difference becomes negative?
        // Benefits: Able to shuffle events sorta

        // Option 3. Don't store the times at all, and just apply the delays when dequeueing.
        // Problem: Boring in-order execution, and no support for keypress events where one key may be released before the other at varying times.
        // Benefits: simple implementation, basically what we had before.

        static Random rand = new Random();
        /*
         * https://blogs.msdn.microsoft.com/oldnewthing/20121101-00/?p=6193
         * */
        static double[][] sleepsDown =
        {  new double[]{ 5.481, 9.688, 25 }, // mean, stdDev, skew
              new double[] { 63.308, 20, -1.758 },
             new double[]  { 82.813, 12.891, -0.195 }
        };
        static double[] selDown = { 1.265, 2 }; // mean, stdDev

        static double[][] sleepsUp =
         {
            new double[]{ 8.594, 8.203, 4.102 }, //mean, stdDev, skew
              new double[] { 68.75, 6.641,0.977 },
             new double[]  { 68.75, 19.531, -8.203 }
        };
        static double[] selUp = { -0.612, 4.898 }; // mean, stdDev

        static int RandSleepDown()
        {
            int pick = (int)Math.Min(Math.Abs(Math.Round(GaussRandom(selDown[0], selDown[1]))), 2);

            return (int)Math.Abs(Math.Round(
                GaussRandomSkewed(sleepsDown[pick][0], sleepsDown[pick][1], sleepsDown[pick][2])
                / 1.6 + 4
                ));
        }

        static int RandSleepUp()
        {
            int pick = (int)Math.Min(Math.Abs(Math.Round(GaussRandom(selUp[0], selUp[1]))), 2);

            return (int)Math.Abs(Math.Round(
                GaussRandomSkewed(sleepsUp[pick][0], sleepsUp[pick][1], sleepsUp[pick][2])
                / 1.5 + 6
                ));
        }

        //courtesy http://azzalini.stat.unipd.it/SN/sn-random.html
        static double GaussRandomSkewed(double mean, double std, double shape)
        {
            double u1 = GaussRandom(0, 1);
            double u2 = GaussRandom(0, 1);

            double z = (u2 > shape * u1) ? -u1 : u1;

            return Math.Max(0, mean + std * z);
        }

        //https://stackoverflow.com/questions/218060/random-gaussian-variables
        //Box-Muller transform
        static double GaussRandom(double mean, double stdDev)
        {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }

        
    }
}
