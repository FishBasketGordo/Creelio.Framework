using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;

namespace Creelio.Framework.Templating.TextTransformations
{
    public class DeleteProcedureTransformation : SmoTransformation<SmoTableRenderArgs>
    {
        public override string Render(SmoTableRenderArgs args)
        {
            var database = GetDatabase(args.ConnectionString, args.DatabaseName);
            var table = GetTable(database, args.TableName);
            var primaryKeyCols = GetPrimaryKeyColumns(table);

            SqlFormatter.WriteDisclaimer();
            SqlFormatter.WriteUseStatement(database);
            WriteLine();
            SqlFormatter.BeginWriteStoredProcedure(string.Format("{0}_DELETE", table.Name), primaryKeyCols, false);
            
            PushIndent();
            SqlFormatter.WriteDeleteStatement(table, primaryKeyCols, false);
            PopIndent();

            SqlFormatter.EndWriteStoredProcedure();

            return GenerationEnvironment.ToString();
        }

        public override string TransformText()
        {
            return GenerationEnvironment.ToString();
        }
    }
}
