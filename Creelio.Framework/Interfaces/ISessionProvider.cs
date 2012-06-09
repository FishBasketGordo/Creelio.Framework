namespace Creelio.Framework.Core.Interfaces
{
    public interface ISessionProvider
    {
        object this[string key] { get; set; }
    }
}