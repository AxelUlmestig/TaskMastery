using System;
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

        public static Task<List<A>> WhenAll<A>(this List<Task<A>> list) =>
            Task
                .WhenAll(list)
                .Map(result => new List<A>(result));

        public static Task WhenAll(this List<Task> list) =>
            Task.WhenAll(list);
    }
}
