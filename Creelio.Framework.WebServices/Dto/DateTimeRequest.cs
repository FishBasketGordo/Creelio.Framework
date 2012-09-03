namespace Creelio.Framework.WebServices.Dto
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DateTimeRequest")]
    public class DateTimeRequest : Request
    {
        [DataMember(Name = "Item")]
        public DateTime Item { get; set; }
    }
}