using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextProUtility
{
    public string text;

    public void Clear()
    {
        text = "";
    }

    public void StartColor(Color c)
    {
        text += "<color=#" + ColorUtility.ToHtmlStringRGB(c) + '>';
    }
    public void EndColor()
    {
        text += "</color>";
    }

    public void NewLine()
    {
        text += '\n';
    }

    public void Write(string newText)
    {
        text += newText + '\n';
    }
    public void Write(string newText, Color color)
    {
        StartColor(color);
        text += newText.ToString();
        EndColor();
        text += '\n';
    }
    public void Write(string newText, string value, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += value;
        EndColor();
        text += '\n';
    }

    public void Write(string newText, bool value)
    {
        text += newText + ": " + (value ? "[ON]" : "[OFF]") + '\n';
    }
    public void Write(string newText, bool value, Color colorOn, Color colorOff)
    {
        text += newText + ": ";
        StartColor(value ? colorOn : colorOff);
        text += (value ? "[ON]" : "[OFF]");
        EndColor();
        text += '\n';
    }

    public void Write(string newText, int value)
    {
        text += newText + ": " + value.ToString() + '\n';
    }
    public void Write(string newText, int value, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += value.ToString();
        EndColor();
        text += '\n';
    }

    public void Write(string newText, uint value)
    {
        text += newText + ": " + value.ToString() + '\n';
    }
    public void Write(string newText, uint value, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += value.ToString();
        EndColor();
        text += '\n';
    }

    public void Write(string newText, float value)
    {
        text += newText + ": " + value.ToString("F2") + '\n';
    }
    public void Write(string newText, float value, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += value.ToString("F2");
        EndColor();
        text += '\n';
    }

    public void Write(string newText, Vector3 vec)
    {
        text += newText + ": [X: " + vec.x.ToString("F2") + ",Y: " + vec.y.ToString("F2") + ",Z: " + vec.z.ToString("F2") + "]\n";
    }
    public void Write(string newText, Vector3 vec, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += "[X: " + vec.x.ToString("F2") + ", Y: " + vec.y.ToString("F2") + ", Z: " + vec.z.ToString("F2") + ']';
        EndColor();
        text += '\n';
    }
    public void Write(string newText, Vector3Int vec)
    {
        text += newText + ": [X: " + vec.x.ToString() + ",Y: " + vec.y.ToString() + ",Z: " + vec.z.ToString() + "]\n";
    }
    public void Write(string newText, Vector3Int vec, Color color)
    {
        text += newText + ": ";
        StartColor(color);
        text += "[X: " + vec.x.ToString() + ", Y: " + vec.y.ToString() + ", Z: " + vec.z.ToString() + ']';
        EndColor();
        text += '\n';
    }
}
