using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SudokuUWP
{
    
    public sealed partial class MainPage : Page
    {
        public static Rect windowRectangle;
        public MainPage()
        {
            this.InitializeComponent();
            windowRectangle = ApplicationView.GetForCurrentView().VisibleBounds;
            //Init_Board();
            GameBoard gb = new GameBoard(BaseGrid,/* Numbers,*/ RankGrid);
        }
    }
}
