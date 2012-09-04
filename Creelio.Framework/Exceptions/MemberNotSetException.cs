namespace Creelio.Framework.Exceptions
{
    using System;

    public class MemberNotSetException : Exception
    {
        public MemberNotSetException(string propertyName)
            : this(propertyName, null, null)
        {            
        }

        public MemberNotSetException(string propertyName, string message)
            : this(propertyName, message, null)
        {
        }

        public MemberNotSetException(string propertyName, string message, Exception innerException)
            : base(
                string.IsNullOrEmpty(propertyName)
                    ? string.Format("One or more required members do not have a value. {0}", message)
                    : string.Format("Must set a value for member \"{0}\". {1}", propertyName, message),
                innerException)
        {
            PropertyName = propertyName;
            Data.Add("PropertyName", propertyName);
        }

        protected MemberNotSetException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }

        public string PropertyName { get; private set; }
    }
}