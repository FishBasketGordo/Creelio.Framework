namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "BooleanResponse")]
    public class BooleanResponse : Response
    {
        [DataMember(Name = "Item")]
        public bool Item { get; set; }
    }
}