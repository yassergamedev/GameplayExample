using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PickUp : MonoBehaviour
{
    public UnityEvent onPickUp;
    private AudioSource audioSource;
    public float destroyAfter = 0f;
    public float spawnTime = 20f;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(this.gameObject, spawnTime);
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            onPickUp.Invoke();
            StartCoroutine(DestroyAfterEvent());
        }
    }

    private IEnumerator DestroyAfterEvent()
    {
        yield return new WaitForEndOfFrame();

        transform.localScale = Vector3.zero;
        Destroy(gameObject, destroyAfter);
    }
}
