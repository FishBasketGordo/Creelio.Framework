namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "StringResponse")]
    public class StringResponse : Response
    {
        [DataMember(Name = "Item")]
        public string Item { get; set; }
    }
}