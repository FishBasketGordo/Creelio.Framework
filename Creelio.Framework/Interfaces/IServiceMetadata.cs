namespace Creelio.Framework.Interfaces
{
    public interface IServiceMetadata
    {
        MetadataStatus Status { get; }

        string Message { get; }
        
        string UserName { get; }
    }
}