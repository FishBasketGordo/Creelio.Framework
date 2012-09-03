namespace Creelio.Framework.WebServices.Dto
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "{0}Response")]
    public class ItemResponse<T> : Response
    {
        private T _item = default(T);

        [DataMember(Name = "Item")]
        public T Item
        {
            get
            {
                return _item;
            }

            set
            {
                _item = value;
            }
        }
    }
}