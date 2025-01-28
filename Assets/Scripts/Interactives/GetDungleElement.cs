using UnityEngine;

public class GetDungleElement : InteractiveElement {
    private NewInterpreter _interpreter;

    private void Start() {
        _interpreter = NewInterpreter.Instance;
    }
    override public void DoSomething() {
        if (_interpreter.AdvanceByAction("plug_dongle")) {
            Destroy(gameObject);
        }
    }
}
