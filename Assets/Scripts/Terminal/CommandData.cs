using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandData", menuName = "Scriptable Objects/CommandData")]
public class CommandDataSO : ScriptableObject
{
    public List<CommandEntry> commandEntries;

    public List<string> GetResponses(string command)
    {
        foreach (var entry in commandEntries)
        {
            if (entry.command == command)
            {
                return entry.responses;
            }
        }
        return null; // or return an empty list if preferred
    }
}

[System.Serializable]
public class CommandEntry
{
    public string command;
    public List<string> responses;
}