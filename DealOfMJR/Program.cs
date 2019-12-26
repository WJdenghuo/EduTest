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
            var test = Directory.GetFiles("/test").Where(x=>Path.GetExtension(x)==".mjr").ToList();
            if (test.Count > 0)
            {
                test.ForEach(x => 
                { 
                    Console.WriteLine($"{x}");
                    if (x.Contains("video.mjr"))
                    {
                        Deal("janus-pp-rec",$"{x} {x.Substring(0,x.Length-3)}webm");
                    }
                    else if (x.Contains("audio.mjr"))
                    {
                        Deal("janus-pp-rec", $"{x} {x.Substring(0, x.Length - 3)}opus");
                    }
                });
            }
            test.Where(x => x.Contains("video.mjr")).ToList().ForEach(x =>
              {
                  Deal("ffmpeg", $"-i {x.Substring(0,x.Length-9)}audio.opus -i {x.Substring(0, x.Length - 3)}webm  -c:v copy -c:a opus -strict experimental {x.Substring(0, x.Length - 4)}-hasDeal.webm");
              });
            Console.WriteLine("Hello World!");

            
        }
        static void Deal(String fileName,string args) 
        {
            #region process 测试
            var psi = new ProcessStartInfo(fileName, args) { RedirectStandardOutput = true };
            Console.WriteLine($"{fileName} {args}");
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
