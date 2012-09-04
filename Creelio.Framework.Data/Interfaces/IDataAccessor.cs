namespace Creelio.Framework.Data.Interfaces
{
    public interface IDataAccessor<T> : IDataSelector<T>, IDataModifier<T> where T : new()
    {
    }
}