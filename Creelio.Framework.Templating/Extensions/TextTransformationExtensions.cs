using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Creelio.Framework.Core.Extensions.IEnumerableExtensions;
using Creelio.Framework.Core.Extensions.MaybeMonad;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.Extensions.TextTransformationExtensions
{
    public static class TextTransformationExtensions
    {
        #region Methods

        //#region Export

        //public bool ExportTo(string exportPath)
        //{
        //    string resolvedPath;
        //    return ExportToInternal(exportPath, out resolvedPath);
        //}

        //public bool ExportTo(string exportPath, bool checkOutForEdit)
        //{
        //    string resolvedPath;
        //    bool exported = ExportToInternal(exportPath, out resolvedPath);
        //    bool checkedOut = true;

        //    if (exported && checkOutForEdit)
        //    {
        //        checkedOut = TryCheckOutForEdit(resolvedPath);
        //    }

        //    return exported && checkedOut;
        //}

        //private bool ExportToInternal(string exportPath, out string resolvedPath)
        //{
        //    resolvedPath = null;

        //    try
        //    {
        //        resolvedPath = ReflectedHost.ResolvePath(exportPath);
        //        ExportFile(resolvedPath);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Warning(string.Format("Failed to export to '{0}' resolved as '{1}'. {2}", exportPath, resolvedPath, ex.Message));
        //        return false;
        //    }
        //}

        //private void ExportFile(string resolvedPath)
        //{
        //    if (File.Exists(resolvedPath))
        //    {
        //        File.SetAttributes(resolvedPath, File.GetAttributes(resolvedPath) & ~FileAttributes.ReadOnly);
        //    }

        //    File.WriteAllText(resolvedPath, GenerationEnvironment.ToString());
        //}

        //private bool TryCheckOutForEdit(string filePath)
        //{
        //    try
        //    {
        //        var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(filePath);
        //        var server = TeamFoundationServerFactory.GetServer(workspaceInfo.ServerUri.ToString());
        //        var workspace = workspaceInfo.GetWorkspace(server);

        //        workspace.PendEdit(filePath);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Warning(string.Format("Failed to check out '{0}' for edit. {1}", filePath, ex.Message));
        //        return false;
        //    }
        //}

        //#endregion

        #region Indent

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

        #endregion

        #region WriteLines

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

        public static void WriteLines<T>(this TextTransformation textTransformation, IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter)
        {
            WriteLines(textTransformation, lines, lineFormatter, firstLineFormatter, null);
        }

        public static void WriteLines<T>(this TextTransformation textTransformation, IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter)
        {
            WriteLines(textTransformation, lines, lineFormatter, firstLineFormatter, lastLineFormatter, false);
        }

        public static void WriteLines<T>(this TextTransformation textTransformation, IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter, bool preferLastLineFormatter)
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
                };

                textTransformation.WriteLine(firstLineFormatter(lines.First()));
            }
        }

        #endregion

        #region Miscellaneous

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
                                                         + "set to 'True'."
                                                     )
                                                 );

            return (ITextTemplatingEngineHost)hostProperty.GetValue(textTransformation, null);
        }

        public static string GetConnectionString(this TextTransformation textTransformation, string connStringKey, string path)
        {
            ValidateTextTransformation(textTransformation);

            var config = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = path },
                ConfigurationUserLevel.None
            );

            config.ThrowIfNull(() => new NullReferenceException(string.Format("Unable to find config file '{0}'.", path)));

            return config.ConnectionStrings.ConnectionStrings[connStringKey].ConnectionString;
        }

        #endregion

        #region Helpers

        private static void ValidateTextTransformation(TextTransformation textTransformation)
        {
            textTransformation.ThrowIfNull(() => new NullReferenceException("Text transformation is null."));
        }

        private static void ProcessWriteLinesArguments<T>(IEnumerable<T> lines, Func<T, string> lineFormatter, ref Func<T, string> firstLineFormatter, ref Func<T, string> lastLineFormatter)
        {
            lines.ThrowIfNull(
                     () => new ArgumentNullException("lines")
                 ).ThrowIf(
                     l => l.Count() == 0,
                     () => new ArgumentException("No lines to write.", "lines")
                 );

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

        #endregion

        #endregion
    }
}
