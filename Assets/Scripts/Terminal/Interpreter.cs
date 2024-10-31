using NUnit.Framework.Internal.Commands;
using System.Collections.Generic;
using UnityEngine;

public class Interpreter : MonoBehaviour
{
    [Tooltip("Commands that are accessible all the time for this interpreter")]
    [SerializeField]
    private CommandDataSO _mainCommands;

    [Tooltip(
        "Every entry is a diffrent set of commands. " +
        "Every entry is unlocked when some achievements are made. " +
        "Phase value represents the level acces. Phase0 -> acces to first entry. Phase1 -> acces toi second. And so on."
        )]
    [SerializeField]
    private List<CommandDataSO> _phasesCommands = new();

    [SerializeField]
    private int phase = -1;

    public List<string> Interpret(string userInput)
    {
        List<string> response = _mainCommands.GetResponses(userInput);
        if (response == null)
        {
            for (int i = 0; i < _phasesCommands.Count; i++)
            {
                if (i > phase)
                    break;

                response = _phasesCommands[i].GetResponses(userInput);
                if (response != null)
                    break;
            }
        }
        if (response == null)
        {
            response = new List<string>
            {
                "Not recognized command"
            };
        }

        return response;
    }
}
