namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "Int32Response")]
    public class Int32Response : Response
    {
        [DataMember(Name = "Item")]
        public int Item { get; set; }
    }
}