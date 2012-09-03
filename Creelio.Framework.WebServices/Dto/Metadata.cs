namespace Creelio.Framework.WebServices.Dto
{
    using System;
    using System.Runtime.Serialization;
    using Creelio.Framework.Interfaces;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "Metadata")]
    public class Metadata : IServiceMetadata
    {
        public Metadata()
            : this(MetadataStatus.Success, string.Empty)
        {
        }

        public Metadata(MetadataStatus status)
            : this(status, string.Empty)
        {
        }

        public Metadata(MetadataStatus status, string message)
        {
            Status = status;
            Message = message;
            UserName = Environment.UserName;
        }

        [DataMember(Name = "Message")]
        public string Message { get; set; }

        [DataMember(Name = "Status", EmitDefaultValue = true)]
        public MetadataStatus Status { get; set; }

        [DataMember(Name = "UserName", EmitDefaultValue = true)]
        public string UserName { get; set; }
    }
}