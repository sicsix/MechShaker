using Avalonia;
using Avalonia.Controls;

namespace MechShakerUI.Views.Controls;

public partial class VolumeControl : UserControl
{
    public static readonly StyledProperty<string> HeaderProperty  = AvaloniaProperty.Register<VolumeControl, string>(nameof(Header));
    public static readonly StyledProperty<float>  VolumeProperty  = AvaloniaProperty.Register<VolumeControl, float>(nameof(Volume));
    public static readonly StyledProperty<bool>   EnabledProperty = AvaloniaProperty.Register<VolumeControl, bool>(nameof(Enabled));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public float Volume
    {
        get => GetValue(VolumeProperty);
        set => SetValue(VolumeProperty, value);
    }

    public bool Enabled
    {
        get => GetValue(EnabledProperty);
        set => SetValue(EnabledProperty, value);
    }

    public VolumeControl()
    {
        InitializeComponent();
    }
}