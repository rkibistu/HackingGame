using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class NewInterpreter : MonoBehaviour {
    public enum OutputTypes {
        inline = 0,
        file
    }

    [SerializeField]
    private bool _debug = false;
    [SerializeField]
    private int _forcedPhase = 0;

    public static NewInterpreter Instance { get; private set; }

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

    // Get the output of the input command
    // The method will look up in the commands list of terminal with the name terminalName
    // and will keep in mind the current phase of the terminal.
    // A terminal on phase 1 can't acces commands from phase 2 or higher.
    public List<string> Interpret(string input, string terminalName) {
        // Parcurge toate comenzile din terminal
        // Vezi comanda care are cele mai multe caractere in comun cu input-ul
        // Daca se potriveste perfect -> return output
        // Altfel intoarce un mesaj sugestiv. Eventual arata partea din comanda care a fost buna si partea care nu ?
        // Atentie ca comanda sa nu fie dintr-o faza la care suerul inca n are acces

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
        if (commonPrefix.Length == input.Length) {
            // Found the right command
            return PostProcessOutput(closerCommand.output);
        }

        // The command is only partially correct
        return new List<string> { "Command is partially recongnized.", "OK: " + commonPrefix };
    }

    private List<string> PostProcessOutput(string output) {

        return new List<string>(output.Split(new string[] { "\r\n" }, StringSplitOptions.None));
    }

    private Command ChooseClosestCommand(string input, Phase phase, out string commonPrefix) {
        Command closerCommand = null;
        commonPrefix = "";

        string aux;
        foreach (var cmd in phase.commands) {
            aux = Helper.GetCommonPrefix(input, cmd.input);
            if (aux.Length >= commonPrefix.Length) {
                commonPrefix = aux;
                closerCommand = cmd;
            }
        }

        return closerCommand;
    }

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
