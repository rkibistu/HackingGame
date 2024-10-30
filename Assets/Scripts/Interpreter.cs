using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    private Dictionary<string, string> _colors = new Dictionary<string, string>()
    {
        {"black",   "#021b21"},
        {"gray",     "#555d71"},
        {"purple",  "#d926ff"}
    };


    private List<string> _response = new List<string>();

    public List<string> Interpret(string userInput)
    {
        _response.Clear();

        //split by spaces
        string[] args = userInput.Split();

        if (args[0] == "help")
        {
            _response.Add("This is a help message to help you! Your welcome!");
            _response.Add("Second line to help youy! Double welcome!");
        }
        else if (args[0]== "scroll")
        {
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
            _response.Add("Scrollll");
        }
        else if (args[0] == "color")
        {

            _response.Add(ColorString("red", _colors["purple"]));
            ListEntry("response","ceva ceva ceva");
        }
        else if (args[0] == "ascii")
        {
            LoadTitle("ascii.txt", "purple", 2);
        }
        else
        {
            _response.Add("Command not recognized!");
        }

        return _response;
    }

    private string ColorString(string s, string color)
    {
        string leftTag = "<color=" + color + ">";
        string rightTag = "</color>";

        return leftTag + s + rightTag;
    }

    private void ListEntry(string a, string b)
    {
        _response.Add(ColorString(a, _colors["purple"]) + ": " + ColorString(b, _colors["gray"]));
    }

    void LoadTitle(string filePath, string color, int padding) 
    {
        StreamReader file = new StreamReader(Path.Combine(Application.streamingAssetsPath, filePath));

        for(int i = 0; i < padding; i++)
        {
            _response.Add("");
        }

        while (!file.EndOfStream)
        {
            _response.Add(ColorString(file.ReadLine(), _colors[color]));
        }

        for (int i = 0; i < padding; i++)
        {
            _response.Add("");
        }

        file.Close();
    }
}
