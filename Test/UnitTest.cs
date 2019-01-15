using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TaskMastery;

namespace TaskMastery
{
    public class UnitTest
    {
        [Fact]
        public async Task WhenAll()
        {
            var x = await new List<int> { 1, 2, 3 }
                .Select(Task.FromResult)
                .WhenAll();

            Assert.Equal(x, new List<int> { 1, 2, 3 });
        }

        [Fact]
        public async Task SelectTasks()
        {
            var x = await new List<int> { 1, 2, 3 }
                .Select(Task.FromResult)
                .SelectTasks(n => n + 1)
                .WhenAll();

            Assert.Equal(x, new List<int> { 2, 3, 4 });
        }

        [Fact]
        public async Task SelectTasksFlatten()
        {
            var x = await new List<int> { 1, 2, 3 }
                .Select(Task.FromResult)
                .SelectTasksFlatten(Task.FromResult)
                .WhenAll();

            Assert.Equal(x, new List<int> { 1, 2, 3 });
        }

        [Fact]
        public async Task SelectBatchAsync2()
        {
            IEnumerable<int> x = await Enumerable
                .Range(0, 13)
                .SelectBatchAsync(async n => {
                    await Task.Delay(1000);
                    Console.WriteLine(n);
                    return n + 1;
                }, 3);

            IEnumerable<int> expected = Enumerable
                .Range(1, 13);

            Assert.Equal(expected, x);
        }

        [Fact]
        public async Task WhenAllBatched()
        {
            IEnumerable<int> x = await Enumerable
                .Range(0, 13)
                .Select(async n => {
                    await Task.Delay(1000);
                    return n + 1;
                })
                .WhenAllBatched(3);

            IEnumerable<int> expected = Enumerable
                .Range(1, 13);

            Assert.Equal(expected, x);
        }
    }
}
