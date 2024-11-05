using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManager : MonoBehaviour
{

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

    private Interpreter _interpreter;

    private InterpreterBase _interpreterWifi;

    // Used to test for window resize
    private int _windowWidth;
    private int _windowHeight;
    private int _charsPerLine;

    private void Awake()
    {
        _linesContainerRectTranform = _linesContainer.GetComponent<RectTransform>();
        _interpreter = GetComponent<Interpreter>();
        _interpreterWifi = GetComponent<InterpreterWifi>();
    }

    private void Start()
    {
      

        RefocusInputField();

        _windowWidth = Screen.width;
        _windowHeight = Screen.height;

        CalculateCharactersPerLine();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            this.gameObject.SetActive(false);
        }

        if (_windowWidth != Screen.width || _windowHeight != Screen.height)
        {
            _windowWidth = Screen.width;
            _windowHeight = Screen.height;

            //Recalculate how many characters fill the line
            CalculateCharactersPerLine();
        }
    }

    private void OnGUI()
    {
        //If user typed text and pressed enter
        if (_terminalInput.isFocused && _terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Store whatever the user typed
            string userInput = _terminalInput.text;

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
        //List<string> responses = _interpreter.Interpret(userInput);
        List<string> responses = _interpreterWifi.Interpret(userInput);

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
