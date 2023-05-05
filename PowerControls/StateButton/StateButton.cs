#nullable disable
using PowerControls.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PowerControls;

public class StateButton : Button
{
    public static readonly DependencyProperty StatesProperty = DependencyProperty.Register("States", typeof(IEnumerable), typeof(StateButton), new PropertyMetadata(null, StatesPropertyChanged));
    public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(object), typeof(StateButton), new PropertyMetadata(null, StatePropertyChanged));
    public static readonly DependencyProperty CaptionsProperty = DependencyProperty.Register("Captions", typeof(IDictionary<object, string>), typeof(StateButton), new PropertyMetadata(null, CaptionPropertyChanged));
    private static readonly DependencyProperty CaptionProperty = DependencyProperty.Register("Caption", typeof(string), typeof(StateButton));

    public IEnumerable<object> States
    {
        get => ((IEnumerable)GetValue(StatesProperty))?.Cast<object>();
        set => SetValue(StatesProperty, value);
    }
    public object State
    {
        get => GetValue(StateProperty);
        set => SetValue(StateProperty, value);
    }

    public IDictionary<object, string> Captions
    {
        get => (IDictionary<object, string>)GetValue(CaptionsProperty);
        set => SetValue(CaptionsProperty, value);
    }

    private string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    static StateButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StateButton), new FrameworkPropertyMetadata(typeof(StateButton)));
    }

    public StateButton()
    {
        Click += StateButton_Click;
        SetBinding(ContentProperty, new Binding("Caption") { Source = this });
    }

    private void StateButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            State = States.ToList()[(States.ToList().IndexOf(State) + 1) % States.Count()];
        }
        catch
        {
            return;
        }
    }

    private static void StatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (e.NewValue is not null && e.NewValue != e.OldValue)
        {
            if (sb.States.Contains(sb.State))
                return;
            else
                sb.State = sb.States.FirstOrDefault();
        }
        else if ((e.NewValue is null || !sb.States.Any()) && sb.State is null)
        {
            sb.State = default;
            sb.Caption = " ";
        }
    }

    private static void StatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (sb.States is not null)
            if (!sb.States.Contains(sb.State))
                sb.State = sb.States.FirstOrDefault();

        if (sb.Captions is not null && sb.Captions.ContainsKey(sb.State))
            sb.Caption = sb.Captions[sb.State];
        else
            sb.Caption = sb.State.ToString();
    }

    private static void CaptionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (sb.State is not null)
            if ((e.NewValue as Dictionary<object, string>).ContainsKey(sb.State))
                sb.Caption = sb.Captions[sb.State];
    }
}
