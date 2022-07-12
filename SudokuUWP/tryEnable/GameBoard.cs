using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SudokuUWP
{
    public class NumberPressedEventArgs : EventArgs
    {
        string _numberPressed;
        public string NumberPressed { get { return _numberPressed; } }
        public NumberPressedEventArgs(string numberPressed)
        {
            _numberPressed = numberPressed;
        }
    }
    public enum Rank { Easy = 36, Medium = 31, Hard = 28 }
    internal class GameBoard
    {
        // Private Fields
        TextBox[,] textBlocks;
        Grid BaseGrid, NumbersGrid, RankGrid;
        SudokuLogic _sudoku;
        int _missingDigits; // Number Of missing digits
        const int _numOfColRow = 9;

        public event Action<object, NumberPressedEventArgs> NumberPressedEventHandler;

        // Constructor
        public GameBoard(Grid boardGrid/*, Grid numbersGrid*/, Grid rankGrid, Rank rank = Rank.Easy)
        {
            _sudoku = new SudokuLogic(_numOfColRow);
            DefineRank(rank);
            SetTextBlocks();
            Init_RankSelectGrid(rankGrid);
            //Init_NumbersGrid(numbersGrid);
            Init_BoardWithNumberLegality(boardGrid); 
        }
        void DefineRank(Rank rank)
        {
            switch (rank)
            {
                case Rank.Easy:
                    _missingDigits = (int)(Math.Pow(_numOfColRow, 2) - (int)Rank.Easy); // Try change here _missingDigits = 1 / 2 to see what happens when u are done
                    break;
                case Rank.Medium:
                    _missingDigits = (int)(Math.Pow(_numOfColRow, 2) - (int)Rank.Medium);
                    break;
                case Rank.Hard:
                    _missingDigits = (int)(Math.Pow(_numOfColRow, 2) - (int)Rank.Hard);
                    break;
            }
        }
        void Init_RankSelectGrid(Grid rankGrid)
        {
            List<Button> rankButtons = new List<Button>
            {
                new Button { Content = $"Easy", FontSize = 20, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
                new Button { Content = $"Medium", FontSize = 20, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center },
                new Button { Content = $"Hard", FontSize = 20, Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)), HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center }
            };

            RankGrid = rankGrid;
            RankGrid.Margin = new Thickness(MainPage.windowRectangle.Width - 300, MainPage.windowRectangle.Height - 100, 0, 0);
            int Col = -1;
            foreach (ColumnDefinition column in RankGrid.ColumnDefinitions)
            {
                Col++;
                Border panel = new Border() { Margin = new Thickness(1), Background = new SolidColorBrush(Color.FromArgb(255, 200, 160, 111)) };

                Grid.SetColumn(panel, Col);
                Grid.SetRow(panel, 1);

                Button tempBtn = rankButtons[Col];
                tempBtn.Click += RankSelect_Click;
                panel.Child = tempBtn;
                RankGrid.Children.Add(panel);
            }
        }
        void RankSelect_Click(object sender, RoutedEventArgs e)
        {
            Button temp = sender as Button;
            string rankStr = temp.Content.ToString();
            switch (rankStr)
            {
                case "Easy": /// Rank.Easy.ToString() #version 8.0
                    RestartGame(Rank.Easy);
                    break;
                case "Medium": /// Rank.Medium.ToString() #version 8.0
                    RestartGame(Rank.Medium);
                    break;
                case "Hard": /// Rank.Hard.ToString() #version 8.0
                    RestartGame(Rank.Hard);
                    break;
            }
        }
        #region NotInUse
        void Init_NumbersGrid(Grid numbersGrid)
        {
            NumbersGrid = numbersGrid;
            NumbersGrid.Margin = new Thickness(20, MainPage.windowRectangle.Height - 100, 200, 0);
            int Col = -1;
            foreach (ColumnDefinition column in NumbersGrid.ColumnDefinitions)
            {
                Col++;
                Border panel = new Border() { Margin = new Thickness(1), Background = new SolidColorBrush(Color.FromArgb(255, 200, 160, 111)) };

                Grid.SetColumn(panel, Col);
                Grid.SetRow(panel, 1);

                Button tempBtn = new Button() {
                    Name = $"{Col + 1}",
                    Content = $"{Col + 1}",
                    FontSize = 35,
                    FontStyle = Windows.UI.Text.FontStyle.Oblique,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                tempBtn.Click += ButtonClick;
                NumberPressedEventHandler += CheckNumberClick;
                panel.Child = tempBtn;
                NumbersGrid.Children.Add(panel);
            }
        }
        void ButtonClick(object sender, RoutedEventArgs e)
        {
            Button temp = sender as Button;
            //GameBoard.num = temp.Content.ToString();
            //temp.IsEnabled = false;
        }
        void CheckNumberClick(object sender, NumberPressedEventArgs e)
        {
        }
        #endregion
        void Init_BoardWithNumberLegality(Grid boardGrid)
        {
            BaseGrid?.Children.Clear();
            BaseGrid = boardGrid;
            BaseGrid.Margin = new Thickness(50, 0, 50, 100);
            int Row = -1;
            foreach (RowDefinition row in BaseGrid.RowDefinitions)
            {
                Row++;
                int Col = -1;
                foreach (ColumnDefinition column in BaseGrid.ColumnDefinitions)
                {
                    Col++;
                    Border panel = new Border() { Margin = new Thickness(1), Background = new SolidColorBrush(Color.FromArgb(100, 100, 100, 100)) };

                    Grid.SetColumn(panel, Col);
                    Grid.SetRow(panel, Row);

                    panel.Child = textBlocks[Col,Row];
                    BaseGrid.Children.Add(panel);
                }
            }
        }
        public void SetTextBlocks()
        {
            textBlocks = new TextBox[_numOfColRow, _numOfColRow];
            for (int i = 0; i < textBlocks.GetLength(0); i++)
            {
                for (int j = 0; j < textBlocks.GetLength(1); j++)
                {
                    textBlocks[i, j] = new TextBox()
                    {
                        Name = $"{_sudoku[i, j]}",
                        Text = $"{_sudoku[i,j]}",
                        FontSize = 35,
                        FontStyle = Windows.UI.Text.FontStyle.Oblique,
                        Foreground = new SolidColorBrush(Color.FromArgb(255, 1, 1, 1)),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        IsEnabled = false
                    };
                    textBlocks[i, j].CharacterReceived += GameBoard_CharacterReceived;
                }
            }
            CollapseDigits();
        }
        void GameBoard_CharacterReceived(UIElement sender, Windows.UI.Xaml.Input.CharacterReceivedRoutedEventArgs args)
        {
            TextBox tempTB = sender as TextBox;
            if (!tempTB.Text.Equals(tempTB.Name))
                tempTB.Text = "";
            else
            {
                _missingDigits--;
                if (_missingDigits == 0)
                    WinMessage();
                else
                    tempTB.IsEnabled = false;
            }
        }
        async void WinMessage()
        {
            await new MessageDialog($"You won.\nPress Close to Play again").ShowAsync();
            RestartGame();
        }
        void RestartGame(Rank rank = Rank.Easy)
        {
            _sudoku = new SudokuLogic(_numOfColRow);
            DefineRank(rank);
            SetTextBlocks();
            Init_RankSelectGrid(RankGrid);
            //Init_NumbersGrid(numbersGrid);
            Init_BoardWithNumberLegality(BaseGrid);
        }
        public void CollapseDigits()
        {
            int count = _missingDigits;
            while (count != 0)
            {
                int cellCoordiantes = SudokuLogic.RandomGenerator(_numOfColRow * _numOfColRow) - 1;

                int i = cellCoordiantes / _numOfColRow;
                int j = cellCoordiantes % _numOfColRow;

                if (textBlocks[i, j].Text != string.Empty)
                {
                    count--;
                    textBlocks[i, j].Text = string.Empty;
                    textBlocks[i, j].IsEnabled = true;
                }
            }
        }
    }
}
