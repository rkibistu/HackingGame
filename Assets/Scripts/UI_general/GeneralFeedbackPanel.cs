using System.Collections;
using TMPro;
using UnityEngine;

public class GeneralFeedbackPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _visualsRoot;
    [SerializeField]
    private TextMeshProUGUI _text;

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void DisplayLimited(int timeToDisplay)
    {
        if (_visualsRoot.activeInHierarchy == true)
            return;

        _visualsRoot.SetActive(true);
        StartCoroutine(DisableWithDelay(timeToDisplay));
    }

    private IEnumerator DisableWithDelay(int timeToDisplay)
    {
        yield return new WaitForSeconds(timeToDisplay);
        _visualsRoot.SetActive(false);
    }
}
