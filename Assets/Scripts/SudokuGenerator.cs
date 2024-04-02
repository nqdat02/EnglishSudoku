using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

public class SudokuGenerator
{
    private static SudokuObject _finalSudokuObject;

    public static void CreateSudokuObject(out SudokuObject finalSudokuObject, out SudokuObject gameObject)
    {
        _finalSudokuObject = null;
        SudokuObject sudokuObject = new();
        CreateRandomGroups(sudokuObject);
        if (TryToSolve(sudokuObject))
        {
            sudokuObject = _finalSudokuObject;
        }
        else
        {
            throw new System.Exception("Something went Wrong");
        }
        finalSudokuObject = sudokuObject;
        gameObject = RemoveSomeRandomNumbers(sudokuObject);
    }

    private static SudokuObject RemoveSomeRandomNumbers(SudokuObject sudokuObject)
    {
        SudokuObject newSudokuObject = new()
        {
            Values = (int[,])sudokuObject.Values.Clone()
        };
        List<Tuple<int, int> > values = GetValues();

        // set Level
        int EndValueIndex = 10;
        if (Setting.EasyMediumHard_Number == 1)
        {
            EndValueIndex = 71;
        }
        if (Setting.EasyMediumHard_Number == 2)
        {
            EndValueIndex = 61;
        }


        bool isFinish = false;
        while (!isFinish)
        {
            int index = Random.Range(0, values.Count);
            var searchedIndex = values[index];
            SudokuObject nextSudokuObject = new()
            {
                Values = (int[,])newSudokuObject.Values.Clone()
            };
            nextSudokuObject.Values[searchedIndex.Item1, searchedIndex.Item2] = 0;

            if (TryToSolve(nextSudokuObject, true)){
                newSudokuObject = nextSudokuObject;
            }
            values.RemoveAt(index);

            if (values.Count < EndValueIndex) 
            {
                isFinish = true;
            }
        }
        return newSudokuObject;
    }

    private static List<Tuple<int, int> > GetValues()
    {
        List<Tuple<int, int>> values = new();
        for (int u = 0; u < 9; ++u)
        {
            for (int v = 0; v < 9; ++v)
            {
                values.Add(new Tuple<int, int>(u, v));
            }
        }
        return values;
    }

    public static void CreateRandomGroups(SudokuObject sudokuObject)
    {
        List<int> values = new() { 0, 1, 2 };
        int index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 1 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 4 + values[index]);
        values.RemoveAt(index);

        index = Random.Range(0, values.Count);
        InsertRandomGroup(sudokuObject, 7 + values[index]);
        //values.RemoveAt(index);
    }

    public static void InsertRandomGroup(SudokuObject sudokuObject, int group) {
        sudokuObject.GetGroupIndex(group, out int startRow, out int startColumn);
        List<int> values = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        for (int row = startRow; row < startRow + 3; row++) { 
            for (int column =  startColumn; column < startColumn + 3; column++)
            {
                int index = Random.Range(0, values.Count);
                sudokuObject.Values[row, column] = values[index];
                values.RemoveAt(index);
            }
        }
    }

    private static bool TryToSolve(SudokuObject sudokuObject, bool OnlyOne = false)
    {
        // find empty fileds which can be filled
        if (HasEmptyFiledsToFill(sudokuObject, out int _row, out int _column, OnlyOne))
        {
            List<int> possibleValues = GetPossibleValues(sudokuObject, _row, _column);
            foreach(var possibleValue in possibleValues)
            {
                SudokuObject nextSudokuObject = new()
                {
                    Values = (int[,])sudokuObject.Values.Clone()
                };
                nextSudokuObject.Values[_row, _column] = possibleValue;
                if (TryToSolve(nextSudokuObject, OnlyOne))
                {
                    return true;
                }
            }
        }
        // has sudokuobject empty fields
        if (HasEmptyFileds(sudokuObject))
        {
            return false;
        }
        _finalSudokuObject = sudokuObject;
        // finish
        return true;
    }

    private static bool HasEmptyFileds(SudokuObject sudokuObject)
    {
        for (int row = 0; row < SudokuObject.sz; ++row)
        {
            for (int column = 0; column < SudokuObject.sz; ++column)
            {
                if (sudokuObject.Values[row, column] == 0)
                {
                    return true;
                }

            }
        }
        return false;
    }

    private static List<int> GetPossibleValues(SudokuObject sudokuObject, int _row, int _column)
    {
        List<int> possibleValues = new();
        for (int value = 1; value <= SudokuObject.sz; ++value)
        {
            if (sudokuObject.IsPossibleNumberInPosition(value, _row, _column))
            {
                possibleValues.Add(value);
            }
        }
        return possibleValues;
    }

    private static bool HasEmptyFiledsToFill(SudokuObject sudokuObject, out int _row, out int _column, bool OnlyOne = false)
    {
        _row = 0;
        _column = 0;
        int amoutOfPossibleValues = 10;
        for (int row = 0; row < SudokuObject.sz; ++row)
        {
            for (int column = 0; column < SudokuObject.sz; column++)
            {
                if (sudokuObject.Values[row, column] == 0)
                {
                    int currentAmount = GetPossibleAmoutOfValues(sudokuObject, row, column);
                    if (currentAmount != 0)
                    {
                        if (currentAmount < amoutOfPossibleValues)
                        {
                            amoutOfPossibleValues = currentAmount;
                            _row = row;
                            _column = column;
                        }
                    }
                }
                    
            }
        }

        if (OnlyOne)
        {
            if (amoutOfPossibleValues == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        if (amoutOfPossibleValues == 10) 
            return false;
        return true;
    }

    private static int GetPossibleAmoutOfValues(SudokuObject sudokuObject, int _row, int _column)
    {
        int amout = 0;
        for (int number = 1; number <= SudokuObject.sz; ++number)
        {
            if (sudokuObject.IsPossibleNumberInPosition(number, _row, _column))
            {
                amout += 1;
            }
        }
        return amout;
    }
}
