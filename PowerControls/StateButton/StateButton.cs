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
    public static readonly DependencyProperty StateIndexProperty = DependencyProperty.Register("StateIndex", typeof(int), typeof(StateButton), new PropertyMetadata(0, StateIndexPropertyChanged));

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
    public int StateIndex
    {
        get => (int)GetValue(StateIndexProperty);
        set => SetValue(StateIndexProperty, value);
    }

    static StateButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StateButton), new FrameworkPropertyMetadata(typeof(StateButton)));
    }

    public StateButton()
    {
        Click += StateButton_Click;
        SetBinding(ContentProperty, new Binding("State") { Converter = new ToStringConverter() });
    }

    private void StateButton_Click(object sender, RoutedEventArgs e) => StateIndex = (StateIndex + 1) % States.Count();

    private static void StatesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (e.NewValue is not null && e.NewValue != e.OldValue)
        {
            if (sb.StateIndex >= sb.States.Count())
            {
                sb.StateIndex = sb.StateIndex % sb.States.Count();
                sb.State = sb.States.ToList()[sb.StateIndex];
            }
            if (sb.States.Contains(sb.State))
            {
                sb.State = sb.States.ToList()[sb.StateIndex];
                sb.StateIndex = sb.States.ToList().IndexOf(sb.State);
            }
            else
            {
                sb.StateIndex = 0;
                sb.State = default;
            }
        }
        else if (e.NewValue is null)
        {
            sb.State = default;
            sb.StateIndex = 0;
        }
    }

    private static void StatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (sb.States is not null)
        {
            if (!sb.States.Contains(sb.State))
            {
                sb.StateIndex = 0;
                sb.State = default;
            }
            sb.StateIndex = sb.States.ToList().IndexOf(sb.State);
        }
        else if (e.NewValue is null)
            sb.StateIndex = 0;
    }

    private static void StateIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var sb = (StateButton)d;

        if (sb.States is not null)
        {
            if ((int)e.NewValue >= sb.States.Count())
            {
                sb.StateIndex = 0;
                sb.State = sb.States.First();
            }
            else
                sb.State = sb.States.ToList()[sb.StateIndex];
        }
    }
}
