using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TypewriterEffect : MonoBehaviour
{
    [SerializeField]
    private float _charactersPerSecond = 20;
    [SerializeField]
    private float _interpunctuationDelayTime = 0.5f;
    [SerializeField]
    private bool _quickSkip;
    [SerializeField]
    [Min(1)] 
    private int _skipSpeedup = 5;
    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float _sendDoneDelay = 0.25f;

    public bool CurrentlySkipping { get; private set; }

    private TMP_Text _textBox;

    private int _currentVisibileCharacterIndex;
    private Coroutine _typewriterCorotuine;
    private bool _readyForNewText = true;

    private WaitForSeconds _simpleDelay;
    private WaitForSeconds _interpunctuationDelay;
    private WaitForSeconds _skipDelay;
    private WaitForSeconds _textboxFullEventDelay;

    public event Action CompleteTextRevealed;
    public event Action<char> CharacterRevealed;

    private void Awake()
    {
        _textBox = GetComponent<TMP_Text>();

        _simpleDelay = new WaitForSeconds(1 / _charactersPerSecond);
        _interpunctuationDelay = new WaitForSeconds(_interpunctuationDelayTime);

        CurrentlySkipping = false;
        _skipDelay = new WaitForSeconds(1 / (_charactersPerSecond * _skipSpeedup));

        _textboxFullEventDelay = new WaitForSeconds(_sendDoneDelay);
    }

    //private void OnEnable()
    //{
    //    TMPro_EventManager.TEXT_CHANGED_EVENT.Add(PrepareForNewText);
    //}

    //private void OnDisable()
    //{
    //    TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(PrepareForNewText);
    //}

    public void FeedText(string text)
    {
        _textBox.text = text;
        PrepareForNewText(null);
    }

    private void Update()
    {
        // PROBLEM here: skips wokrs. But not toghether with the idea of rpessing the same button
        // to go to the next line in gameplaymanager.
        // TODO later; skip is optional behaviour

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
        //        Skip();
        //}
    }

    public void PrepareForNewText(UnityEngine.Object obj)
    {
        if (!_readyForNewText)
            return;
        
        _readyForNewText = false;
        CurrentlySkipping = false;

        if (_typewriterCorotuine != null)
            StopCoroutine(_typewriterCorotuine);

        _textBox.maxVisibleCharacters = 0;
        _currentVisibileCharacterIndex = 0;

        _typewriterCorotuine = StartCoroutine(TypeWriter());
    }

    private IEnumerator TypeWriter()
    {
        TMP_TextInfo textInfo = _textBox.textInfo;

        while (_currentVisibileCharacterIndex < textInfo.characterCount + 1)
        {

            var lastCharacterIndex = textInfo.characterCount - 1;
            if(_currentVisibileCharacterIndex == lastCharacterIndex)
            {
                _textBox.maxVisibleCharacters++;
                yield return _textboxFullEventDelay;
                _readyForNewText = true;
                CompleteTextRevealed?.Invoke();
                yield break;
            }

            char character = textInfo.characterInfo[_currentVisibileCharacterIndex].character;
            _textBox.maxVisibleCharacters++;

            if (!CurrentlySkipping &&
                   (character == '?' || character == '.' || character == ',' || character == ':' ||
                    character == ';' || character == '!' || character == '-'))
            {
                yield return _interpunctuationDelay;
            }
            else
            {
                yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
            }

            CharacterRevealed?.Invoke(character);
            _currentVisibileCharacterIndex++;
        }
    }

    public void Skip()
    {
        if (CurrentlySkipping)
            return;

        CurrentlySkipping = true;
        
        if (!_quickSkip )
        {
            StartCoroutine(SkipSpeedupReset());
            return;
        }

        if(_typewriterCorotuine != null)
            StopCoroutine(_typewriterCorotuine);
        _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
        _readyForNewText = true;
        CurrentlySkipping = false;
        CompleteTextRevealed?.Invoke();
    }

    private IEnumerator SkipSpeedupReset()
    {
        yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
        CurrentlySkipping = false;
    }
}
