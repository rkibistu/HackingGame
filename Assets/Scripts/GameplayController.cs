using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private int _levelIndex = 0;

    public static GameplayController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    protected virtual void Start()
    {

    }
    protected virtual void  Update()
    {

    }

    public int GetCurrentLevelIndex() {
        return _levelIndex;
    }

    public virtual void StartLevel() {

    }
}
