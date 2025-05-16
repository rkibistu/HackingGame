using UnityEngine;

public class UnplugDucky : InteractiveElement {
    private Interpreter _interpreter;

    private void Start() {
        _interpreter = Interpreter.Instance;
    }
    override public void DoSomething() {
        if (_interpreter.AdvanceByAction("remove_rubber_ducky")) {
            //TasksController.Instance.ActivateTask("use-wifi-dongle");
            Destroy(gameObject);
        }
    }
}
