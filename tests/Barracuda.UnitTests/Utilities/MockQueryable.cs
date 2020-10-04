using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Barracuda.UnitTests.Utilities
{
    public class MockQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
    {
        private readonly IQueryable<T> _queryable;

        public MockQueryable(IQueryable<T> queryable)
        {
            _queryable = queryable;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType => _queryable.ElementType;
        public Expression Expression => _queryable.Expression;
        public IQueryProvider Provider => new MockQueryProvider(_queryable.Provider);
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return new MockAsyncEnumerator<T>(_queryable.GetEnumerator());
        }
    }
    
    public class MockQueryProvider : IQueryProvider
    {
        private readonly IQueryProvider _queryProvider;

        public MockQueryProvider(IQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
            => _queryProvider.CreateQuery(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new MockQueryable<TElement>(_queryProvider.CreateQuery<TElement>(expression));
        }

        public object Execute(Expression expression)
            => _queryProvider.Execute(expression);

        public TResult Execute<TResult>(Expression expression)
            => _queryProvider.Execute<TResult>(expression);
    }
}