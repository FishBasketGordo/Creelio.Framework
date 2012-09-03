namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "Int32Request")]
    public class Int32Request : Request
    {
        [DataMember(Name = "Item")]
        public int Item { get; set; }
    }
}