namespace Creelio.Framework.WebServices.Dto
{
    using System;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "DateTimeResponse")]
    public class DateTimeResponse : Response
    {
        [DataMember(Name = "Item")]
        public DateTime Item { get; set; }
    }
}