using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Creelio.Framework.Templating.TextTransformations
{
    public class SmoRenderArgs : IRenderArgs
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
