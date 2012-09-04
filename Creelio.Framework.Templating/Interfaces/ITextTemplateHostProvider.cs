namespace Creelio.Framework.Templating.Interfaces
{
    using Microsoft.VisualStudio.TextTemplating;

    public interface ITextTemplateHostProvider
    {
        ITextTemplatingEngineHost Host { get; set; }
    }
}