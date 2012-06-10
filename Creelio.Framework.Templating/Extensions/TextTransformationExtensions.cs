namespace Creelio.Framework.Templating.Extensions.TextTransformationExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Creelio.Framework.Core.Extensions.MaybeMonad;
    using Microsoft.VisualStudio.TextTemplating;

    public static class TextTransformationExtensions
    {
        public static void PushIndent(this TextTransformation textTransformation)
        {
            PushIndent(textTransformation, 1);
        }

        public static void PushIndent(this TextTransformation textTransformation, int indent)
        {
            if (indent > 0)
            {
                ValidateTextTransformation(textTransformation);
                textTransformation.PushIndent(new string(' ', 4 * indent));
            }
        }

        public static void WriteLine(this TextTransformation textTransformation)
        {
            WriteLine(textTransformation, 1);
        }

        public static void WriteLine(this TextTransformation textTransformation, int howMany)
        {
            ValidateTextTransformation(textTransformation);

            while (howMany-- != 0)
            {
                textTransformation.WriteLine(string.Empty);
            }
        }

        public static void WriteLines<T>(this TextTransformation textTransformation, IEnumerable<T> lines)
        {
            WriteLines(textTransformation, lines, line => line.ToString());
        }

        public static void WriteLines<T>(this TextTransformation textTransformation, IEnumerable<T> lines, Func<T, string> lineFormatter)
        {
            WriteLines(textTransformation, lines, lineFormatter, null);
        }

        public static void WriteLines<T>(
            this TextTransformation textTransformation, 
            IEnumerable<T> lines, 
            Func<T, string> lineFormatter, 
            Func<T, string> firstLineFormatter)
        {
            WriteLines(textTransformation, lines, lineFormatter, firstLineFormatter, null);
        }

        public static void WriteLines<T>(
            this TextTransformation textTransformation, 
            IEnumerable<T> lines, 
            Func<T, string> lineFormatter, 
            Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter)
        {
            WriteLines(textTransformation, lines, lineFormatter, firstLineFormatter, lastLineFormatter, false);
        }

        public static void WriteLines<T>(
            this TextTransformation textTransformation, 
            IEnumerable<T> lines, 
            Func<T, string> lineFormatter, 
            Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter, 
            bool preferLastLineFormatter)
        {
            ValidateTextTransformation(textTransformation);
            ProcessWriteLinesArguments<T>(lines, lineFormatter, ref firstLineFormatter, ref lastLineFormatter);

            var lineCount = lines.Count();

            if (lineCount == 1)
            {
                if (preferLastLineFormatter)
                {
                    textTransformation.WriteLine(lastLineFormatter(lines.Last()));
                }
                else
                {
                    textTransformation.WriteLine(firstLineFormatter(lines.First()));
                }
            }
            else if (lineCount == 2)
            {
                textTransformation.WriteLine(firstLineFormatter(lines.First()));
                textTransformation.WriteLine(lastLineFormatter(lines.Last()));
            }
            else
            {
                textTransformation.WriteLine(lastLineFormatter(lines.Last()));

                foreach (var line in lines.Skip(1).Take(lineCount - 2)) 
                {
                    textTransformation.WriteLine(lineFormatter(line));
                }

                textTransformation.WriteLine(firstLineFormatter(lines.First()));
            }
        }

        public static ITextTemplatingEngineHost GetHost(this TextTransformation textTransformation)
        {
            ValidateTextTransformation(textTransformation);

            var hostProperty = textTransformation.GetType()
                                                 .GetProperty("Host")
                                                 .ThrowIfNull(
                                                     () => new InvalidOperationException(
                                                           "Cannot find Host property. "
                                                         + "Please ensure that the 'hostspecific' "
                                                         + "attribute on the template directive is "
                                                         + "set to 'True'."));

            return (ITextTemplatingEngineHost)hostProperty.GetValue(textTransformation, null);
        }

        public static string GetConnectionString(this TextTransformation textTransformation, string connStringKey, string path)
        {
            ValidateTextTransformation(textTransformation);

            var config = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = path },
                ConfigurationUserLevel.None);

            config.ThrowIfNull(() => new NullReferenceException(string.Format("Unable to find config file '{0}'.", path)));

            return config.ConnectionStrings.ConnectionStrings[connStringKey].ConnectionString;
        }

        private static void ValidateTextTransformation(TextTransformation textTransformation)
        {
            textTransformation.ThrowIfNull(() => new NullReferenceException("Text transformation is null."));
        }

        private static void ProcessWriteLinesArguments<T>(IEnumerable<T> lines, Func<T, string> lineFormatter, ref Func<T, string> firstLineFormatter, ref Func<T, string> lastLineFormatter)
        {
            lines.ThrowIfNull(
                     () => new ArgumentNullException("lines"))
                 .ThrowIf(
                     l => l.Count() == 0,
                     () => new ArgumentException("No lines to write.", "lines"));

            lineFormatter.ThrowIfNull(() => new ArgumentNullException("lineFormatter"));

            if (firstLineFormatter == null)
            {
                firstLineFormatter = lineFormatter;
            }

            if (lastLineFormatter == null)
            {
                lastLineFormatter = lineFormatter;
            }
        }        
    }
}