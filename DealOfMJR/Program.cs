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
            var test = Directory.GetFiles("/test");
            if (test.Length > 0)
            {
                test.ToList().ForEach(x => 
                { 
                    Console.WriteLine($"{x}");
                    if (x.Contains("video.mjr"))
                    {
                        Deal($"{x} {x.Substring(0,x.Length-3)}webm");
                    }
                    else if (x.Contains("audio.mjr"))
                    {
                        Deal($"{x} {x.Substring(0, x.Length - 3)}opus");
                    }
                    
                });
            }

            Console.WriteLine("Hello World!");

            
        }
        static void Deal(string args) 
        {
            #region process 测试
            var psi = new ProcessStartInfo("janus-pp-rec", args) { RedirectStandardOutput = true };
            Console.WriteLine($"janus-pp-rec {args}");
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
                //Console.WriteLine($"Total execute time :{(proc.ExitTime - proc.StartTime).TotalMilliseconds} ms");
                //Console.WriteLine($"Exited Code ： {proc.ExitCode}");
            }
            #endregion
        }
    }
}
