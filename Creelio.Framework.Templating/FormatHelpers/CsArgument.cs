namespace Creelio.Framework.Templating.FormatHelpers
{
    public class CsArgument
    {
        public CsArgument(string name)
            : this(name, null)
        {
        }

        public CsArgument(string name, string modifier)
        {
            Name = name;
            Modifier = modifier;
        }

        public string Name { get; set; }
        
        public string Modifier { get; set; }
    }
}