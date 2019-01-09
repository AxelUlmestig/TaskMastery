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

        // C# can't implicitly convert from Task<A[]> to Task<IEnumerable<A>>
        // without async/await...
        public static async Task<IEnumerable<A>> WhenAll<A>(this IEnumerable<Task<A>> list) =>
            await Task
                .WhenAll(list);

        public static Task WhenAll(this IEnumerable<Task> list) =>
            Task.WhenAll(list);
    }
}
