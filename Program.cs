using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        private const int LISTS = 50;
        static void Main(string[] args)
        {
            new Program().Run();
        }
        public void Run()
        {
            Console.WriteLine("Start...");
            CalcBySequential();
            CalcByThread();
            CalcByTask();
            Console.ReadLine();
        }
        private void CalcBySequential()
        {
            Stopwatch watch = Stopwatch.StartNew();
            for (int n = 0; n < LISTS; n++)
            {
                DoSomething(null);
            }
            watch.Stop();
            Console.WriteLine("Sequential: {0}ms.", watch.ElapsedMilliseconds);
        }
        private void CalcByThread()
        {
            WaitHandle[] waitHandle = new WaitHandle[LISTS];
            Stopwatch watch = Stopwatch.StartNew();
            for (int n = 0; n < LISTS; n++)
            {
                ManualResetEvent resetEvent = new ManualResetEvent(false);
                waitHandle[n] = resetEvent;
                Thread thread = new Thread(DoSomething);
                thread.Start(resetEvent);
            }
            WaitHandle.WaitAll(waitHandle);
            watch.Stop();
            Console.WriteLine("Thread: {0}ms.", watch.ElapsedMilliseconds);
        }
        private void CalcByTask()
        {
            Task[] tasks = new Task[LISTS];
            Stopwatch watch = Stopwatch.StartNew();
            for (int n = 0; n < LISTS; n++)
            {
                tasks[n] = Task.Factory.StartNew(DoSomething, null);
            }
            Task.WaitAll(tasks);
            watch.Stop();
            Console.WriteLine("Task: {0}ms.", watch.ElapsedMilliseconds);
        }
        private void DoSomething(object o)
        {
            ManualResetEvent resetEvent = null;
            if (o != null) resetEvent = (ManualResetEvent)o;
            double x = 0;
            for (int a = 0; a < 100000; a++)
                x = Math.Log(a) / Math.Sqrt(a - Math.Sin(a));
            if (resetEvent != null) resetEvent.Set();
        }
    }
}
