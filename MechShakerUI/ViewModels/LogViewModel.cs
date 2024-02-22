using AvaloniaEdit.Document;
using MechShakerUI.Managers;

namespace MechShakerUI.ViewModels;

internal class LogViewModel : ViewModelBase
{
    public TextDocument? TextDocument { get; }

    public LogViewModel(LoggingManager loggingManager)
    {
        TextDocument              =  new TextDocument();
        loggingManager.LogUpdated += LogUpdated;
    }

    private void LogUpdated(string message)
    {
        TextDocument?.Insert(TextDocument.TextLength, message);
    }
}