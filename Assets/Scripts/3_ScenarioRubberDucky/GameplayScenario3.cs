using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario3 : GameplayController
{
    protected override void Update() {
        base.Update();

    }

    // Do every action that has to be done at the stat of the scene
    public override void StartLevel() {
        DialogueController.Instance.PlayStory("intro");
        TasksController.Instance.ActivateTask("upload-payload"); 
    }
    
    
}
