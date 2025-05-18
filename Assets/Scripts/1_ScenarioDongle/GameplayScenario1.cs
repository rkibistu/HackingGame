using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario1 : GameplayController
{
    public static new GameplayScenario1 Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }
    protected override void Update() {
        base.Update();

    }

    // Do every action that has to be done at the stat of the scene
    public override void StartLevel() {

        AudioControllerBasic.Instance.PlaySound("knock-door");
        DialogueController.Instance.PlayStory("intro");
        TasksController.Instance.ActivateTask("check-door"); 
    }
    
    
}
