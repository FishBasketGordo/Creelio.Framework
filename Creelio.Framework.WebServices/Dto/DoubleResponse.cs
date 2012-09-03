namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DoubleResponse")]
    public class DoubleResponse : Response
    {
        [DataMember(Name = "Item")]
        public double Item { get; set; }
    }
}