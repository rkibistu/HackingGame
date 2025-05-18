using UnityEngine;

public class PlaceDucky : InteractiveElement
{
    override public void DoSomething()
    {
        GameplayScenario3.Instance.PlaceDucky();
        Destroy(gameObject);
    }
}
