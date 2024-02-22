using System;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace MechShakerUI.Managers;

internal class StringWriterSink : ILogEventSink
{
    private readonly LoggingManager _loggingManager;
    private readonly StringWriter   _writer = new();
    private readonly StringBuilder  _builder;
    private readonly ITextFormatter _formatter;

    public StringWriterSink(LoggingManager loggingManager, ITextFormatter formatter)
    {
        _loggingManager = loggingManager;
        _formatter      = formatter;
        _builder        = _writer.GetStringBuilder();
    }

    public void Emit(LogEvent logEvent)
    {
        if (logEvent == null)
            throw new ArgumentNullException(nameof(logEvent));

        _formatter.Format(logEvent, _writer);
        _loggingManager.Update(_builder.ToString());
        _builder.Clear();
    }
}