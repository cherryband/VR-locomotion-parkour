using UnityEngine;

public class Demolisher : MonoBehaviour
{
    public ParkourCounter parkourCounter;
    public AudioClip sndBadexplosion;

    void OnTriggerEnter(Collider other)
    {
        // These are for the game mechanism.
        if (other.CompareTag("destroyable"))
        {
            parkourCounter.destructionCount += 1;
            AudioSource.PlayClipAtPoint(sndBadexplosion, other.gameObject.transform.position);
            other.gameObject.SetActive(false);
        }
        // These are for the game mechanism.
    }
}
