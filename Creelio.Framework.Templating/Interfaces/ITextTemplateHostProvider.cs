using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.Interfaces
{
    public interface ITextTemplateHostProvider
    {
        ITextTemplatingEngineHost Host { get; set; }
    }
}
