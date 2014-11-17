using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Joint_project_1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            //create the form and show it
            Form1 form = new Form1();
            form.Show();
            //store time information for controlling the speed of the loop
            DateTime lastTime = new DateTime();
            DateTime currentTime = new DateTime();
            TimeSpan t = new TimeSpan();

            lastTime = DateTime.Now;

            //loop while the form is running
            while (form.Created)
            {
                currentTime = DateTime.Now;
                //get the number of milliseconds passed since lastTime
                t = currentTime - lastTime;
                if (t.TotalMilliseconds > 10)
                {

                    //process all the events (Mouseclicks, buttons etc)
                    Application.DoEvents();

                    //update our game world
                    form.UpdateWorld();

                    //force a redraw
                    form.Invalidate();


                    //reset lastTime for timing info
                    lastTime = DateTime.Now;
                }

            }
        }
    }
}
