using UnityEngine;

public class GetDungleElement : InteractiveElement
{
    [Tooltip("Interpreter that need the dongle")]
    [SerializeField]
    private InterpreterWifi _interpreter;
    override public void DoSomething()
    {
        _interpreter.DonglePlugged = true;
        Destroy(gameObject);
    }
}
