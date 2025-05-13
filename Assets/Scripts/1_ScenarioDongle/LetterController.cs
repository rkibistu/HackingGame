using UnityEngine;

public class LetterController : InteractiveElement
{

    private Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    override public void DoSomething() {
        UIController.Instance.SetActiveLetterPanel(true);
        //Destroy(gameObject);
    }

    public void PlaySlideInAniamtion() {
        _animator?.Play("SlideIn");
    }

    public void PlaySlideSound() {
        AudioControllerBasic.Instance.PlaySound("paper-slide");
    }
}
