using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Light2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;
        if (playerHealth.Health >= playerHealth.MaxHealth) return;

        playerHealth.Health += 1;
        source.PlayOneShot(clip);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Light2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject, 1);
    }
}