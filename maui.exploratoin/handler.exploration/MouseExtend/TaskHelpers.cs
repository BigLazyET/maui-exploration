using Microsoft.UI.Dispatching;
using System.Collections.Concurrent;

namespace handler.exploration.Interfaces
{
    internal static class TaskHelpers
    {
        private static ConcurrentDictionary<(object, object), Timer> dict = new ConcurrentDictionary<(object, object), Timer>();

        //
        // 摘要:
        //     Executes action dueTime after this method was last called with these arguments.
        //
        //
        // 参数:
        //   arg1:
        //     First argument to action
        //
        //   arg2:
        //     Second argument to action
        //
        //   action:
        //     The action which should be called with arg1 and arg2.
        //
        //   dueTime:
        //     Time to wait until the action is called.
        public static void Debounce<T1, T2>(T1 arg1, T2 arg2, Action<T1, T2> action, TimeSpan dueTime)
        {
            (T1, T2) tuple = (arg1, arg2);
            ConcurrentDictionary<(object, object), Timer> concurrentDictionary = dict;
            (T1, T2) tuple2 = tuple;
            concurrentDictionary.AddOrUpdate((tuple2.Item1, tuple2.Item2), ((object, object) key) => new Timer(delegate (object? k)
            {
                (object, object) tuple3 = ((object, object))k;
                var (arg3, arg4) = tuple3;
                dict.TryRemove(tuple3, out var _);
                BeginInvokeOnMainThread(delegate
                {
                    action((T1)arg3, (T2)arg4);
                });
            }, key, dueTime, Timeout.InfiniteTimeSpan), delegate ((object, object) k, Timer oldTimer)
            {
                oldTimer.Change(dueTime, Timeout.InfiniteTimeSpan);
                return oldTimer;
            });
        }

        public static void BeginInvokeOnMainThread(Action action)
        {
            DispatcherQueue obj = DispatcherQueue.GetForCurrentThread() ?? WindowStateManager.Default.GetActiveWindow()?.DispatcherQueue;
            if (obj == null)
            {
                throw new InvalidOperationException("Unable to find main thread.");
            }

            if (!obj.TryEnqueue(DispatcherQueuePriority.Normal, delegate
            {
                action();
            }))
            {
                throw new InvalidOperationException("Unable to queue on the main thread.");
            }
        }

        private static void Log(string msg)
        {
        }
    }
}
