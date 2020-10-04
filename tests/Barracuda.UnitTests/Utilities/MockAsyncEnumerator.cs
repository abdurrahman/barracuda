using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barracuda.UnitTests.Utilities
{
    public class MockAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public MockAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public async ValueTask DisposeAsync()
        {
            _enumerator.Dispose();
            await new ValueTask(Task.CompletedTask);
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await new ValueTask<bool>(Task.FromResult(_enumerator.MoveNext()));
        }

        public T Current => _enumerator.Current;
    }
}