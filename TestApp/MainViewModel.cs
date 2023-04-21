using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestApp;

public partial class MainViewModel : INotifyPropertyChanged
{
    public List<int> States => new() { 0, 1, 2, 3 };

    private int state;

    public int State
    {
        get => state;
        set
        {
            state = value;
            OnPropertyChanged();
        }
    }

    private int index;

    public int Index
    {
        get => index;
        set
        {
            index = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void Click()
    {
        Index++;
    }

    #region PropertyChanged
#nullable disable
    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string name = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
#nullable enable
    #endregion
}
