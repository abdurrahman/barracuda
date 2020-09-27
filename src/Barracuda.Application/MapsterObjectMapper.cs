using Barracuda.Core;
using Mapster;

namespace Barracuda.Application
{
    public class MapsterObjectMapper : IObjectMapper
    {
        public TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }

        public TDestination MapTo<TDestination>(object source)
        {
            return source.Adapt<TDestination>();
        }
    }
}