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
    }
}
