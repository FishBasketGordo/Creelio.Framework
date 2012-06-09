using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.TextTransformations
{
    public abstract class MultipleOutputTransformation : TextTransformation
    {
        public void RenderMultiple<TRenderArgs>(IRenderTransformation<TRenderArgs> transformation, IEnumerable<TRenderArgs> argsList)
            where TRenderArgs : IRenderArgs
        {
            var engine = new Engine();

            foreach (var args in argsList)
            {
                var rendered = transformation.Render(args);
                throw new Exception(rendered);
            }
        }
    }
}
