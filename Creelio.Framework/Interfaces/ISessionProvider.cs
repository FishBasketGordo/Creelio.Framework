namespace Creelio.Framework.Interfaces
{
    public interface ISessionProvider
    {
        object this[string key] { get; set; }
    }
}