using UnityEngine;

public class TurnOnElement : InteractiveElement
{
    [SerializeField]
    private GameObject _toEnable;
    override public void DoSomething() 
    {
        Debug.Log("Did soemthing ELSE!");
        _toEnable.SetActive(true);
    }
}
