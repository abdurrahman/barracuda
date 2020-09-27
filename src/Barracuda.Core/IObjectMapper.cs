namespace Barracuda.Core
{
    public interface IObjectMapper
    {
        TDestination MapTo<TSource, TDestination>(TSource source, TDestination destination);

        TDestination MapTo<TDestination>(object source);
    }
}