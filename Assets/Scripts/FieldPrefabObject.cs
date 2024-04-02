using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FieldPrefabObject
{
    private int _row;
    private int _column;
    private readonly GameObject _instance;

    private readonly Color _baseColor = new(1f, 1f, 1f, 1f);
    private readonly Color _changeColor = new(0.3823959f, 0.9074903f, 0.9339623f, 1f);
    private readonly Color _greenColor = Color.green;
    private readonly Color _redColor = Color.red;
    public bool isChangeAble = true;

    public void ChangeColorToGreen()
    {
        _instance.GetComponent<Image>().color = _greenColor;
    }

    public void ChangeColorToRed() 
    {
        _instance.GetComponent<Image>().color = _redColor;
    }

    public bool TryGetTextByName(string name, out Text text)
    {
        text = null;
        Text[] texts = _instance.GetComponentsInChildren<Text>();
        foreach (var currentText in texts)
        {
            if (currentText.name.Equals(name))
            {
                text = currentText;
                return true;
            }
        }
        return false;
    }

    public FieldPrefabObject(GameObject instance, int row, int colomn)
    {
        _instance = instance;
        _row = row;
        _column = colomn;
    }

    public int Row
    {
        get { return _row; }
        set { _row = value; }
    }

    public int Colomn
    {
        get { return _column; }
        set { _column = value; }
    }

    public void SetHoverMode()
    {
        _instance.GetComponent<Image>().color = _changeColor;
    }

    public void UnsetHoverMode()
    {
        _instance.GetComponent<Image>().color = _baseColor;
    }

    public int Number;
    public void SetNumber(int number)
    {
        //_instance.GetComponentInChildren<Text>().text = number.ToString();
        if (TryGetTextByName("Value", out Text text))
        {
            text.text = number.ToString();
            Number = number;
            for (int i = 1; i < 10; ++i)
            {
                if (TryGetTextByName($"Text_{i}", out Text textNumber))
                {
                    textNumber.text = "";
                }
            }
        }
    }

    public void SetSmallNumber(int number)
    {
        if (TryGetTextByName($"Text_{number}", out Text text)){
            text.text = number.ToString();

            if (TryGetTextByName("Value", out Text textValue))
            {
                textValue.text = "";
            }
        }
    }
}
