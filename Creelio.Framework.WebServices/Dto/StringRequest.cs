namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "StringRequest")]
    public class StringRequest : Request
    {
        public StringRequest()
        {
        }

        [DataMember(Name = "Item")]
        public string Item { get; set; }
    }
}