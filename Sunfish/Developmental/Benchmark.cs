using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Sunfish
{
    public static class StaticBenchmark
    {
        static Stopwatch Timer = new Stopwatch();
        static string result;

        public static void Begin()
        {
            Timer.Start();
        }
        public static void End()
        {
            Timer.Stop();
            result = Timer.ElapsedMilliseconds.ToString() + " Milliseconds";
            Timer.Reset();
        }
        public static string Result { get { return result; } }

        public static new string ToString()
        {
            return Result;
        }
    }

    public class Benchmark
    {
        Stopwatch Timer = new Stopwatch();
        string result;

        public void Begin()
        {
            Timer.Start();
        }
        public void End()
        {
            Timer.Stop();
            result = Timer.ElapsedMilliseconds.ToString() + " ms";
            Timer.Reset();
        }
        public string Result { get { return result; } }

        public new string ToString()
        {
            return Result;
        }
    }
}
