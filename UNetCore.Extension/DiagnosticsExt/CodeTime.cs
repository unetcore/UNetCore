using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

public interface IAction
{
    object Execute(int index);

}

public interface IOutputAction
{
    void WriteLine(string msg);

}

public static class CodeTimer
{

    [DllImport("kernel32.dll", SetLastError = true)]

    static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,

       out long lpExitTime, out long lpKernelTime, out long lpUserTime);



    [DllImport("kernel32.dll")]

    static extern IntPtr GetCurrentThread();



    public delegate object ActionDelegate(int index);
    public delegate void OutputActionDelegate(string msg);



    private static long GetCurrentThreadTimes()
    {

        long l;

        long kernelTime, userTimer;

        GetThreadTimes(GetCurrentThread(), out l, out l, out kernelTime,

           out userTimer);

        return kernelTime + userTimer;

    }



    static CodeTimer()
    {

        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

        Thread.CurrentThread.Priority = ThreadPriority.Highest;
    }


    public static void Time(string name, int iteration, ActionDelegate action, OutputActionDelegate actionOut, bool outProbability = false)
    {

        if (String.IsNullOrEmpty(name))
        {

            return;

        }



        if (action == null)
        {

            return;

        }



        //1. Print name 

        actionOut(name);





        // 2. Record the latest GC counts

        //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

        GC.Collect(GC.MaxGeneration);

        int[] gcCounts = new int[GC.MaxGeneration + 1];

        for (int i = 0; i <= GC.MaxGeneration; i++)
        {

            gcCounts[i] = GC.CollectionCount(i);

        }



        // 3. Run action

        Stopwatch watch = new Stopwatch();

        watch.Start();

        long ticksFst = GetCurrentThreadTimes(); //100 nanosecond one tick


        Dictionary<object, int> dicProbability = new Dictionary<object, int>();
        if (outProbability == true)
        {
            object v = null;
            for (int i = 0; i < iteration; i++)
            {

                v = action(i);
                if (v != null)
                {
                    if (dicProbability.ContainsKey(v))
                    {
                        dicProbability[v] += 1;
                    }
                    else
                    {
                        dicProbability.Add(v, 1);
                    }
                }

            }

        }
        else
        {
            for (int i = 0; i < iteration; i++) { action(i); }

        }


        long ticks = GetCurrentThreadTimes() - ticksFst;

        watch.Stop();



        // 4. Print CPU 
        actionOut("\tTime Elapsed:\t\t" +

           watch.ElapsedMilliseconds.ToString("N0") + "ms");

        actionOut("\tTime Elapsed (one time):" +

           (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms");



        actionOut("\tCPU time:\t\t" + (ticks * 100).ToString("N0")

           + "ns");

        actionOut("\tCPU time (one time):\t" + (ticks * 100 /

           iteration).ToString("N0") + "ns");



        // 5. Print GC

        for (int i = 0; i <= GC.MaxGeneration; i++)
        {

            int count = GC.CollectionCount(i) - gcCounts[i];

            actionOut("\tGen " + i + ": \t\t\t" + count);

        }


        // 6. Print probility
        if (outProbability == true)
        {
            actionOut("\tRandCount  : \t\t" + dicProbability.Count);
            int sum = 0;
            foreach (var item in dicProbability.Values)
            {
                sum += item;
            }
            foreach (var item in dicProbability)
            {
                actionOut("\t\t\t RandItem  :" + item.Key.ToString() + "\t Count  :" + item.Value + "\t Probability  :" + Math.Round((double)((double)item.Value / (double)sum), 3));

            }

        }

    }

    public static void Time(string name, int iteration, IAction action, IOutputAction actionOut, bool outProbability = false)
    {

        if (String.IsNullOrEmpty(name))
        {

            return;

        }



        if (action == null)
        {

            return;

        }



        //1. Print name

        actionOut.WriteLine(name);


        // 2. Record the latest GC counts

        //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);

        GC.Collect(GC.MaxGeneration);

        int[] gcCounts = new int[GC.MaxGeneration + 1];

        for (int i = 0; i <= GC.MaxGeneration; i++)
        {

            gcCounts[i] = GC.CollectionCount(i);

        }



        // 3. Run action

        Stopwatch watch = new Stopwatch();

        watch.Start();

        long ticksFst = GetCurrentThreadTimes(); //100 nanosecond one tick


        Dictionary<object, int> dicProbability = new Dictionary<object, int>();
        if (outProbability == true)
        {
            object v = null;
            for (int i = 0; i < iteration; i++)
            {

                v = action.Execute(i);
                if (v != null)
                {
                    if (dicProbability.ContainsKey(v))
                    {
                        dicProbability[v] += 1;
                    }
                    else
                    {
                        dicProbability.Add(v, 1);
                    }
                }

            }

        }
        else
        {
            for (int i = 0; i < iteration; i++) { action.Execute(i); }

        }

        long ticks = GetCurrentThreadTimes() - ticksFst;

        watch.Stop();



        // 4. Print CPU 

        actionOut.WriteLine("\tTime Elapsed:\t\t" +

           watch.ElapsedMilliseconds.ToString("N0") + "ms");

        actionOut.WriteLine("\tTime Elapsed (one time):" +

           (watch.ElapsedMilliseconds / iteration).ToString("N0") + "ms");



        actionOut.WriteLine("\tCPU time:\t\t" + (ticks * 100).ToString("N0")

            + "ns");

        actionOut.WriteLine("\tCPU time (one time):\t" + (ticks * 100 /

            iteration).ToString("N0") + "ns");



        // 5. Print GC

        for (int i = 0; i <= GC.MaxGeneration; i++)
        {

            int count = GC.CollectionCount(i) - gcCounts[i];

            actionOut.WriteLine("\tGen " + i + ": \t\t\t" + count);

        }

        // 6. Print probility
        if (outProbability == true)
        {
            actionOut.WriteLine("\tRandCount  : \t\t" + dicProbability.Count);
            foreach (var item in dicProbability)
            {
                actionOut.WriteLine(item.Key.ToString() + "\t\tRandItem  : \t\t" + item.Value + "\t\tProbability  : \t\t" + Math.Round((double)(item.Value / dicProbability.Count)));

            }

        }



    }

}

