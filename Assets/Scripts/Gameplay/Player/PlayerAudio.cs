using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource damageTakenSource;
    [SerializeField] AudioClip damageTakenClip;

    public void DamageTaken()
    {
        damageTakenSource.PlayOneShot(damageTakenClip);
    }
}