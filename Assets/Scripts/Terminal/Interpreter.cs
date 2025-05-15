using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using static ScenarioJSONStructure;
using System.Text.RegularExpressions;
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
    private string _scenarioBasePath = "default_scenario_folder_path";
    [Tooltip("The file with scenario json data. The filename should be relative to ScenarioBasePath")]
    [SerializeField]
    private string _jsonFilenama = "scenario.json";

    private RootObject _scenario;

    private void Awake() {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

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

    void Start() {
        
    }


    // Check if the length of the common prefix is equal to the length of one of the alternatives commands
    // If it is, return true. Else, return false
    // This is used to check if the command is a partial match with one of the alternatives
    private bool CheckCloserPrefixLengthFromAlternatives(string input, List<string> alternatives) {
        foreach (var alternative in alternatives) {
            if (input == alternative) {
                return true;
            }
        }

        return false;
    }

    // Get the output of the input command.
    // The method will look up in the commands list of terminal with the name terminalName
    // and will keep in mind the current phase of the terminal. Only the commands from
    // the current phase of the terminal are checked. Not from previous or future phases.
    public List<string> Interpret(string input, string terminalName, out string newPromt) {
        //this will be changed to a specific value if the input triggered a change in terminal promt
        newPromt = null;

        Terminal terminal = _scenario.terminals.Find(t => t.name == terminalName);
        if (terminal == null) {
            Debug.LogError("MissConfiguration: tried to interpret a command from a terminal that doesn t exist. The name of the terminal may be wrong in unity or inside the scenario json");
            return new List<string> { "Command is not recongnized." };
        }
        int phaseToCheck = (_debug == true) ? _forcedPhase : terminal.currentPhase;
        Phase phase = terminal.phases[phaseToCheck];

        string commonPrefix;
        Command closerCommand = ChooseClosestCommand(input, phase, out commonPrefix);

        if (closerCommand == null) {
            return new List<string> { "Command is not recongnized." };
        }
        if (input == closerCommand.input
            || CheckCloserPrefixLengthFromAlternatives(commonPrefix, closerCommand.alternatives)) {
            // Found the right command

            //Try to advance to next phase if all requirements are meet
            AdvanceScenario(closerCommand, phase, terminal);

            // Some commands can finish/start tasks
            CheckForTasks(closerCommand);

            // Some commands can change the terminal promt
            //CheckForPromtChanging(closerCommand);
            newPromt = closerCommand.changePrompt;

            return PostProcessOutput(closerCommand.output);
        }
    
        // The command is only partially correct
        return new List<string> { "Command is partially recongnized.", "OK: " + commonPrefix };
    }

    // Mark as compelte or start a task if the current command specify it
    private void CheckForTasks(Command cmd) {
        if (cmd.taskIdToComplete != null) {
            TasksController.Instance.Mark(cmd.taskIdToComplete, true, true);
        }
        if (cmd.taskIdToStart != null) {
            TasksController.Instance.ActivateTask(cmd.taskIdToStart);
        }

        if (cmd.storyIdToStart != null) {
            DialogueController.Instance.SkipCurrentStoryCompletely();
            DialogueController.Instance.PlayStory(cmd.storyIdToStart);
        }
    }

    // Returns a list with all accesible commands that begin with
    // the string inputPrefix
    public List<string> GetPossibleCommands(string inputPrefix, string terminalName) {
        if (_debug)
            return GetPossibleCommandsWithArgs(inputPrefix, terminalName);
        else
            return GetPossibleCommandsNoArgs(inputPrefix, terminalName);
    }
    public List<string> GetPossibleCommandsWithArgs(string inputPrefix, string terminalName) {
        if (inputPrefix == null || inputPrefix == "")
            return null;

        List<string> result = new List<string>();

        Terminal terminal = _scenario.terminals.Find(t => t.name == terminalName);
        if (terminal == null) {
            Debug.LogError("MissConfiguration: tried to interpret a command from a terminal that doesn t exist. The name of the terminal may be wrong in unity or inside the scenario json");
            return new List<string> { "Command is not recongnized." };
        }
        int phaseToCheck = (_debug == true) ? _forcedPhase : terminal.currentPhase;
        Phase phase = terminal.phases[phaseToCheck];

        string aux;
        foreach (var cmd in phase.commands) {
            bool found = false;

            aux = Helper.GetCommonPrefix(inputPrefix, cmd.input);
            if (aux.Length == inputPrefix.Length) {
                result.Add(cmd.input + ";");
                found = true;
            }

            // Check if the command is in the alternatives list
            if (found == false) {
                foreach (var alternative in cmd.alternatives) {
                    aux = Helper.GetCommonPrefix(inputPrefix, alternative);
                    if (aux.Length == inputPrefix.Length) {
                        result.Add(alternative + ";");
                        break;
                    }
                }
            }
        }

        return result;
    }

    public List<string> GetPossibleCommandsNoArgs(string inputPrefix, string terminalName) {
        if (inputPrefix == null || inputPrefix == "")
            return null;

        if (Helper.HasOnlyOneWord(inputPrefix) == false)
            return null;

        List<string> result = new List<string>();

        Terminal terminal = _scenario.terminals.Find(t => t.name == terminalName);
        if (terminal == null) {
            Debug.LogError("MissConfiguration: tried to interpret a command from a terminal that doesn t exist. The name of the terminal may be wrong in unity or inside the scenario json");
            return new List<string> { "Command is not recongnized." };
        }
        int phaseToCheck = (_debug == true) ? _forcedPhase : terminal.currentPhase;
        Phase phase = terminal.phases[phaseToCheck];

        string aux;
        foreach (var cmd in phase.commands) {
            aux = Helper.GetCommonPrefix(inputPrefix, cmd.input);
            if (aux.Length == inputPrefix.Length) {
                // Add only the cmd, not the args too
                string toAdd = Helper.GetFirstWord(cmd.input);
                if (!result.Contains(toAdd)) {
                    result.Add(toAdd);
                }
            }

            //DONT need this, alternatives start with the same word(cmd) always
            // Check if the command is in the alternatives list
            //foreach (var alternative in cmd.alternatives) {
            //    aux = Helper.GetCommonPrefix(inputPrefix, alternative);
            //    if (aux.Length == inputPrefix.Length) {
            //        result.Add(alternative);
            //    }
            //}
        }

        return result;
    }

    // Get the actionName identifier (should be the same as in scenario json)
    // and advance to the next phase if all the requirements are met
    public bool AdvanceByAction(string actionName) {

        bool result = false;
        foreach (var terminal in _scenario.terminals) {
            var action = terminal.phases[terminal.currentPhase];
            if (action.name == actionName) {

                if (AdvanceRequirementsMet(terminal.phases[terminal.currentPhase])) {
                    terminal.currentPhase++;
                    result = true;
                    //don t break/return because maybe the same action is needed to advance in multiuple terminals/phases
                }
            }
        }

        return result;
    }

    public int GetPhase(string terminalName) {
        foreach (var terminal in _scenario.terminals) {
            if (terminal.name == terminalName) {
                return terminal.currentPhase;
            }
        }

        return -1;
    }

    // Get the default promt of a specific terminal
    public string GetDefaultPromt(string terminalName) {
        foreach (var terminal in _scenario.terminals) {
            if (terminal.name == terminalName) {
                return terminal.prompt;
            }
        }

        return null;
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

        //remove extra spaces
        //input = Regex.Replace(input.Trim(), @"\s+", " ");

        string aux;
        foreach (var cmd in phase.commands) {
            //check for exact match
            if (Helper.IsMatching(input, cmd.input)) {
                commonPrefix = input;
                return cmd;
            }
            else {
                foreach(var alternative in cmd.alternatives) {
                    if(Helper.IsMatching(input, alternative)) {
                        commonPrefix = input;
                        return cmd;
                    }
                }
            }


            // No match -> keep the longest common prefix
            aux = Helper.GetCommonPrefix(input, cmd.input);
            if (aux.Length > 0 && aux.Length >= commonPrefix.Length) {
                commonPrefix = aux;
                closerCommand = cmd;
            }
            // Check if the command is in the alternatives list
            foreach (var alternative in cmd.alternatives) {
                aux = Helper.GetCommonPrefix(input, alternative);
                if (aux.Length > 0 && aux.Length >= commonPrefix.Length) {
                    commonPrefix = aux;
                    closerCommand = cmd;
                }
            }
        }

        return closerCommand;
    }

    // If all the reuired comamnds from the specified phase were executed,
    // pass to the new phase of the scenario for the specified terminal
    private void AdvanceScenario(Command completedCommand, Phase phase, Terminal terminal) {

        completedCommand.executed = true;
        if (completedCommand.final == true) {

            if (AdvanceRequirementsMet(phase) == true) {
                terminal.currentPhase++;
                //check for tasl completion using phase name
            }
        }
        terminal.currentPhase = Mathf.Min(terminal.currentPhase, terminal.phases.Count - 1);
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
