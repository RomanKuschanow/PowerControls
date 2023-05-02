#nullable disable
using PowerControls.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PowerControls;
public class MaskedTextBox : TextBox
{
    private ScrollViewer host;

    public static readonly DependencyProperty MaskProperty = DependencyProperty.Register("Mask", typeof(Mask), typeof(MaskedTextBox));
    public static readonly DependencyProperty ResultProperty = DependencyProperty.Register("Result", typeof(string), typeof(MaskedTextBox));

    public Mask Mask
    {
        get => (Mask)GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    public string Result
    {
        get => (string)GetValue(ResultProperty);
        set => SetValue(ResultProperty, value);
    }

    static MaskedTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(MaskedTextBox), new FrameworkPropertyMetadata(typeof(MaskedTextBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        SelectionChanged += MaskedTextBox_SelectionChanged;
        GotFocus += MaskedTextBox_GotFocus;
        LostFocus += MaskedTextBox_LostFocus;
        (Result, Text) = Mask.GetText(Text, IsFocused);
    }

    private void MaskedTextBox_GotFocus(object sender, RoutedEventArgs e) => (Result, Text) = Mask.GetText(Text, true);

    private void MaskedTextBox_LostFocus(object sender, RoutedEventArgs e) => (Result, Text) = Mask.GetText(Text);

    private void MaskedTextBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        if (CaretIndex != Text.Length)
            CaretIndex = Text.Length;
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        if (e.Property == TextProperty)
            (Result, Text) = Mask.GetText(e.NewValue as string, IsFocused);
    }
}
