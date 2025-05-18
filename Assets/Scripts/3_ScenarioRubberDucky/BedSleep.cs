using UnityEngine;

public class BedSleep : InteractiveElement
{
    [SerializeField]
    private int _timeToDisplayFeedback = 3;
    private Interpreter _interpreter;
    private void Start()
    {
        _interpreter = Interpreter.Instance;
    }
    override public void DoSomething()
    {
        if (_interpreter.AdvanceByAction("return-home-and-sleep"))
        {
            GameplayScenario3.Instance.Sleep();
            Destroy(gameObject);
        }
        else
        {
            UIController.Instance.ShowAndSetGeneralFeedbackPanel("Finish your mission before sleeping!", _timeToDisplayFeedback);
        }

    }
}
