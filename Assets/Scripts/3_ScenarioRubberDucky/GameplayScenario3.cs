using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayScenario3 : GameplayController
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Transform _parkingTeleportPoint;
    [SerializeField]
    private Transform _homeTeleportPoint;
    [SerializeField]
    private int _teleportDelay;
    [SerializeField]
    private FadeEffect _fadeEffect;
    [SerializeField]
    private GameObject _outsideLight;

    private bool _firstTeleport = true;

    public static new GameplayScenario3 Instance { get; private set; }

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
        DialogueController.Instance.PlayStory("intro");
        TasksController.Instance.ActivateTask("upload-payload"); 
    }
    
    public void TeleportToParkingSpot()
    {
        StartCoroutine(TeleportWithDelay(_parkingTeleportPoint));
    }
    public void TeleportHome()
    {
        
        StartCoroutine(TeleportWithDelay(_homeTeleportPoint));
    }
    public void TeleportTo(Transform destination)
    {
        StartCoroutine(TeleportWithDelay(destination));
    }

    private void PreTeleport()
    {
        _fadeEffect.StartFadeIn();

    }
    private void PostTeleport()
    {
        _fadeEffect.StartFadeOut();
        if(_firstTeleport == true)
        {
            _firstTeleport = false;
            _outsideLight.SetActive(true);
        }
        else
        {
            _outsideLight.SetActive(false);
        }
    }

    private IEnumerator TeleportWithDelay(Transform destination)
    {
        PreTeleport();
        yield return new WaitForSeconds(_teleportDelay);
        _player.transform.position = destination.position;
        PostTeleport();
    }
}
