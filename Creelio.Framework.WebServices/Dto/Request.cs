namespace Creelio.Framework.WebServices.Dto
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "Request")]
    public abstract class Request
    {
        private Dictionary<string, object> _additionalData = null;

        public Request()
        {
        }

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

        /// <summary>
        /// If request is invalid, should throw an Exception.
        /// </summary>
        public virtual void Validate()
        {
        }
    }
}