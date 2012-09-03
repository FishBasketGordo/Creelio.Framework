namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DecimalRequest")]
    public class DecimalRequest : Request
    {
        [DataMember(Name = "Item")]
        public decimal Item { get; set; }
    }
}