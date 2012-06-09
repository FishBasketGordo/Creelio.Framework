using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Creelio.Framework.Templating.TextTransformations
{
    public abstract class CsvTransformation : TextTransformation
    {
        protected internal IEnumerable<Dictionary<string, string>> ReadCsvRecords(string path)
        {
            var resolvedPath = ReflectedHost.ResolvePath(path);

            using (var parser = new TextFieldParser(resolvedPath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.Delimiters = new string[] { "," };
                parser.TrimWhiteSpace = true;
                parser.HasFieldsEnclosedInQuotes = false;

                string[] headers = null;

                while (!parser.EndOfData)
                {
                    var fields = parser.ReadFields();

                    if (headers == null)
                    {
                        headers = fields;
                    }
                    else
                    {
                        var record = new Dictionary<string, string>();

                        for (int fieldIndex = 0; fieldIndex < headers.Length; fieldIndex++)
                        {
                            record.Add(headers[fieldIndex], fields[fieldIndex]);
                        }

                        yield return record;
                    }
                }
            }
        }

        protected internal Dictionary<string, int> ReadMaxFieldLengths(string path)
        {
            Dictionary<string, int> maxLengths = null;

            foreach (var record in ReadCsvRecords(path))
            {
                if (maxLengths == null)
                {
                    maxLengths = new Dictionary<string, int>();

                    foreach (var kvp in record)
                    {
                        maxLengths.Add(kvp.Key, Math.Max(kvp.Key.Length, kvp.Value.Length));
                    }
                }
                else
                {
                    foreach (var kvp in record)
                    {
                        if (kvp.Value.Length > maxLengths[kvp.Key])
                        {
                            maxLengths[kvp.Key] = kvp.Value.Length;
                        }
                    }
                }
            }

            return maxLengths;
        }
    }
}