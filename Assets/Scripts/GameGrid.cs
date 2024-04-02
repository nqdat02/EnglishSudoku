using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameGrid : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject sudokuPanel;
    public GameObject fieldPrefab;

    public GameObject controlPanel;
    public GameObject controlPrefab;

    public Button infomationButton;
    public Button backButton;
    public Button finishButton;

    private readonly Dictionary<Tuple<int, int>, FieldPrefabObject> _fieldPrefabObjectsDic = new();
    private FieldPrefabObject _currentHoveredFieldPrefab;
    private SudokuObject _gameObject;
    private SudokuObject _finalObject;

    // Start is called before the first frame update
    void Start()
    {
        CreatePrefabs();
        GenerateNumBtns();
        CreateSudokuObject();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateSudokuObject()
    {
        SudokuGenerator.CreateSudokuObject(out SudokuObject finalSudokuObject, out SudokuObject gameObject);
        // test with pos[2, 2] can be changed?
        //sudokuObject.Values[2, 2] = 9;
        _gameObject = gameObject;
        _finalObject = finalSudokuObject;
        for (int row = 0; row < SudokuObject.sz; row++)
        {
            for (int column = 0;  column < SudokuObject.sz; column++)
            {
                var currentValue = _gameObject.Values[row, column];
                if (currentValue != 0)
                {
                    FieldPrefabObject fieldObject = _fieldPrefabObjectsDic[new Tuple<int, int>(row, column)];
                    fieldObject.SetNumber(currentValue);
                    fieldObject.isChangeAble = false;
                }
            }
        }
    }

    private bool isInformationButtonActive = false;
    public void ClickOn_InformationButton()
    {
        Debug.Log($"Clicked Information Button");
        if ( isInformationButtonActive )
        {
            isInformationButtonActive = false;
            infomationButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
        }
        else
        {
            isInformationButtonActive = true;
            infomationButton.GetComponent<Image>().color = new Color(0.5301531f, 0.96226420f, 0.9488714f);
        }
    }

    public void ClickOn_BackButton()
    {
        SceneManager.LoadScene("Main");
    }

    public void ClickOn_FinishButton()
    {
        for (int row = 0; row < 9; ++row)
        {
            for (int column = 0; column < 9; ++column)
            {
                FieldPrefabObject fieldObject = _fieldPrefabObjectsDic[new Tuple<int, int>(row, column)];
                if (fieldObject.isChangeAble)
                {
                    if (_finalObject.Values[row, column] == fieldObject.Number)
                    {
                        fieldObject.ChangeColorToGreen();
                    }
                    else 
                    {
                        fieldObject.ChangeColorToRed();
                    }
                }
            }
        }
    }

    private void CreatePrefabs()
    {
        for (int row = 0; row < 9; ++row)
        {
            for (int column = 0; column < 9; ++column)
            {
                GameObject instance = GameObject.Instantiate(fieldPrefab, sudokuPanel.transform);

                FieldPrefabObject fieldObject = new(instance, row, column);
                _fieldPrefabObjectsDic.Add(new Tuple<int, int>(row, column), fieldObject);

                instance.GetComponent<Button>().onClick.AddListener(
                    () => OnClick_FieldPrefab(fieldObject)
                );
            }
        }
    }
    private void OnClick_FieldPrefab(FieldPrefabObject fieldObject)
    {
        //Debug.Log($"CLicked on Prefab Row: {fieldObject.Row} and Column: {fieldObject.Colomn}");
        if (fieldObject.isChangeAble)
        {
            if (_currentHoveredFieldPrefab != null)
            {
                _currentHoveredFieldPrefab.UnsetHoverMode();
                Debug.Log("Change to default");
            }
            _currentHoveredFieldPrefab = (FieldPrefabObject)fieldObject;
            fieldObject.SetHoverMode();

        }
    }

    private void GenerateNumBtns()
    {
        for (int i = 1; i < 10; ++i)
        {
            GameObject instance = GameObject.Instantiate(controlPrefab, controlPanel.transform);
            instance.GetComponentInChildren<Text>().text = i.ToString();
            GameNumber gameNumber = new(){
                Number = i
            };
            instance.GetComponent<Button>().onClick.AddListener(
                () => OnClick_FieldNumberPrefab(gameNumber)
            );
        }
    }

    private void OnClick_FieldNumberPrefab(GameNumber gameNumber)
    {
        //Debug.Log($"CLicked on number {gameNumber.Number}");
        //_currentHoveredFieldPrefab?.SetNumber(gameNumber.Number);
        if (_currentHoveredFieldPrefab != null)
        {
            if (isInformationButtonActive)
            {
                _currentHoveredFieldPrefab.SetSmallNumber(gameNumber.Number);
            }
            else
            {   // Not allow fill when not fill
                //int currentNumber = gameNumber.Number;
                //int row = _currentHoveredFieldPrefab.Row;
                //int column = _currentHoveredFieldPrefab.Colomn;
                //if (_currentSudokuObject.IsPossibleNumberInPosition(currentNumber, row, column))
                //{
                //    _currentHoveredFieldPrefab.SetNumber(gameNumber.Number);
                //}

                // allow fill every number
                _currentHoveredFieldPrefab.SetNumber(gameNumber.Number);

            }
        }
    }
}
