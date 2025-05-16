using UnityEngine;

public class TeleportOnTouch : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject playerObject; // drag PlayerCapsule or Player here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerObject.transform.position = teleportTarget.position;
        }
    }
}
