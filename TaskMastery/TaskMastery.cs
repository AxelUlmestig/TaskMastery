using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskMastery
{
    public static class TaskMastery
    {
        public static async Task<B> Map<A, B>(this Task<A> task, Func<A, B> f) =>
            f(await task);

        public static async Task<B> FlatMap<A, B>(this Task<A> task, Func<A, Task<B>> f) =>
            await f(await task);

        public static IEnumerable<Task<B>> SelectTasks<A, B>(this IEnumerable<Task<A>> list, Func<A, B> f) =>
            list.Select(task => task.Map(f));

        public static IEnumerable<Task<B>> SelectTasksFlatten<A, B>(this IEnumerable<Task<A>> list, Func<A, Task<B>> f) =>
            list.Select(task => task.FlatMap(f));

        // C# can't implicitly convert from Task<A[]> to Task<IEnumerable<A>>
        // without async/await...
        public static async Task<IEnumerable<A>> WhenAll<A>(this IEnumerable<Task<A>> list) =>
            await Task
                .WhenAll(list);

        public static Task WhenAll(this IEnumerable<Task> list) =>
            Task.WhenAll(list);

        public static async Task<IEnumerable<B>> SelectBatchAsync<A, B>(this IEnumerable<A> list, Func<A, Task<B>> f, int batchSize)
        {
            IEnumerable<B> output = new List<B>();
            var remainingList = list;

            while(remainingList.Any())
            {
                var batch = await remainingList
                    .Take(batchSize)
                    .Select(f)
                    .WhenAll();

                output = output.Concat(batch);
                remainingList = remainingList.Skip(batchSize);
            }

            return output;
        }
    }
}
