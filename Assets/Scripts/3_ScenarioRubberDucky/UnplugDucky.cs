using UnityEngine;

public class UnplugDucky : InteractiveElement {

    [SerializeField]
    private int _timeTiShowHint = 2;

    private Interpreter _interpreter;
    private void Start() {
        _interpreter = Interpreter.Instance;
    }
    override public void DoSomething() {
        if (_interpreter.AdvanceByAction("remove_rubber_ducky")) {
            //TasksController.Instance.ActivateTask("use-wifi-dongle");
            Destroy(gameObject);
        }
        else
        {
            UIController.Instance.ShowAndSetGeneralFeedbackPanel("You need to copy the payload first!", _timeTiShowHint);
        }
    }
}
