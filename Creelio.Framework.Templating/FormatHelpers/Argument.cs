namespace Creelio.Framework.Templating.FormatHelpers
{
    public class Argument
    {
        #region Constructors

        public Argument(string name)
            : this(name, null)
        {
        }

        public Argument(string name, string modifier)
        {
            Name = name;
            Modifier = modifier;
        }

        #endregion

        #region Properties

        public string Name { get; set; }
        public string Modifier { get; set; }

        #endregion
    }
}