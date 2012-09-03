﻿namespace Creelio.Framework.WebServices.Dto
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract(Namespace = "Creelio.Framework.WebServices.DTO", Name = "{0}ListResponse")]
    public class ListResponse<T> : Response
    {
        private IEnumerable<T> _items = null;

        [DataMember(Name = "Items")]
        public IEnumerable<T> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new List<T>();
                }

                return _items;
            }

            set
            {
                _items = value;
            }
        }
    }
}