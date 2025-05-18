using UnityEngine;

public class RestartAnimationContinue : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _obj;

    private void Start()
    {
        if (_particleSystem == null)
            _particleSystem = GetComponent<ParticleSystem>();

        _particleSystem.Play();
    }

    private void Update()
    {
        if (_particleSystem != null && !_particleSystem.IsAlive())
        {
            //_particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            //_particleSystem.Play();
            _obj.SetActive(false);

        }
        if (_obj.activeInHierarchy == false)
            _obj.SetActive(true);
    }
}
