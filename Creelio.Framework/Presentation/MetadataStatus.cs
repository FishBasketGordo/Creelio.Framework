namespace Creelio.Framework.Core.Presentation
{
    public enum MetadataStatus
    {
        Success,
        Rejected,
        Failed
    }

    public static class MetadataStatusEx
    {
        public static string ToClientSideValue(this MetadataStatus status)
        {
            return status.ToString().ToLower();
        }
    }
}