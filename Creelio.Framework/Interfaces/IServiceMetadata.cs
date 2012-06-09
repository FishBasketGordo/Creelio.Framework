using Creelio.Framework.Core.Presentation;

namespace Creelio.Framework.Core.Interfaces
{
    public interface IServiceMetadata
    {
        MetadataStatus Status { get; }
        string Message { get; }
        string UserName { get; }
    }
}