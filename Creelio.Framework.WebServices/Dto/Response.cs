namespace Creelio.Framework.WebServices.Dto
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "Response")]
    public abstract class Response
    {
        private Dictionary<string, object> _additionalData = null;

        public Response()
        {
        }

        [DataMember(Name = "Metadata", IsRequired = true)]
        public Metadata Metadata { get; set; }

        [DataMember(Name = "AdditionalData", IsRequired = false)]
        public Dictionary<string, object> AdditionalData
        {
            get
            {
                if (_additionalData == null)
                {
                    _additionalData = new Dictionary<string, object>();
                }

                return _additionalData;
            }

            set
            {
                _additionalData = value;
            }
        }
    }
}