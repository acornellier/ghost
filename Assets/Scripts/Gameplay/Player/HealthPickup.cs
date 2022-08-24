using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip clip;

    Collider2D _collider;
    SpriteRenderer _renderer;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        var playerHealth = col.GetComponent<PlayerHealth>();
        if (playerHealth == null) return;
        if (playerHealth.Health >= playerHealth.MaxHealth) return;

        playerHealth.Health += 1;
        source.PlayOneShot(clip);

        _collider.enabled = false;
        _renderer.enabled = false;
        Destroy(gameObject, 1);
    }
}