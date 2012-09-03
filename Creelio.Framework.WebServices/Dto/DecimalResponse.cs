namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DecimalResponse")]
    public class DecimalResponse : Response
    {
        [DataMember(Name = "Item")]
        public decimal Item { get; set; }
    }
}