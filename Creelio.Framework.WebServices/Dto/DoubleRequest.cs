namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DoubleRequest")]
    public class DoubleRequest : Request
    {
        [DataMember(Name = "Item")]
        public double Item { get; set; }
    }
}