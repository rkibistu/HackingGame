using UnityEngine;

public class LetterController : InteractiveElement
{
    [SerializeField]
    private GameObject _toEnable;

    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    override public void DoSomething() {
        _toEnable.SetActive(true);
        Destroy(gameObject);
    }

    public void PlaySlideInAniamtion() {
        _animator?.Play("SlideIn");
    }

    public void PlaySlideSound() {
        AudioControllerBasic.Instance.PlaySound("paper-slide");
    }
}
