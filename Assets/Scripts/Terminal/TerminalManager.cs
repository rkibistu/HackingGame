using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{
    [Tooltip("The name of the terminal. It is a uniq identifier. Must be the same as in scenario json so an assosiacion can be made")]
    [SerializeField]
    private string _terminalName;

    [SerializeField]
    private GameObject _line;
    [Tooltip("Line used to calculate things like: number of chars per line. It is disabled all time. The script will enable and disable it, the suer can't see it.")]
    [SerializeField]
    private GameObject _helperLine;
    [SerializeField]
    private TextMeshProUGUI _helperLineText;

    // Only one exist at a tiem on terminal
    [SerializeField]
    private GameObject _inputLine;
    [SerializeField]
    private TMP_InputField _terminalInput;
    [SerializeField]
    private TextMeshProUGUI _directoryPathText;

    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private GameObject _linesContainer; // container of all lines (including input line)
    private RectTransform _linesContainerRectTranform;

    [SerializeField]
    [Tooltip("When more lines are added to terminal we don't want to snap to last one. We want  asmooth transition. This controls the speed of the scrolling transition.")]
    private int _scrollToBottomSpeed = 1200;
    [SerializeField]
    [Tooltip("Cmd line height + all paddings. This is used to rescale the scroll rectangle. It has to be the value of total height of a cmd line.")]
    private float _rectGrowValue = 35.0f;

    private List<string> _linesContent = new List<string>();
    private int _linesContentIndex = 0;

    private Interpreter _newInterpreter;

    // Used for fast acces to previous input by pressing specific keys
    private LinkedList<string> _inputHistory = new LinkedList<string>();
    private LinkedListNode<string> _inputHistoryCurrentNode = null;

    // Used to test for window resize
    private int _windowWidth;
    private int _windowHeight;
    private int _charsPerLine;

    public string Name { get => _terminalName; private set { } }

    private void Awake()
    {
    }

    private void Start()
    {
        _linesContainerRectTranform = _linesContainer.GetComponent<RectTransform>();
        _newInterpreter = Interpreter.Instance;

        RefocusInputField();

        _windowWidth = Screen.width;
        _windowHeight = Screen.height;

        CalculateCharactersPerLine();
    }

    private void Update()
    {


        if (_windowWidth != Screen.width || _windowHeight != Screen.height)
        {
            _windowWidth = Screen.width;
            _windowHeight = Screen.height;

            //Recalculate how many characters fill the line
            CalculateCharactersPerLine();
        }

        if (_terminalInput.isFocused && Input.GetKeyDown(KeyCode.UpArrow))
        {
            UsePreviousInput();
        }
        if (_terminalInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow))
        {
            UseNextInput();
        }
        if (_terminalInput.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {

            UseAutoComplete();
        }
    }

    private void OnGUI()
    {
        //If user typed text and pressed enter
        if (_terminalInput.isFocused && _terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Store whatever the user typed
            string userInput = _terminalInput.text;
            ResetHistorySearch();
            _inputHistory.AddFirst(userInput);


            //Clear the input field
            ClearInputField();

            //Add input to lines list that must be displayed
            string a = _directoryPathText + userInput;
            AddContent(_directoryPathText.text + userInput);

            //Interpret the input and add the result to lines list that must be displayed
            Interpret(userInput);

            //Display new lines added
            DisplayLinesContent();

            //Move the inputLine to the end
            _inputLine.transform.SetAsLastSibling();

            //Refocus the input field (so the user doesn't have to reselect the field to type)
            RefocusInputField();

            //Scroll to the bottom of the messages list
            ScrollToBottom(1);
        }


    }

    private void ClearInputField()
    {
        _terminalInput.text = "";
    }

    private void AddContent(string content)
    {
        _linesContent.Add(content);
    }

    private int DisplayLinesContent()
    {
        int addedLinesCount = 0;
        for (int i = _linesContentIndex; i < _linesContent.Count; i++)
        {
            //Instantiate lines for every content (one content can fill multiple lines)
            DisplayOneContent(_linesContent[i]);
            addedLinesCount++;
        }

        _linesContentIndex += addedLinesCount;
        return addedLinesCount;
    }

    private int DisplayOneContent(string text)
    {
        int linesNeeded = Mathf.CeilToInt((float)text.Length / _charsPerLine);
        for (int i = 0; i < linesNeeded; i++)
        {
            GameObject responseLineObj = Instantiate(_line, _linesContainer.transform);

            //Set last in list
            responseLineObj.transform.SetAsLastSibling();

            //Get the size of the message list and resize
            Vector2 messageListSize = _linesContainer.GetComponent<RectTransform>().sizeDelta;
            _linesContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(messageListSize.x, messageListSize.y + _rectGrowValue);

            //Set the text of this line
            int start = i * _charsPerLine;
            int length = Mathf.Min(_charsPerLine, text.Length - start);
            responseLineObj.GetComponentInChildren<TextMeshProUGUI>().text = text.Substring(start, length);
        }

        return linesNeeded;
    }

    private void Interpret(string userInput)
    {
        List<string> responses = _newInterpreter.Interpret(userInput, _terminalName);

        for (int i = 0; i < responses.Count; i++)
        {
            _linesContent.Add(responses[i]);
        }
    }

    private void ScrollToBottom(int lines)
    {
        // Ensure the layout updates before starting the scroll
        Canvas.ForceUpdateCanvases();

        StartCoroutine(ScrollToBottomSmooth(lines));
    }

    private IEnumerator ScrollToBottomSmooth(int lines)
    {

        float targetPosition = 0f; // Target is the bottom

        // Smooth scroll by updating verticalNormalizedPosition over time
        while (_scrollRect.verticalNormalizedPosition > targetPosition)
        {
            _scrollRect.verticalNormalizedPosition = Mathf.MoveTowards(
                _scrollRect.verticalNormalizedPosition, targetPosition, _scrollToBottomSpeed * Time.deltaTime);
            yield return null;
        }

        // Set the final position to ensure it reaches exactly 0
        _scrollRect.verticalNormalizedPosition = targetPosition;
    }

    private void RefocusInputField()
    {
        //Refocus the input field (so the user doesn't have to reselect the field to type)
        _terminalInput.ActivateInputField();
        _terminalInput.Select();
    }

    // Populate the terminal input field with last command interpeted
    // Can be called recursively to acces even older commands
    private void UsePreviousInput()
    {

        if (_inputHistoryCurrentNode == null)
        {
            _inputHistoryCurrentNode = _inputHistory.First;
        }
        else if (_inputHistoryCurrentNode.Next != null)
        {
            _inputHistoryCurrentNode = _inputHistoryCurrentNode.Next;
        }

        if (_inputHistoryCurrentNode != null)
        {
            _terminalInput.text = _inputHistoryCurrentNode.Value;
            _terminalInput.caretPosition = _terminalInput.text.Length;
        }
    }

    // Populate the terminal with the next command relative to the current 
    // command. The current command refers to the one updated by this method
    // and UsePreviousInput method
    private void UseNextInput()
    {
        if (_inputHistoryCurrentNode == null)
        {
            return;
        }

        if (_inputHistoryCurrentNode.Previous != null)
        {
            _inputHistoryCurrentNode = _inputHistoryCurrentNode.Previous;
        }

        if (_inputHistoryCurrentNode != null)
        {
            _terminalInput.text = _inputHistoryCurrentNode.Value;
            _terminalInput.caretPosition = _terminalInput.text.Length;
        }
    }

    // Reset the current command that is used to search trough input history
    // This must be called every time the user interpret a new command
    private void ResetHistorySearch()
    {
        _inputHistoryCurrentNode = null;
    }

    private void UseAutoComplete()
    {
        List<string> options = _newInterpreter.GetPossibleCommands(_terminalInput.text, _terminalName);
        if (options == null)
            return;

        if (options.Count == 1)
        {
            _terminalInput.text = options[0];
            _terminalInput.caretPosition = _terminalInput.text.Length;
        }
        else
        {
            //TODO (optional): treat the case when multiple commands are available with the same prefix
        }
    }

    private int CalculateCharactersPerLine()
    {
        _helperLine.SetActive(true);

        string testContent = "";
        _helperLineText.text = testContent;
        while (_helperLineText.preferredWidth < _linesContainerRectTranform.rect.width)
        {
            testContent += "_";
            _helperLineText.text = testContent;
        }

        _helperLine.SetActive(false);

        _charsPerLine = testContent.Length - 2;
        return _charsPerLine;
    }
}
