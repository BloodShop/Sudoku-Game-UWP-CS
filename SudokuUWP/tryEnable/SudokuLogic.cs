using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuUWP
{
    public class SudokuLogic
    {
        int[,] _board;
        int _numOfColRow; // number of columns/rows.
        int _sqrtOfNumOfColRow; // square root of _numOfColRow
        public int[,] Board { get => _board; }
        public int this[int i, int j] { get => _board[i, j]; } // Indexer to get the number on board

        // Constructor
        public SudokuLogic(int numOfColRow = 9)
        {
            _numOfColRow = numOfColRow;
            _sqrtOfNumOfColRow = (int)Math.Sqrt(_numOfColRow);
            _board = new int[numOfColRow, numOfColRow];

            Init_Board();
        }

        public void Init_Board()
        {
            for (int i = 0; i < _numOfColRow; i = i + _sqrtOfNumOfColRow)
                FillBlock3x3(i, i);
            
            FillRemainingCells(0, _sqrtOfNumOfColRow);
        }
        void FillBlock3x3(int row, int col)
        { 
            int num;
            for (int i = 0; i < _sqrtOfNumOfColRow; i++)
                for (int j = 0; j < _sqrtOfNumOfColRow; j++)
                {
                    do
                    {
                        num = RandomGenerator(_numOfColRow);
                    }
                    while (!NotUsedInHouse(row, col, num));

                    _board[row + i, col + j] = num;
                }
        }
        bool NotUsedInHouse(int rowStart, int colStart, int num)
        {
            for (int i = 0; i < _sqrtOfNumOfColRow; i++)
                for (int j = 0; j < _sqrtOfNumOfColRow; j++)
                    if (_board[rowStart + i, colStart + j] == num)
                        return false;
            return true;
        } // Returns false if given 3 x 3 house contains num
        bool NotUsedInRow(int i, int num) // check Row
        {
            for (int j = 0; j < _numOfColRow; j++)
                if (_board[i, j] == num)
                    return false;
            return true;
        } 
        bool NotUsedInColumn(int j, int num) // check Column
        {
            for (int i = 0; i < _numOfColRow; i++)
                if (_board[i, j] == num)
                    return false;
            return true;
        }
        public static int RandomGenerator(int num) => (int)Math.Floor((double)(new Random().NextDouble() * num + 1));
        bool CheckSafety(int i, int j, int num) => (NotUsedInRow(i, num) && NotUsedInColumn(j, num) && NotUsedInHouse(i - i % _sqrtOfNumOfColRow, j - j % _sqrtOfNumOfColRow, num));
        bool FillRemainingCells(int i, int j) // Fill what left .. Most intense function
        {
            if (j >= _numOfColRow && i < _numOfColRow - 1)
            {
                i = i + 1;
                j = 0;
            }
            if (i >= _numOfColRow && j >= _numOfColRow)
                return true;

            if (i < _sqrtOfNumOfColRow)
            {
                if (j < _sqrtOfNumOfColRow)
                    j = _sqrtOfNumOfColRow;
            }
            else if (i < _numOfColRow - _sqrtOfNumOfColRow)
            {
                if (j == (int)(i / _sqrtOfNumOfColRow) * _sqrtOfNumOfColRow)
                    j = j + _sqrtOfNumOfColRow;
            }
            else
            {
                if (j == _numOfColRow - _sqrtOfNumOfColRow)
                {
                    i = i + 1;
                    j = 0;
                    if (i >= _numOfColRow)
                        return true;
                }
            }

            for (int num = 1; num <= _numOfColRow; num++)
                if (CheckSafety(i, j, num))
                {
                    _board[i, j] = num;
                    if (FillRemainingCells(i, j + 1))
                        return true;

                    _board[i, j] = 0;
                }
            
            return false;
        }
    }
}