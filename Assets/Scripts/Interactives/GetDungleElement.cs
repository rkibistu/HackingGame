using UnityEngine;

public class GetDungleElement : InteractiveElement {
    private Interpreter _interpreter;

    private void Start() {
        _interpreter = Interpreter.Instance;
    }
    override public void DoSomething() {
        if (_interpreter.AdvanceByAction("plug_dongle")) {
            Destroy(gameObject);
        }
    }
}
