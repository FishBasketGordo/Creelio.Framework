namespace Creelio.Framework.Test.Templating.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Creelio.Framework.Templating.Extensions.TextTransformationExtensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.VisualStudio.TextTemplating;

    [TestClass]
    public class TextTransformationExtensionsTest
    {
        private MockTextTransformation _tt = new MockTextTransformation();

        [TestMethod]
        public void WritingOneLineShouldMatchFormat()
        {
            string[] lines = { "Line 1" };
            TestWriteLines(lines);
        }

        [TestMethod]
        public void WritingTwoLinesShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2" };
            TestWriteLines(lines);
        }

        [TestMethod]
        public void WritingThreeLinesShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            TestWriteLines(lines);
        }

        [TestMethod]
        public void WritingOneLineWithFormatterShouldMatchFormat()
        {
            string[] lines = { "Line 1" };
            TestWriteLinesWithFormatter(lines);
        }

        [TestMethod]
        public void WritingTwoLinesWithFormatterShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2" };
            TestWriteLinesWithFormatter(lines);
        }

        [TestMethod]
        public void WritingThreeLinesWithFormatterShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            TestWriteLinesWithFormatter(lines);
        }

        [TestMethod]
        public void WritingOneLineWithFirstAndOtherFormattersShouldMatchFormat()
        {
            string[] lines = { "Line 1" };
            TestWriteLinesWithFirstAndOtherFormatters(lines);
        }

        [TestMethod]
        public void WritingTwoLinesWithFirstAndOtherFormattersShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2" };
            TestWriteLinesWithFirstAndOtherFormatters(lines);
        }

        [TestMethod]
        public void WritingThreeLinesWithFirstAndOtherFormattersShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            TestWriteLinesWithFirstAndOtherFormatters(lines);
        }

        [TestMethod]
        public void WritingOneLineWithFirstMiddleAndLastFormattersShouldMatchFormat()
        {
            string[] lines = { "Line 1" };
            TestWriteLinesWithFirstMiddleAndLastFormatters(lines);
        }

        [TestMethod]
        public void WritingTwoLinesWithFirstMiddleAndLastFormatterShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2" };
            TestWriteLinesWithFirstMiddleAndLastFormatters(lines);
        }

        [TestMethod]
        public void WritingThreeLinesWithFirstMiddleAndLastFormatterShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            TestWriteLinesWithFirstMiddleAndLastFormatters(lines);
        }

        [TestMethod]
        public void WritingOneLineLinesWithFirstMiddleAndLastFormattersPreferLastShouldMatchFormat()
        {
            string[] lines = { "Line 1" };
            TestWriteLinesWithFirstMiddleAndLastFormattersPreferLast(lines);
        }

        [TestMethod]
        public void WritingTwoLinesLinesWithFirstMiddleAndLastFormattersPreferLastShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2" };
            TestWriteLinesWithFirstMiddleAndLastFormattersPreferLast(lines);
        }

        [TestMethod]
        public void WritingThreeLinesWithFirstMiddleAndLastFormattersPreferLastShouldMatchFormat()
        {
            string[] lines = { "Line 1", "Line 2", "Line 3" };
            TestWriteLinesWithFirstMiddleAndLastFormattersPreferLast(lines);
        }

        private void TestWriteLines(string[] lines)
        {
            _tt.WriteLines(lines);

            Assert.AreEqual(FormatLines(lines), _tt.TransformText());
        }

        private void TestWriteLinesWithFormatter(string[] lines)
        {
            Func<string, string> formatter = l => string.Format("*{0}*", l);
            var formattedLines = from l in lines select formatter(l);

            _tt.WriteLines(lines, formatter);

            Assert.AreEqual(FormatLines(formattedLines), _tt.TransformText());
        }

        private void TestWriteLinesWithFirstAndOtherFormatters(string[] lines)
        {
            Func<string, string> formatter = l => string.Format("m:{0}", l);
            Func<string, string> firstLineFormatter = l => string.Format("f:{0}", l);

            var formattedLines = lines.Select((l, i) => i == 0 ? firstLineFormatter(l) : formatter(l));

            _tt.WriteLines(lines, formatter, firstLineFormatter);

            Assert.AreEqual(FormatLines(formattedLines), _tt.TransformText());
        }

        private void TestWriteLinesWithFirstMiddleAndLastFormatters(string[] lines)
        {
            Func<string, string> formatter = l => string.Format("m:{0}", l);
            Func<string, string> firstLineFormatter = l => string.Format("f:{0}", l);
            Func<string, string> lastLineFormatter = l => string.Format("l:{0}", l);

            var lastIndex = lines.Count() - 1;
            var formattedLines = lines.Select(
                (l, i) =>
                {
                    if (i == 0)
                    {
                        return firstLineFormatter(l);
                    }
                    else if (i == lastIndex)
                    {
                        return lastLineFormatter(l);
                    }
                    else
                    {
                        return formatter(l);
                    }
                });

            _tt.WriteLines(lines, formatter, firstLineFormatter, lastLineFormatter);

            Assert.AreEqual(FormatLines(formattedLines), _tt.TransformText());
        }

        private void TestWriteLinesWithFirstMiddleAndLastFormattersPreferLast(string[] lines)
        {
            Func<string, string> formatter = l => string.Format("m:{0}", l);
            Func<string, string> firstLineFormatter = l => string.Format("f:{0}", l);
            Func<string, string> lastLineFormatter = l => string.Format("l:{0}", l);

            var lastIndex = lines.Count() - 1;
            var formattedLines = lines.Select(
                (l, i) =>
                {
                    if (i == lastIndex)
                    {
                        return lastLineFormatter(l);
                    }
                    else if (i == 0)
                    {
                        return firstLineFormatter(l);
                    }
                    else
                    {
                        return formatter(l);
                    }
                });

            _tt.WriteLines(lines, formatter, firstLineFormatter, lastLineFormatter, true);

            Assert.AreEqual(FormatLines(formattedLines), _tt.TransformText());
        }

        private string FormatLines(IEnumerable<string> lines)
        {
            return string.Format(
                "{1}{0}",
                Environment.NewLine,
                string.Join(Environment.NewLine, lines));
        }

        private class MockTextTransformation : TextTransformation
        {
            public override string TransformText()
            {
                return GenerationEnvironment.ToString();
            }
        }
    }
}