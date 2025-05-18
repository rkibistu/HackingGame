using System.Collections;
using UnityEngine;

public class CobaltWebsite : MonoBehaviour
{

    [SerializeField]
    private int _delay = 3;
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(EndLEvelWIthDelay());
    }

    private IEnumerator EndLEvelWIthDelay()
    {
        yield return new WaitForSeconds(_delay);
        Interpreter.Instance.AdvanceByAction("check-website");
        TasksController.Instance.Mark("outro");
    }
}
