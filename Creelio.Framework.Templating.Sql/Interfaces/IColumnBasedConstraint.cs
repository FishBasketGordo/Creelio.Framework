namespace Creelio.Framework.Templating.Sql
{
    using System;

    public interface IColumnBasedConstraint
    {
        Column Column { get; }
    }
}
