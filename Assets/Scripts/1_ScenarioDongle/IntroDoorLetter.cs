using UnityEngine;

/***
 * This is a veri specific script. Used only once:
 * at the start of scenaio 1 when first letter is spawned
 * at the door.
 */

public class IntroDoorLetter : MonoBehaviour {
    [SerializeField]
    private LetterController _letter;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {

            _letter.PlaySlideInAniamtion();
            Destroy(gameObject);
        }
    }
}
