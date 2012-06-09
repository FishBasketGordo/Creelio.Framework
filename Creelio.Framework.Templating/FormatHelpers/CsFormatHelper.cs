using System;
using System.Collections.Generic;
using System.Linq;
using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.FormatHelpers
{
    public class CsFormatHelper : FormatHelper
    {
        #region Fields

        /// <remarks>
        /// The order of the items in this array is important. 
        /// It is used by the method GetLeastRestrictiveModifier. 
        /// 
        /// Do not change it.
        /// </remarks>
        private static readonly List<string> _accessModifiers = new List<string> 
        { 
            "public", "protected internal", "internal", "protected", "private"
        };

        #endregion

        #region Constructors

        public CsFormatHelper(TextTransformation textTransformation)
            : base(textTransformation)
        {
        }

        #endregion

        #region Methods

        #region WriteDisclaimer

        protected override IEnumerable<string> FormatDisclaimerLines(IEnumerable<string> disclaimerLines)
        {
            return from line in disclaimerLines
                   select string.Format("// {0}", line);
        }

        #endregion

        #region WriteRegion

        public void BeginWriteRegion()
        {
            BeginWriteRegion(null);
        }

        public void BeginWriteRegion(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
            {
                _tt.WriteLine("#region");
            }
            else
            {
                _tt.WriteLine("#region {0}", regionName);
            }
        }

        public void EndWriteRegion()
        {
            _tt.WriteLine("#endregion");
        }

        #endregion

        #region WriteClass

        public void BeginWriteClass(string className)
        {
            BeginWriteClass(className, "public");
        }

        public void BeginWriteClass(string className, string accessModifier)
        {
            BeginWriteClass(className, accessModifier, (string)null);
        }

        public void BeginWriteClass(string className, string accessModifier, string inheritsFrom)
        {
            BeginWriteClass(className, accessModifier, new List<string> { inheritsFrom });
        }

        public void BeginWriteClass(string className, string accessModifier, IEnumerable<string> inheritsFrom)
        {
            BeginWriteClass(className, accessModifier, inheritsFrom, (string)null);
        }

        public void BeginWriteClass(string className, string accessModifier, string inheritsFrom, string genericContraint)
        {
            BeginWriteClass(className, accessModifier, new List<string> { inheritsFrom }, genericContraint);
        }

        public void BeginWriteClass(string className, string accessModifier, IEnumerable<string> inheritsFrom, string genericContraint)
        {
            BeginWriteClass(className, accessModifier, inheritsFrom, new List<string> { genericContraint });
        }

        public void BeginWriteClass(string className, string accessModifier, string inheritsFrom, IEnumerable<string> genericContraints)
        {
            BeginWriteClass(className, accessModifier, new List<string> { inheritsFrom }, genericContraints);
        }

        public void BeginWriteClass(string className, string accessModifier, IEnumerable<string> inheritsFrom, IEnumerable<string> genericContraints)
        {
            ProcessClassName(ref className);

            bool isStatic = false;
            ProcessAccessModifier(ref accessModifier, ref isStatic);

            if (isStatic)
            {
                accessModifier += " static";
            }

            ProcessEnumerable(ref inheritsFrom);
            ProcessEnumerable(ref genericContraints);

            _tt.Write("{0} class {1}", accessModifier, className);
            if (inheritsFrom != null)
            {
                WriteInheritsFrom(inheritsFrom);
            }

            if (genericContraints != null)
            {
                WriteGenericConstraints(genericContraints);
            }
            else
            {
                _tt.WriteLine();
            }

            _tt.WriteLine("{");
        }

        public void EndWriteClass()
        {
            _tt.WriteLine("}");
        }

        private void WriteInheritsFrom(IEnumerable<string> inheritsFrom)
        {
            var baseList = GetCsv(inheritsFrom, GetProcessedTypeName);
            _tt.Write(" : {0}", baseList);
        }

        private void WriteGenericConstraints(IEnumerable<string> genericContraints)
        {
            _tt.WriteLine();

            foreach (var genericConstraint in genericContraints)
            {
                var processed = GetProcessedGenericConstaint(genericConstraint);

                _tt.PushIndent();
                _tt.WriteLine(processed);
                _tt.PopIndent();
            }
        }

        #endregion

        #region WriteField

        public void WriteField(string fieldName, string typeName)
        {
            WriteField(fieldName, typeName, "private");
        }

        public void WriteField(string fieldName, string typeName, string accessModifier)
        {
            WriteField(fieldName, typeName, accessModifier, GetDefaultValue(typeName));
        }

        public void WriteField(string fieldName, string typeName, string accessModifier, string defaultValue)
        {
            ProcessFieldName(ref fieldName);
            ProcessTypeName(ref typeName, ref defaultValue);

            bool isStatic = false;
            bool isReadonly = false;
            bool isConst = false;
            ProcessAccessModifier(ref accessModifier, ref isStatic, ref isReadonly, ref isConst);

            if (isStatic)
            {
                accessModifier += " static";
            }

            if (isReadonly)
            {
                accessModifier += " readonly";
            }

            if (isConst)
            {
                accessModifier += " const";
            }

            _tt.WriteLine("{0} {1} {2} = {3};", accessModifier, typeName, fieldName, defaultValue);
        }

        #endregion

        #region WriteAutoProperty

        public void WriteAutoProperty(string propertyName, string typeName)
        {
            WriteAutoProperty(propertyName, typeName, "public");
        }

        public void WriteAutoProperty(string propertyName, string typeName, string accessModifier)
        {
            WriteAutoProperty(propertyName, typeName, accessModifier, accessModifier);
        }

        public void WriteAutoProperty(string propertyName, string typeName, string getAccessModifier, string setAccessModifier)
        {
            ProcessPropertyName(ref propertyName);
            ProcessTypeName(ref typeName);

            string primaryAccessModifier;
            ProcessPropertyAccessModifiers(ref getAccessModifier, ref setAccessModifier, out primaryAccessModifier);

            var get = GetPropertyAccessorSpecifier("get", getAccessModifier);
            var set = GetPropertyAccessorSpecifier("set", setAccessModifier);

            _tt.WriteLine("{0} {1} {2} {{ {3}; {4}; }}", primaryAccessModifier, typeName, propertyName, get, set);
        }

        #endregion

        #region WritePropertyWithField

        public void WritePropertyWithField(string propertyName, string typeName)
        {
            WritePropertyWithField(propertyName, typeName, "public");
        }

        public void WritePropertyWithField(string propertyName, string typeName, string accessModifier)
        {
            WritePropertyWithField(propertyName, typeName, accessModifier, accessModifier, GetDefaultValue(typeName));
        }

        public void WritePropertyWithField(string propertyName, string typeName, string accessModifier, string defaultValue)
        {
            WritePropertyWithField(propertyName, accessModifier, accessModifier, defaultValue);
        }

        public void WritePropertyWithField(string propertyName, string typeName, string getAccessModifier, string setAccessModifier, string defaultValue)
        {
            ProcessPropertyName(ref propertyName);
            ProcessTypeName(ref typeName, ref defaultValue);

            string primaryAccessModifier; bool isStatic;
            ProcessPropertyAccessModifiers(ref getAccessModifier, ref setAccessModifier, out primaryAccessModifier, out isStatic);

            var fieldName = string.Format("_field_{0}", propertyName);
            var fieldAccessModifier = isStatic ? "private static" : "private";
            var get = GetPropertyAccessorSpecifier("get", getAccessModifier);
            var set = GetPropertyAccessorSpecifier("set", setAccessModifier);

            _tt.WriteLine("{0} {1} {2} = {3};", fieldAccessModifier, typeName, fieldName, defaultValue);
            _tt.WriteLine("{0} {1} {2}", primaryAccessModifier, typeName, propertyName);
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine(get);
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine("return {0};", fieldName);
            _tt.PopIndent();
            _tt.WriteLine("}");
            _tt.WriteLine(set);
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine("{0} = value;", fieldName);
            _tt.PopIndent();
            _tt.WriteLine("}");
            _tt.PopIndent();
            _tt.WriteLine("}");
        }

        #endregion

        #region WriteLazyLoadProperty

        public void WriteLazyLoadProperty(string propertyName, string typeName)
        {
            WriteLazyLoadProperty(propertyName, "public");
        }

        public void WriteLazyLoadProperty(string propertyName, string typeName, string accessModifier)
        {
            var defaultValue = string.Format("new {0}()", typeName);
            WriteLazyLoadProperty(propertyName, accessModifier, defaultValue);
        }

        public void WriteLazyLoadProperty(string propertyName, string typeName, string accessModifier, string defaultValue)
        {
            ProcessPropertyName(ref propertyName);
            ProcessTypeName(ref typeName, ref defaultValue);

            bool isStatic = false;
            ProcessAccessModifier(ref accessModifier, ref isStatic);

            var fieldName = string.Format("_field_{0}", propertyName);
            var fieldAccessModifier = isStatic ? "private static" : "private";

            _tt.WriteLine("{0} {1} {2} = null;", fieldAccessModifier, typeName, fieldName);
            _tt.WriteLine("{0} {1} {2}", accessModifier, typeName, propertyName);
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine("get");
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine("if ({0} == null)", fieldName);
            _tt.WriteLine("{");
            _tt.PushIndent(1);
            _tt.WriteLine("{0} = {1};", fieldName, defaultValue);
            _tt.PopIndent();
            _tt.WriteLine("}");
            _tt.WriteLine("");
            _tt.WriteLine("return {0};", fieldName);
            _tt.PopIndent();
            _tt.WriteLine("}");
            _tt.PopIndent();
            _tt.WriteLine("}");
        }

        #endregion

        #region WriteConstructor

        public void BeginWriteConstructor(string className)
        {
            BeginWriteConstructor(className, "public");
        }

        public void BeginWriteConstructor(string className, string accessModifier)
        {
            BeginWriteConstructor(className, accessModifier, null);
        }

        public void BeginWriteConstructor(string className, string accessModifier, ParameterList parameters)
        {
            BeginWriteConstructor(className, accessModifier, parameters, null);
        }

        public void BeginWriteConstructor(string className, string accessModifier, ParameterList parameters, ArgumentList baseArguments)
        {
            ProcessClassName(ref className);
            ProcessConstructorAccessModifier(ref accessModifier);

            var formattedParameters = GetFormattedEnumerable(parameters, GetFormattedParameter);
            var formattedArguments = GetFormattedEnumerable(baseArguments, GetFormattedArgument);

            _tt.Write("{0} {1}(", accessModifier, className);

            if (formattedParameters != null)
            {
                _tt.Write(GetCsv(formattedParameters));
            }

            _tt.WriteLine(")");

            if (formattedArguments != null)
            {
                _tt.PushIndent();
                _tt.Write(": base({0})", GetCsv(formattedArguments));
                _tt.PopIndent();
                _tt.WriteLine();
            }

            _tt.WriteLine("{");
        }

        public void EndWriteConstructor()
        {
            _tt.WriteLine("}");
        }

        #endregion

        #region Helpers

        private void ProcessClassName(ref string className)
        {
            ProcessIdentifier(ref className, "Class name");
        }

        private void ProcessFieldName(ref string fieldName)
        {
            ProcessIdentifier(ref fieldName, "Field name");
        }

        private void ProcessPropertyName(ref string propertyName)
        {
            ProcessIdentifier(ref propertyName, "Property name");
        }

        private void ProcessTypeName(ref string typeName)
        {
            ProcessIdentifier(ref typeName, "Type name");
        }

        private void ProcessTypeName(ref string typeName, ref string defaultValue)
        {
            ProcessTypeName(ref typeName);

            Type type;
            if (TryGetType(typeName, out type))
            {
                if (type == typeof(string) && defaultValue != "null" && !defaultValue.StartsWith("\"") && !defaultValue.EndsWith("\""))
                {
                    defaultValue = string.Format("\"{0}\"", defaultValue);
                }
            }
        }

        private void ProcessGenericConstraint(ref string genericConstraint)
        {
            genericConstraint = genericConstraint.Trim();

            if (!genericConstraint.StartsWith("where"))
            {
                genericConstraint = string.Format("where {0}", genericConstraint);
            }
        }

        private void ProcessPropertyAccessModifiers(ref string getAccessModifier, ref string setAccessModifier, out string primaryAccessModifier)
        {
            bool isStatic;
            ProcessPropertyAccessModifiers(ref getAccessModifier, ref setAccessModifier, out primaryAccessModifier, out isStatic);
        }

        private void ProcessPropertyAccessModifiers(ref string getAccessModifier, ref string setAccessModifier, out string primaryAccessModifier, out bool isStatic)
        {
            isStatic = false;
            ProcessAccessModifier(ref getAccessModifier, ref isStatic);
            ProcessAccessModifier(ref setAccessModifier, ref isStatic);
            primaryAccessModifier = GetLeastRestrictiveModifier(ref getAccessModifier, ref setAccessModifier);

            if (isStatic)
            {
                primaryAccessModifier += " static";
            }
        }

        private void ProcessAccessModifier(ref string accessModifier, ref bool isStatic)
        {
            bool isReadonly = false;
            bool isConst = false;
            ProcessAccessModifier(ref accessModifier, ref isStatic, ref isReadonly, ref isConst);
        }

        private void ProcessAccessModifier(ref string accessModifier, ref bool isStatic, ref bool isReadonly, ref bool isConst)
        {
            if (string.IsNullOrEmpty(accessModifier))
            {
                throw new ArgumentNullException("Access modifiers cannot be null.", "accessModifier");
            }

            accessModifier = accessModifier.ToLower();

            isStatic |= accessModifier.Contains("static");
            isReadonly |= accessModifier.Contains("readonly");
            isConst |= accessModifier.Contains("const");

            accessModifier = accessModifier.Replace("static", string.Empty)
                                           .Replace("readonly", string.Empty)
                                           .Replace("const", string.Empty);

            accessModifier = accessModifier.Trim();

            if (!_accessModifiers.Contains(accessModifier))
            {
                throw new ArgumentException(
                    string.Format(
                        "The access modifier '{0}' is invalid. Valid access modifiers are: {1}.",
                        accessModifier,
                        GetCsv(_accessModifiers)
                    ),
                    "accessModifier"
                );
            }
        }

        private void ProcessConstructorAccessModifier(ref string accessModifier)
        {
            if (string.IsNullOrEmpty(accessModifier))
            {
                throw new ArgumentNullException("Access modifiers cannot be null.", "accessModifier");
            }

            if (accessModifier.IndexOf("static", StringComparison.OrdinalIgnoreCase) != -1)
            {
                accessModifier = "static";
            }
            else
            {
                bool isStatic = false;
                ProcessAccessModifier(ref accessModifier, ref isStatic);
            }
        }

        private void ProcessEnumerable(ref IEnumerable<string> enumerable)
        {
            if (HasValidItems(enumerable))
            {
                enumerable = enumerable.Where(item => !string.IsNullOrEmpty(item));
            }
            else
            {
                enumerable = null;
            }
        }

        private void ProcessEnumerable<T>(ref IEnumerable<T> enumerable) where T : class
        {
            if (HasValidItems<T>(enumerable))
            {
                enumerable = enumerable.Where(item => item != null);
            }
            else
            {
                enumerable = null;
            }
        }

        private string GetProcessedIdentifier(string id, string idType)
        {
            ProcessIdentifier(ref id, idType);
            return id;
        }

        private string GetProcessedTypeName(string typeName)
        {
            ProcessTypeName(ref typeName);
            return typeName;
        }

        private string GetProcessedGenericConstaint(string genericConstraint)
        {
            ProcessGenericConstraint(ref genericConstraint);
            return genericConstraint;
        }

        private string GetPropertyAccessorSpecifier(string accessorKeyword, string accessModifier)
        {
            if (string.IsNullOrEmpty(accessModifier))
            {
                return accessorKeyword;
            }
            else
            {
                accessModifier = accessModifier.Replace("static", string.Empty).Trim();
                return string.Format("{0} {1}", accessModifier, accessorKeyword);
            }
        }

        private string GetDefaultValue(string typeName)
        {
            Type type;
            if (TryGetType(typeName, out type))
            {
                return GetDefaultValue(type);
            }
            else
            {
                return "null";
            }
        }

        private string GetDefaultValue(Type type)
        {
            if (type.IsClass || type.IsInterface)
            {
                return "null";
            }
            else if (IsStruct(type))
            {
                return string.Format("new {0}", type.FullName);
            }
            else if (type.IsEnum)
            {
                return string.Format("{0}.{1}", type.FullName, Activator.CreateInstance(type).ToString());
            }
            else
            {
                return Activator.CreateInstance(type).ToString();
            }
        }

        private string GetLeastRestrictiveModifier(ref string getAccessModifier, ref string setAccessModifier)
        {
            string primaryAccessModifier;

            if (getAccessModifier == setAccessModifier)
            {
                primaryAccessModifier = getAccessModifier;
                getAccessModifier = null;
                setAccessModifier = null;
            }
            else
            {
                int getIndex = _accessModifiers.IndexOf(getAccessModifier);
                int setIndex = _accessModifiers.IndexOf(setAccessModifier);

                if (getIndex < setIndex)
                {
                    primaryAccessModifier = getAccessModifier;
                    getAccessModifier = null;
                }
                else
                {
                    primaryAccessModifier = setAccessModifier;
                    setAccessModifier = null;
                }
            }

            return primaryAccessModifier;
        }

        private string GetCsv(IEnumerable<string> enumerable)
        {
            return GetCsv(enumerable, item => item.ToString());
        }

        private string GetCsv(IEnumerable<string> enumerable, Func<string, string> processString)
        {
            return enumerable.Aggregate(
                (a, b) => string.Format(
                    "{0}, {1}",
                    processString(a),
                    processString(b)
                )
            );
        }

        private IEnumerable<string> GetFormattedEnumerable<T>(List<T> list, Func<T, string> formatter) where T : class
        {
            if (list == null || list.All(item => item == null))
            {
                return null;
            }
            else
            {
                var stringList = new List<string>();

                foreach (var item in list.Where(item => item != null))
                {
                    stringList.Add(formatter(item));
                }

                return stringList;
            }
        }

        private string GetFormattedParameter(Parameter parameter)
        {
            return string.Format(
                "{0} {1} {2}",
                parameter.Modifier,
                GetProcessedTypeName(parameter.TypeName),
                GetProcessedIdentifier(parameter.Name, "Parameter name")
            ).Trim();
        }

        private string GetFormattedArgument(Argument argument)
        {
            return string.Format(
                "{0} {1}",
                argument.Modifier,
                GetProcessedIdentifier(argument.Name, "Argument name")
            ).Trim();
        }

        private bool TryGetType(string typeName, out Type type)
        {
            type = null;

            try
            {
                type = Type.GetType(typeName);
                if (type != null)
                {
                    return true;
                }
            }
            catch { /* Intentionally consume */ }

            if (string.Compare(typeName, "int", true) == 0)
            {
                type = typeof(int);
            }
            else if (string.Compare(typeName, "double", true) == 0)
            {
                type = typeof(double);
            }
            else if (string.Compare(typeName, "string", true) == 0)
            {
                type = typeof(string);
            }
            else if (string.Compare(typeName, "object", true) == 0)
            {
                type = typeof(object);
            }
            else if (string.Compare(typeName, "DateTime", true) == 0)
            {
                type = typeof(DateTime);
            }

            return type != null;
        }

        private bool IsStruct(Type type)
        {
            return type.IsValueType && !type.IsPrimitive && !type.IsEnum;
        }

        private bool HasValidItems(IEnumerable<string> enumerable)
        {
            return enumerable != null && enumerable.Any(item => !string.IsNullOrEmpty(item));
        }

        private bool HasValidItems<T>(IEnumerable<T> enumerable) where T : class
        {
            return enumerable != null && enumerable.Any(item => item != null);
        }

        #endregion

        #endregion
    }
}