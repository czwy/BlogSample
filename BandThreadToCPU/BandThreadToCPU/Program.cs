using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Management;

namespace BandThreadToCPU
{
    class Program
    {
        //SetThreadAffinityMask: Set hThread run on logical processer(LP:) dwThreadAffinityMask
        [DllImport("kernel32.dll")]
        static extern UIntPtr SetThreadAffinityMask(IntPtr hThread, UIntPtr dwThreadAffinityMask);

        //Get the handler of current thread
        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThread();

        //The real function
        public static void ChangeValue(object lpIdx)
        {
            //Bind current thread to a specific logical processer
            ulong LpId = SetCpuID((int)lpIdx);
            SetThreadAffinityMask(GetCurrentThread(), new UIntPtr(LpId));

            // Let program run for a while
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 1000000; i++)
            {
                for (int j = 0; j < 1000000; j++)
                {
                    int _data = j;
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Running Time: " + stopwatch.ElapsedMilliseconds.ToString());
        }

        //Get CPU id. index:0 -> id:1, 1->2, 2->4, 3->8, 4->16, ...
        static ulong SetCpuID(int lpIdx)
        {
            ulong cpuLogicalProcessorId = 0;
            if (lpIdx < 0 || lpIdx >= System.Environment.ProcessorCount)
            {
                lpIdx = 0;
            }
            cpuLogicalProcessorId |= 1UL << lpIdx;
            return cpuLogicalProcessorId;
        }

        static void Main(string[] args)
        {
            /**
            // Specify the cpu logical processor index you want. CPU lp index start from 0

            // Get the ManagementClass object
            ManagementClass mc = new ManagementClass(new ManagementPath("Win32_Processor"));
            // Get the properties in the class
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                PropertyDataCollection properties = mo.Properties;
                // Print CPU properties
                foreach (PropertyData property in properties)
                {
                    Console.WriteLine(property.Name + ": " + property.Value);
                }
            }
            **/
            Console.Write("选择绑定的CPU核心(从0开始):");
            string core = Console.ReadLine();
            int coreNumber = 0;
            try
            {
                coreNumber = Int32.Parse(core);
            }
            catch
            {
                coreNumber = 0;
            }

            Stopwatch stopwatch = new Stopwatch();
            Thread thread = new Thread(new ParameterizedThreadStart(ChangeValue));
            stopwatch.Start();
            thread.Start(coreNumber);
            thread.Join();
            stopwatch.Stop();
            Console.WriteLine("Total running time: " + stopwatch.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
