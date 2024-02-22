using Avalonia;
using Avalonia.Controls;

namespace MechShakerUI.Views.Controls;

public partial class SliderControl : UserControl
{
    public static readonly StyledProperty<string> LabelProperty   = AvaloniaProperty.Register<SliderControl, string>(nameof(Label));
    public static readonly StyledProperty<float>  ValueProperty   = AvaloniaProperty.Register<SliderControl, float>(nameof(Value));
    public static readonly StyledProperty<string> UnitsProperty   = AvaloniaProperty.Register<SliderControl, string>(nameof(Units));
    public static readonly StyledProperty<double> StepProperty    = AvaloniaProperty.Register<SliderControl, double>(nameof(Step));
    public static readonly StyledProperty<double> MinimumProperty = AvaloniaProperty.Register<SliderControl, double>(nameof(Minimum));
    public static readonly StyledProperty<double> MaximumProperty = AvaloniaProperty.Register<SliderControl, double>(nameof(Maximum));

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public float Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string Units
    {
        get => GetValue(UnitsProperty);
        set => SetValue(UnitsProperty, value);
    }

    public double Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public SliderControl()
    {
        InitializeComponent();
    }
}