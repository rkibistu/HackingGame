using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

/*
 * This class is responsible with interpreting the input of all terminals.
 */
public class Interpreter : MonoBehaviour {
    public enum OutputTypes {
        inline = 0,
        file
    }

    [SerializeField]
    private bool _debug = false;
    [SerializeField]
    private int _forcedPhase = 0;

    public static Interpreter Instance { get; private set; }

    [SerializeField]
    [Tooltip("The path to scenario data folder. The path should be relative to streaming assets folder")]
    private string _scenarioBasePath = "Scenario_1";
    [Tooltip("The file with scenario json data. The filename should be relative to ScenarioBasePath")]
    [SerializeField]
    private string _jsonFilenama = "scenario.json";

    private RootObject _scenario;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    void Start() {
        string filePath = Path.Combine(Application.streamingAssetsPath, _scenarioBasePath + "/" + _jsonFilenama);
        if (File.Exists(filePath)) {
            string jsonContent = File.ReadAllText(filePath);
            _scenario = JsonUtility.FromJson<RootObject>(jsonContent);
        }
        else {
            Debug.LogError("Scenario json file not found: " + filePath);
        }

        PrepareOutputsFromFiles();
    }

    // Get the output of the input command.
    // The method will look up in the commands list of terminal with the name terminalName
    // and will keep in mind the current phase of the terminal. Only the commands from
    // the current phase of the terminal are checked. Not from previous or future phases.
    public List<string> Interpret(string input, string terminalName) {

        Terminal terminal = _scenario.terminals.Find(t => t.name == terminalName);
        if (terminal == null) {
            Debug.LogError("MissConfiguration: tried to itnerpret a command from a terminal that doesn t exist. The name of the terminal may be wrong in unity or inside the scenario json");
            return new List<string> { "Command is not recongnized." };
        }
        int phaseToCheck = (_debug == true) ? _forcedPhase : terminal.currentPhase;
        Phase phase = terminal.phases[phaseToCheck];

        string commonPrefix;
        Command closerCommand = ChooseClosestCommand(input, phase, out commonPrefix);
         
        if (closerCommand == null) {
            return new List<string> { "Command is not recongnized." };
        }
        if (commonPrefix.Length == closerCommand.input.Length) {
            // Found the right command
            AdvanceScenario(closerCommand, phase, terminal);
            return PostProcessOutput(closerCommand.output);
        }

        // The command is only partially correct
        return new List<string> { "Command is partially recongnized.", "OK: " + commonPrefix };
    }

    // Get the actionName identifier (should be the same as in scenario json)
    // and advance to the next phase if all the requirements are met
    public bool AdvanceByAction(string actionName) {

        bool result = false;
        foreach(var terminal in _scenario.terminals) {
            var action = terminal.phases[terminal.currentPhase];
            if(action.name == actionName) {

                if (AdvanceRequirementsMet(terminal.phases[terminal.currentPhase])) {
                    terminal.currentPhase++;
                    result = true;
                    //don t break/return because maybe the same action is needed to advance in multiuple terminals/phases
                }
            }
        }

        return result;
    }

    // The output message is wrote in json or separate files.
    // It needs to be processed and converted to a list of strings,
    // every string representing a new line in terminal
    private List<string> PostProcessOutput(string output) {

        return new List<string>(output.Split(new string[] { "\r\n" }, StringSplitOptions.None));
    }

    // Choose the closest command from the phase specified by 
    // comparing the input and the input of every command from that phase
    private Command ChooseClosestCommand(string input, Phase phase, out string commonPrefix) {
        Command closerCommand = null;
        commonPrefix = "";

        string aux;
        foreach (var cmd in phase.commands) {
            aux = Helper.GetCommonPrefix(input, cmd.input);
            if (aux.Length > 0 && aux.Length >= commonPrefix.Length) {
                commonPrefix = aux;
                closerCommand = cmd;
            }
        }

        return closerCommand;
    }

    // If all the reuired comamnds from the specified phase were executed,
    // pass to the new phase of the scenario for the specified terminal
    private void AdvanceScenario(Command completedCommand, Phase phase, Terminal terminal) {

        completedCommand.executed = true;
        if(completedCommand.final == true) {
            
            if(AdvanceRequirementsMet(phase) == true) {
                terminal.currentPhase++;
            }
        }
    }

    private bool AdvanceRequirementsMet(Phase phase) {
        foreach (var cmd in phase.commands) {
            if (cmd.executed == false && cmd.required == true) {
                return false;
            }
        }
        return true;
    }


    // The scenario json can specify the ouput directly or can redirect to a file
    // This method reads all the files and populates the output variable of the command
    // with the content of the files. This is done so we can use the output variable
    // whenever we want to acces the output of a command
    private void PrepareOutputsFromFiles() {
        foreach (var terminal in _scenario.terminals) {
            foreach (var phase in terminal.phases) {
                foreach (var cmd in phase.commands) {
                    if (cmd.outputType == OutputTypes.file.ToString()) {
                        string filePath = Path.Combine(Application.streamingAssetsPath, _scenarioBasePath + "/" + cmd.outputFile);
                        if (File.Exists(filePath)) {
                            cmd.output = File.ReadAllText(filePath);
                        }
                        else {
                            Debug.LogError("Command output file not found: " + filePath);
                        }
                    }

                }
            }
        }
    }


}
