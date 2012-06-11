namespace Creelio.Framework.Templating.FormatHelpers
{
    using System;

    public class CsParameter
    {
        public CsParameter(Type type, string name)
            : this(type.FullName, name, null)
        {
        }

        public CsParameter(string typeName, string name)
            : this(typeName, name, null)
        {
        }

        public CsParameter(Type type, string name, string modifier)
            : this(type.FullName, name, modifier)
        {
        }

        public CsParameter(string typeName, string name, string modifier)
        {
            TypeName = typeName;
            Name = name;
            Modifier = modifier;
        }

        public string TypeName { get; set; }
        
        public string Name { get; set; }
        
        public string Modifier { get; set; }
    }
}