using Avalonia;
using Avalonia.Controls;

namespace MechShakerUI.Views.Controls;

public partial class FilterControl : UserControl
{
    public static readonly StyledProperty<string> LabelProperty     = AvaloniaProperty.Register<VolumeControl, string>(nameof(Label));
    public static readonly StyledProperty<float>  FrequencyProperty = AvaloniaProperty.Register<VolumeControl, float>(nameof(Frequency));
    public static readonly StyledProperty<bool>   EnabledProperty   = AvaloniaProperty.Register<VolumeControl, bool>(nameof(Enabled));

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public float Frequency
    {
        get => GetValue(FrequencyProperty);
        set => SetValue(FrequencyProperty, value);
    }

    public bool Enabled
    {
        get => GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    public FilterControl()
    {
        InitializeComponent();
    }
}