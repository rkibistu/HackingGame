using TMPro;
using UnityEngine;

public class InteractiveElement : MonoBehaviour
{
    [SerializeField]
    private string _hintDescription = "Press E to do";

    public string HintDescription
    {
        get { return _hintDescription; }
    }

    public virtual void DoSomething()
    {
        Debug.Log("Did soemthing!");
    }
}
