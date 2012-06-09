using System;

namespace Creelio.Framework.Templating.FormatHelpers
{
    public class Parameter
    {
        #region Constructors

        public Parameter(Type type, string name)
            : this(type.FullName, name, null)
        {
        }

        public Parameter(string typeName, string name)
            : this(typeName, name, null)
        {
        }

        public Parameter(Type type, string name, string modifier)
            : this(type.FullName, name, modifier)
        {
        }

        public Parameter(string typeName, string name, string modifier)
        {
            TypeName = typeName;
            Name = name;
            Modifier = modifier;
        }

        #endregion

        #region Properties

        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Modifier { get; set; }

        #endregion
    }
}