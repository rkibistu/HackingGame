using UnityEngine;

public class TurnOnElement : InteractiveElement
{
    [SerializeField]
    private GameObject _toEnable;
    override public void DoSomething() 
    {
        _toEnable.SetActive(true);
    }
}
