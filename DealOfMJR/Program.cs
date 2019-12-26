using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DealOfMJR
{
    class Program
    {
        static void Main(string[] args)
        {
            var test = Directory.GetFiles("/test/");
            if (test.Length > 0)
            {
                test.ToList().ForEach(x => Console.WriteLine($"{x}"));
            }

            Console.WriteLine("Hello World!");

            #region process 测试
            var psi = new ProcessStartInfo("janus-pp-rec") { RedirectStandardOutput = true };
            //启动
            var proc = Process.Start(psi);
            if (proc == null)
            {
                Console.WriteLine("Can not exec.");
            }
            else
            {
                Console.WriteLine("-------------Start read standard output--------------");
                //开始读取
                using (var sr = proc.StandardOutput)
                {
                    while (!sr.EndOfStream)
                    {
                        Console.WriteLine(sr.ReadLine());
                    }

                    if (!proc.HasExited)
                    {
                        proc.Kill();
                    }
                }
                Console.WriteLine("---------------Read end------------------");
                Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
                Console.WriteLine($"Exited Code ： {proc.ExitCode}");
                #endregion
            }
        }
    }
}
