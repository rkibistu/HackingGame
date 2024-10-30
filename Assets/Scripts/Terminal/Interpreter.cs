using System.Collections.Generic;
using UnityEngine;

public class Interpreter
{
    private List<string> _response = new List<string>();

    public List<string> Interpret(string userInput)
    {
        _response.Clear();



        return _response;
    }

    virtual protected void InternalInterpret(string userInput)
    {

    }

  
}
