using Microsoft.VisualStudio.Core.Imaging;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Language.StandardClassification;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncQuickInfo
{
    internal sealed class LineAsyncQuickInfoSource : IAsyncQuickInfoSource
    {
        private static readonly ImageId _icon = KnownMonikers.AbstractCube.ToImageId();

        private readonly ITextBuffer _textBuffer;

        private LinesCounter linesCounter = new LinesCounter();

        public LineAsyncQuickInfoSource(ITextBuffer textBuffer)
        {
            _textBuffer = textBuffer;
        }

        // This is called on a background thread.
        public Task<QuickInfoItem> GetQuickInfoItemAsync(IAsyncQuickInfoSession session, CancellationToken cancellationToken)
        {
            var triggerPoint = session.GetTriggerPoint(_textBuffer.CurrentSnapshot);

            if (triggerPoint?.Snapshot?.ContentType?.TypeName?.ToLower() == "csharp")
            {
                var allLines = triggerPoint.Value.Snapshot.Lines.Select(x => x.GetText()).ToArray();
                var line = triggerPoint.Value.GetContainingLine();
                var linesCount = linesCounter.GetCurrentLinesInMethod(allLines, line.LineNumber);
                var lineSpan = _textBuffer.CurrentSnapshot.CreateTrackingSpan(line.Extent, SpanTrackingMode.EdgeInclusive);

                var lineNumberElm = new ContainerElement(
                    ContainerElementStyle.Wrapped,
                    new ImageElement(_icon),
                    new ClassifiedTextElement(
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.Keyword, "Lines count in current block: "),
                        new ClassifiedTextRun(PredefinedClassificationTypeNames.Identifier, $"{linesCount}")
                    ));

                return Task.FromResult(new QuickInfoItem(lineSpan, lineNumberElm));
            }

            return Task.FromResult<QuickInfoItem>(null);
        }

        public void Dispose()
        {
            // This provider does not perform any cleanup.
        }
    }
}
