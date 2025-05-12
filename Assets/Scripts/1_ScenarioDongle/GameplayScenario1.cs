using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario1 : GameplayController
{

    // Do every action that has to be done at the stat of the scene
    public override void StartLevel() {

        AudioControllerBasic.Instance.PlaySound("knock-door");
    }
}
