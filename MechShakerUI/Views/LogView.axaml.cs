using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaEdit;

namespace MechShakerUI.Views;

public partial class LogView : UserControl
{
    private readonly TextEditor _textEditor;

    public LogView()
    {
        InitializeComponent();

        _textEditor                              =  this.FindControl<TextEditor>("TextEditor")!;
        _textEditor.TextArea.Caret.CaretBrush    =  Brushes.Transparent;
        _textEditor.TextArea.SelectionBrush      =  new SolidColorBrush(new Color(255, 51, 103, 209));
        _textEditor.TextArea.SelectionForeground =  Brushes.White;
        _textEditor.TextChanged                  += TextEditorOnTextChanged;
    }

    private void TextEditorOnTextChanged(object? sender, EventArgs e)
    {
        ScrollToEndHack();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        ScrollToEndHack();
    }

    private void ScrollToEndHack()
    {
        _textEditor.ScrollToHome();
        _textEditor.ScrollToLine(_textEditor.LineCount - 18);
    }
}