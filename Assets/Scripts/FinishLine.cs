using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] ParticleSystem finishParticles;
    [SerializeField] float restartDelay = 3f;

    void Awake()
    {
        SnowParticles.Configure(finishParticles, true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponentInParent<PlayerController>();
        var celebrationPosition = collision.bounds.center;
        if (player == null || !player.EndRun()) return;
        if (finishParticles != null)
        {
            finishParticles.transform.position = celebrationPosition;
            finishParticles.Play();
        }
        Invoke(nameof(ReloadScene), restartDelay);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
