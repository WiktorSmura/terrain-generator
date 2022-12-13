using System;
using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
using System.Windows.Forms;

namespace terrain_generator
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string hgtPath = "N50E017.hgt";
            //Tuple<int[], int> dataHgt = Helpers.getData(hgtPath);
            //Console.WriteLine("x:21, y:37, z:{0}", Helpers.getElement(dataHgt.Item1, dataHgt.Item2, new int[] { 21, 37 }));

            // Setup form
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
