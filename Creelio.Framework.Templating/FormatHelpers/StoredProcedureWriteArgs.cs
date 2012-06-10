namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;

    [Flags]
    public enum StoredProcedureWriteArgs
    {
        WriteAlter = 1,
        DefaultParamsToNull = 2,
    }
}