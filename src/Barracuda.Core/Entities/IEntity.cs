namespace Barracuda.Core
{
    public interface IEntity : IEntity<int>
    {
    }
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}