using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private bool isTransitioning = false;
    private Rigidbody m_rigidbody;
    private AudioSource m_audio;
    private float thrustThisFrame;
    private bool collisionsEnabled = true;

    [SerializeField] float rotThrust = default, upThrust = default, levelLoadDelay = default;
    [SerializeField] AudioClip engineSound = default, successSound = default, deathSound = default;
    [SerializeField] ParticleSystem successParticles = default, deathParticles = default;
    [SerializeField] ParticleSystem[] engineParticles = default;

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys();
        }
    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsEnabled = !collisionsEnabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || !collisionsEnabled) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning || !collisionsEnabled) return;

        switch (other.gameObject.tag)
        {
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        StartDissolving(true);
        m_audio.Stop();
        m_audio.PlayOneShot(deathSound);
        ToggleEngine(false);
        deathParticles.Play();
        Invoke("RestartLevel", levelLoadDelay);
    }

    private void StartDissolving(bool collidedWithEnemy)
    {
        if (collidedWithEnemy)
        {
            foreach (var item in transform.GetComponentsInChildren<Dissolve>())
            {
                item.StartDissolving(Dissolve.Colors.Red);
            }
        }
        else
        {
            foreach (var item in transform.GetComponentsInChildren<Dissolve>())
            {
                item.StartDissolving(Dissolve.Colors.Green);
            }
        }
    }

    private void ToggleEngine(bool toggle)
    {
        if (toggle)
        {
            foreach (var engine in engineParticles)
            {
                engine.Play();
            }
        }
        else
        {
            foreach (var engine in engineParticles)
            {
                engine.Stop();
            }
        }

    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        StartDissolving(false);
        m_audio.Stop();
        m_audio.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = (currentSceneIndex + 1 == SceneManager.sceneCountInBuildSettings ? 0 : ++currentSceneIndex);

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(Time.deltaTime * rotThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-Time.deltaTime * rotThrust);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        m_rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        m_rigidbody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) { ApplyThrust(); }
        else { StopApplyingThrust(); }
    }

    private void StopApplyingThrust()
    {
        m_audio.Stop();
        ToggleEngine(false);
    }

    private void ApplyThrust()
    {
        thrustThisFrame = upThrust * Time.deltaTime;
        m_rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!m_audio.isPlaying) { m_audio.PlayOneShot(engineSound); }
        ToggleEngine(true);
    }
}
