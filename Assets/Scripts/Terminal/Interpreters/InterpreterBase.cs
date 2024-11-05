using System;
using System.Collections.Generic;
using UnityEngine;

public class InterpreterBase : MonoBehaviour
{
    protected Dictionary<string, Func<List<string>, List<string>>> _commandDictionary = new();

    protected List<string> _response = new();

    public List<string> Interpret(string input)
    {
        var (cmd, args) = SplitString(input);
        foreach (var arg in args)
        {

            Debug.Log(arg);
        }
        if (_commandDictionary.ContainsKey(cmd))
        {
            return _commandDictionary[cmd].Invoke(args);
        }
        return new List<string> { "Command is not recongnized." };
    }

    private (string cmd, List<string> args) SplitString(string line)
    {
        // Split the string by spaces
        string[] words = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Check if there is at least one word in the input
        if (words.Length == 0)
        {
            return (null, new List<string>()); // Return null and an empty list if no words are found
        }

        // Extract the first word
        string cmd = words[0];

        // Create a list with the rest of the words
        List<string> restOfWords = new List<string>(words.Length > 1 ? words[1..] : Array.Empty<string>());

        return (cmd, restOfWords);
    }
}

