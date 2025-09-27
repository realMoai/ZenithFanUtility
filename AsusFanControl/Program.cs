using System;
using System.Collections.Generic;

namespace AsusFanControl
{
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: AsusFanControl <args>");
                Console.WriteLine("\t--get-fan-speeds");
                Console.WriteLine("\t--set-fan-speeds=0-100 (percent value, 0 for turning off test mode)");
                Console.WriteLine("\t--get-fan-count");
                Console.WriteLine("\t--get-fan-speed=fanId (comma separated)");
                Console.WriteLine("\t--set-fan-speed=fanId:0-100 (comma separated, percent value, 0 for turning off test mode)");
                Console.WriteLine("\t--get-cpu-temp");
                Console.WriteLine("\t--get-gpu-ts1l-temp");
                Console.WriteLine("\t--get-gpu-ts1r-temp");
                Console.WriteLine("\t--get-highest-gpu-temp");
                return 1;
            }
            AsusControl asusControl = new AsusControl();
            foreach (string str1 in args)
            {
                if (str1.StartsWith("--get-fan-speeds"))
                    Console.WriteLine("Current fan speeds: " + string.Join<int>(" ", (IEnumerable<int>)asusControl.GetFanSpeeds()) + " RPM");
                if (str1.StartsWith("--set-fan-speeds"))
                {
                    int percent = int.Parse(str1.Split('=')[1]);
                    asusControl.SetFanSpeedsAsync(percent).Wait();
                    if (percent == 0)
                        Console.WriteLine("Test mode turned off");
                    else
                        Console.WriteLine(string.Format("New fan speeds: {0}%", (object)percent));
                }
                if (str1.StartsWith("--get-fan-speed="))
                {
                    string str2 = str1.Split('=')[1];
                    char[] chArray = new char[1] { ',' };
                    foreach (string s in str2.Split(chArray))
                    {
                        int fanIndex = int.Parse(s);
                        int fanSpeed = asusControl.GetFanSpeed((byte)fanIndex);
                        Console.WriteLine(string.Format("Current fan speed for fan {0}: {1} RPM", (object)fanIndex, (object)fanSpeed));
                    }
                }
                if (str1.StartsWith("--get-fan-count"))
                    Console.WriteLine(string.Format("Fan count: {0}", (object)asusControl.HealthyTable_FanCounts()));
                if (str1.StartsWith("--set-fan-speed="))
                {
                    string str3 = str1.Split('=')[1];
                    char[] chArray = new char[1] { ',' };
                    foreach (string str4 in str3.Split(chArray))
                    {
                        int fanIndex = int.Parse(str4.Split(':')[0]);
                        int percent = int.Parse(str4.Split(':')[1]);
                        asusControl.SetFanSpeed(percent, (byte)fanIndex);
                        if (percent == 0)
                            Console.WriteLine(string.Format("Test mode turned off for fan {0}", (object)fanIndex));
                        else
                            Console.WriteLine(string.Format("New fan speed for fan {0}: {1}%", (object)fanIndex, (object)percent));
                    }
                }
                if (str1.StartsWith("--get-cpu-temp"))
                    Console.WriteLine(string.Format("Current CPU temp: {0}", (object)asusControl.Thermal_Read_Cpu_Temperature()));
                if (str1.StartsWith("--get-gpu-ts1l-temp"))
                    Console.WriteLine(string.Format("Current GPU TS1L temp: {0}", (object)asusControl.Thermal_Read_GpuTS1L_Temperature()));
                if (str1.StartsWith("--get-gpu-ts1r-temp"))
                    Console.WriteLine(string.Format("Current GPU TS1R temp: {0}", (object)asusControl.Thermal_Read_GpuTS1R_Temperature()));
                if (str1.StartsWith("--get-highest-gpu-temp"))
                    Console.WriteLine(string.Format("Current Highest GPU temp: {0}", (object)asusControl.Thermal_Read_Highest_Gpu_Temperature()));
            }
            return 0;
        }
    }
}
