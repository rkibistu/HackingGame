using UnityEngine;

public class TeleportTo : InteractiveElement
{
    [SerializeField]
    private Transform _destination;
    [SerializeField]
    private int _timeToDisplayFeedback = 3;

    private Interpreter _interpreter;
    private void Start()
    {
        _interpreter = Interpreter.Instance;
    }
    override public void DoSomething()
    {
        if (_interpreter.AdvanceByAction("teleport_parking_lot"))
        {
            GameplayScenario3.Instance.TeleportTo(_destination);
            Destroy(gameObject);
        }
        else
        {
            UIController.Instance.ShowAndSetGeneralFeedbackPanel("You have to set up everything before leaving!", _timeToDisplayFeedback);
        }

    }
}
