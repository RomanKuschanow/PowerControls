using System;
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
public class PlaceholderTextBox : TextBox
{
    public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder", typeof(string), typeof(MaskedTextBox));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    static PlaceholderTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PlaceholderTextBox), new FrameworkPropertyMetadata(typeof(PlaceholderTextBox)));
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        TextBlock placeholder = (TextBlock)Template.FindName("Placeholder", this);
        placeholder.DataContext = this;
        placeholder.SetBinding(TextBlock.TextProperty, new Binding("Placeholder"));
    }
}
