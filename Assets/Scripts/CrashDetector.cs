using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] float restartDelay = 1.8f;

    void Awake()
    {
        SnowParticles.Configure(crashParticles, false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Floor")) return;
        var player = GetComponent<PlayerController>();
        if (player == null || !player.EndRun()) return;
        if (crashParticles != null) crashParticles.Play();
        Invoke(nameof(ReloadScene), restartDelay);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
