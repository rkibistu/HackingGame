using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TerminalManagerExample : MonoBehaviour
{
    [SerializeField]
    public GameObject _directoryLine;
    [SerializeField]
    public GameObject _responseLine;

    [SerializeField]
    private TMP_InputField _terminalInput;
    [SerializeField]
    private GameObject _userInputLine;
    [SerializeField]
    private ScrollRect _scrollRect;
    [SerializeField]
    private GameObject _messageList; // container of all messages

    [SerializeField]
    [Tooltip("When more lines are added to terminal we don't want to snap to last one. We want  asmooth transition. This controls the speed of the scrolling transition.")]
    private int _scrollToBottomSpeed = 1200;
    [SerializeField]
    [Tooltip("Cmd line height + all paddings. This is used to rescale the scroll rectangle. It has to be the value of total height of a cmd line.")]
    private float _rectGrowValue = 35.0f;

    private InterpreterExample _interpreter;

    private void Start()
    {
        _interpreter = new InterpreterExample();

        RefocusInputField();
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScrollToBottom(1);
        }
    }

    private void OnGUI()
    {
        //If user typed text and pressed enter
        if(_terminalInput.isFocused && _terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            //Store whatever the user typed
            string userInput = _terminalInput.text;

            //Clear the input field
            ClearInputField();

            //Instantiate gameobject with directory prefix (the text that shows current directory)
            AddDirectoryLine(userInput);

            //Add the interpretation lines (the response of the command)
            int lines = AddInterpreterLines(_interpreter.Interpret(userInput));

            //Move the userInputLine to the end
            _userInputLine.transform.SetAsLastSibling();

            //Refocus the input field (so the user doesn't have to reselect the field to type)
            RefocusInputField();

            //Scroll to the bottom of the messages list
            ScrollToBottom(lines);
        }
    }

    private void ClearInputField()
    {
        _terminalInput.text = "";
    }

    private void AddDirectoryLine(string userInput)
    {
        //Resizing the command line container, so the scrollRect doesn't throw a fit
        Vector2 messageListSize = _messageList.GetComponent<RectTransform>().sizeDelta;
        _messageList.GetComponent<RectTransform>().sizeDelta = new Vector2(messageListSize.x, messageListSize.y + _rectGrowValue);

        //Isntantiate the directory line
        GameObject directoryLineObj = Instantiate(_directoryLine, _messageList.transform);

        //Set its child index -> so it appears last in the list
        directoryLineObj.transform.SetAsLastSibling();

        //Set the text of this new gameobject
        //directory line has 2 childs (directory text and userinput text)
        directoryLineObj.GetComponentsInChildren<TextMeshProUGUI>()[1].text = userInput;
    }

    int AddInterpreterLines(List<string> lines)
    {
        for(int i = 0; i < lines.Count; i++)
        {
            //Instantiate the response line
            GameObject responseLineObj = Instantiate(_responseLine, _messageList.transform);

            //Set last in list
            responseLineObj.transform.SetAsLastSibling();

            //Get the size of the message list and resize
            Vector2 messageListSize = _messageList.GetComponent<RectTransform>().sizeDelta;
            _messageList.GetComponent<RectTransform>().sizeDelta = new Vector2(messageListSize.x, messageListSize.y + _rectGrowValue);

            //Set the text of this line
            responseLineObj.GetComponentInChildren<TextMeshProUGUI>().text = lines[i];
        }

        return lines.Count;
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
}
