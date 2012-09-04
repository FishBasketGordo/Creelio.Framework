namespace Creelio.Framework.Data.Interfaces
{
    public interface IDataModifier<T> where T : new()
    {
        void Save(ref T entity);

        void Remove(T entity);
    }
}