namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "BooleanRequest")]
    public class BooleanRequest : Request
    {
        [DataMember(Name = "Item")]
        public bool Item { get; set; }
    }
}