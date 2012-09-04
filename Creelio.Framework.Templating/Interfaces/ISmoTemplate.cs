namespace Creelio.Framework.Templating.Interfaces
{
    public interface ISmoTemplate : ITextTemplateHostProvider
    {
        string ConnectionString { get; set; }
        
        string DatabaseName { get; set; }
        
        string TableName { get; set; }

        string OutputFileName { get; }

        void RenderToFile();
    }
}