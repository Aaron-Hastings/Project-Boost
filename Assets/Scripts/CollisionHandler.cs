using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float disableControlsTime = 1;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    int maxSceneIdx;
    int currentSceneIdx;
    int nextSceneIdx;

    bool isTransitioning = false;
    bool collisionsDisabled = false;

    void Start()
    {
        maxSceneIdx = SceneManager.sceneCountInBuildSettings - 1;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
            if (collisionsDisabled)
            {
                Debug.Log("Collisions Disabled!");
            }
            else
            {
                Debug.Log("Collisions Enabled!");
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionsDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Hit Launch Pad");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        this.GetComponent<Movement>().enabled = false; 
        audioSource.Stop(); // Stops all sounds currently being played through audio component, e.g. mainEngine
        audioSource.PlayOneShot(success);
        successParticles.Play();

        Invoke("LoadNextLevel", disableControlsTime);

    }

    void StartCrashSequence()
    {
        // todo add particle effect upon crash
        isTransitioning = true;
        this.GetComponent<Movement>().enabled = false; 
        audioSource.Stop(); // Stops all sounds currently being played through audio component, e.g. mainEngine
        audioSource.PlayOneShot(crash);
        crashParticles.Play();

        Invoke("ReloadLevel", disableControlsTime);
    }

    void ReloadLevel()
    {
        currentSceneIdx = SceneManager.GetActiveScene().buildIndex; // This could be different for each collision

        Debug.Log("Crashed. Going back to start of level: " + currentSceneIdx);
        SceneManager.LoadScene(currentSceneIdx);
    }

    void LoadNextLevel()
    {
        currentSceneIdx = SceneManager.GetActiveScene().buildIndex; // This could be different for each collision

        if (currentSceneIdx == maxSceneIdx)
        {
            nextSceneIdx = 0; // Go back to start of game, i.e. beginning level 
            Debug.Log("Going back to start of game: " + nextSceneIdx);
        }
        else
        {
            nextSceneIdx = currentSceneIdx + 1;
            Debug.Log("Going to next level: " + nextSceneIdx);
        }
        SceneManager.LoadScene(nextSceneIdx);
    }
}
