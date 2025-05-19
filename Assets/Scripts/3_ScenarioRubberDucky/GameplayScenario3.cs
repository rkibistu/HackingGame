using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GameplayScenario3 : GameplayController
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private CharacterController _playerCC;
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
    [Tooltip("How many ducky have to be placed before going home")]
    [SerializeField]
    private int _duckyTotalCount = 3;

    private bool _firstTeleport = true;

    private int _duckyPlaceCOunt = 0;
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

    public void Sleep()
    {
        StartCoroutine(SleepFadeEffect());
    }

    public void PlaceDucky()
    {
        _duckyPlaceCOunt++;
        if(_duckyPlaceCOunt >= _duckyTotalCount)
        {
            Interpreter.Instance.AdvanceByAction("scatter-rubber-duckies");
            TeleportHome();
        }
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
        Debug.Log("Pre teleport!");
        PreTeleport();
        yield return new WaitForSeconds(_teleportDelay);
        _playerCC.enabled = false;
        _player.transform.position = destination.position;
        _playerCC.enabled = true;
        Debug.Log("POST teleport!");
        PostTeleport();
    }

    private IEnumerator SleepFadeEffect()
    {
        _fadeEffect.StartFadeIn();
        yield return new WaitForSeconds(_teleportDelay);
        _fadeEffect.StartFadeOut();
    }


}
