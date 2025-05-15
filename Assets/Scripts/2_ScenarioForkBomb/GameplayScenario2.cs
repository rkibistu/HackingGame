using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario2 : GameplayController
{
    protected override void Update() {
        base.Update();

    }

    // Do every action that has to be done at the stat of the scene
    public override void StartLevel() {

        DialogueController.Instance.PlayStory("intro");
    }
    
    
}
