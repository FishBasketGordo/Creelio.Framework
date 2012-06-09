namespace Creelio.Framework.Templating.Interfaces
{
    interface ISmoTemplate : ITextTemplateHostProvider
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string TableName { get; set; }
        string OutputFileName { get; }

        void RenderToFile();
    }
}
