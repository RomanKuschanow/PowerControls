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

public partial class MainViewModel : ObservableObject
{
    public List<int> States => new() { 1, 2, 3 };

    [ObservableProperty]
    private int? state;

    [RelayCommand]
    private void Click()
    {
        
    }
}
