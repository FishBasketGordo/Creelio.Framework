using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Creelio.Framework.Extensions.MaybeMonad;
using Creelio.Framework.Extensions.IEnumerableEx;
using Microsoft.VisualStudio.TextTemplating;
using System.IO;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Configuration;

namespace Creelio.Framework.Templating.TextTransformations
{
    public abstract class TextTransformation : Microsoft.VisualStudio.TextTemplating.TextTransformation
    {
        #region Fields

        private ITextTemplatingEngineHost _reflectedHost = null;

        #endregion

        #region Properties

        protected internal ITextTemplatingEngineHost ReflectedHost
        {
            get
            {
                if (_reflectedHost == null)
                {
                    var hostProperty = this.GetType().GetProperty("Host");
                    if (hostProperty == null)
                    {
                        throw new InvalidOperationException(
                              "Cannot find Host property. "
                            + "Please ensure that the 'hostspecific' "
                            + "attribute on the template directive is "
                            + "set to 'True'."
                        );
                    }

                    _reflectedHost = (ITextTemplatingEngineHost)hostProperty.GetValue(this, null);
                }

                return _reflectedHost;
            }
        }

        #endregion

        #region Methods

        #region Export

        public bool ExportTo(string exportPath)
        {
            string resolvedPath;
            return ExportToInternal(exportPath, out resolvedPath);
        }

        public bool ExportTo(string exportPath, bool checkOutForEdit)
        {
            string resolvedPath;
            bool exported = ExportToInternal(exportPath, out resolvedPath);
            bool checkedOut = true;

            if (exported && checkOutForEdit)
            {
                checkedOut = TryCheckOutForEdit(resolvedPath);
            }

            return exported && checkedOut;
        }

        private bool ExportToInternal(string exportPath, out string resolvedPath)
        {
            resolvedPath = null;

            try
            {
                resolvedPath = ReflectedHost.ResolvePath(exportPath);
                ExportFile(resolvedPath);
                return true;
            }
            catch (Exception ex)
            {
                Warning(string.Format("Failed to export to '{0}' resolved as '{1}'. {2}", exportPath, resolvedPath, ex.Message));
                return false;
            }
        }

        private void ExportFile(string resolvedPath)
        {
            if (File.Exists(resolvedPath))
            {
                File.SetAttributes(resolvedPath, File.GetAttributes(resolvedPath) & ~FileAttributes.ReadOnly);
            }

            File.WriteAllText(resolvedPath, GenerationEnvironment.ToString());
        }

        private bool TryCheckOutForEdit(string filePath)
        {
            try
            {
                var workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(filePath);
                var server = TeamFoundationServerFactory.GetServer(workspaceInfo.ServerUri.ToString());
                var workspace = workspaceInfo.GetWorkspace(server);

                workspace.PendEdit(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Warning(string.Format("Failed to check out '{0}' for edit. {1}", filePath, ex.Message));
                return false;
            }
        }

        #endregion

        #region Indent

        protected internal void PushIndent()
        {
            PushIndent(1);
        }

        protected internal void PushIndent(int indent)
        {
            if (indent > 0)
            {
                PushIndent(new string(' ', 4 * indent));
            }
        }

        #endregion

        #region WriteLines

        protected internal void WriteLine()
        {
            WriteLine(1);
        }

        protected internal void WriteLine(int howMany)
        {
            while (howMany-- != 0)
            {
                WriteLine(string.Empty);
            }
        }

        protected internal void WriteLines<T>(IEnumerable<T> lines)
        {
            WriteLines(lines, line => line.ToString());
        }

        protected internal void WriteLines<T>(IEnumerable<T> lines, Func<T, string> lineFormatter)
        {
            WriteLines(lines, lineFormatter, null);
        }

        protected internal void WriteLines<T>(IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter)
        {
            WriteLines(lines, lineFormatter, firstLineFormatter, null);
        }

        protected internal void WriteLines<T>(IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter)
        {
            WriteLines(lines, lineFormatter, firstLineFormatter, lastLineFormatter, false);
        }

        protected internal void WriteLines<T>(IEnumerable<T> lines, Func<T, string> lineFormatter, Func<T, string> firstLineFormatter,
            Func<T, string> lastLineFormatter, bool preferLastLineFormatter)
        {
            ProcessWriteLinesArguments<T>(lines, lineFormatter, ref firstLineFormatter, ref lastLineFormatter);

            var lineCount = lines.Count();

            if (lineCount == 1)
            {
                if (preferLastLineFormatter)
                {
                    WriteLine(lastLineFormatter(lines.Last()));
                }
                else
                {
                    WriteLine(firstLineFormatter(lines.First()));
                }
            }
            else if (lineCount == 2)
            {
                WriteLine(firstLineFormatter(lines.First()));
                WriteLine(lastLineFormatter(lines.Last()));
            }
            else
            {
                WriteLine(lastLineFormatter(lines.Last()));
                lines.Skip(1).Take(lineCount - 2).ForEach(line => WriteLine(lineFormatter(line)));
                WriteLine(firstLineFormatter(lines.First()));
            }
        }

        #endregion

        #region Miscellaneous

        protected internal string GetConnectionString(string connStringKey, string path)
        {
            var configPath = ReflectedHost.ResolvePath(path);
            var config = ConfigurationManager.OpenMappedExeConfiguration(
                new ExeConfigurationFileMap { ExeConfigFilename = configPath },
                ConfigurationUserLevel.None
            );
            return config.ConnectionStrings.ConnectionStrings[connStringKey].ConnectionString;
        }

        #endregion

        #region Helpers

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
