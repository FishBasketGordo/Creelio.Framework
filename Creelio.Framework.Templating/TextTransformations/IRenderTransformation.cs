using System;

namespace Creelio.Framework.Templating.TextTransformations
{
    public interface IRenderTransformation<in TRenderArgs> where TRenderArgs : IRenderArgs
    {
        string Render(TRenderArgs args);
    }
}
