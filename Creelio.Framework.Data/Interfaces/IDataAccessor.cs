using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Creelio.Framework.Data.Interfaces
{
    public interface IDataAccessor<T> : IDataSelector<T>, IDataModifier<T> where T : new()
    {
    }
}
