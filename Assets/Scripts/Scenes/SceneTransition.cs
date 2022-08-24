using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class SceneTransition : MonoBehaviour
{
    [SerializeField] string scene;
    [SerializeField] string spawnTag;

    [Inject] LevelLoader _levelLoader;

    void OnTriggerEnter2D(Collider2D col)
    {
        var player = col.GetComponent<Player>();
        if (player == null) return;

        _levelLoader.LoadScene(scene, spawnTag);
        player.DisableControls();
    }
}