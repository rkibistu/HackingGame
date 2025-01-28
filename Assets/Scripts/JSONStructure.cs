using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

[Serializable]
public class Command
{
    // data from json
    public string input;
    public string outputType;
    public string output;
    public string outputFile;
    public bool required;
    public bool final;

    // Changed/used in execution, not present in json
    public bool executed = false;
}

[Serializable]
public class Action
{
    // data from json
    public string description;
    public bool required;
    public bool final;

    // Changed/used in execution, not present in json
    public bool executed = false;
}

[Serializable]
public class Phase
{
    public string name;
    public List<Command> commands;
    public Action action;
}

[Serializable]
public class Terminal
{
    // data from json
    public string name;
    public List<Phase> phases;

    // Changed/used in execution, not present in json
    public int currentPhase = 0;
}

[Serializable]
public class RootObject
{
    public List<Terminal> terminals;
}
