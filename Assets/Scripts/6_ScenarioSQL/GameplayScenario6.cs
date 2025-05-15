using JetBrains.Annotations;
using ScenarioSQL;
using TMPro;
using UnityEngine;

public class GameplayScenario6 : GameplayController
{
    [SerializeField]
    private float _personalBalance = 100;

    public float PersonalBalance { get { return _personalBalance; } set { _personalBalance = value; } }

    public static new GameplayScenario6 Instance { get; private set; }

    protected override void Awake() {
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
