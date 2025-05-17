using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario7 : GameplayController
{
    protected override void Update() {
        base.Update();

    }

    // Do every action that has to be done at the stat of the scene
    public override void StartLevel() {

        //AudioControllerBasic.Instance.PlaySound("knock-door");
        DialogueController.Instance.PlayStory("intro");
        TasksController.Instance.ActivateTask("go-to-smartbuy"); 
    }
    
    
}
