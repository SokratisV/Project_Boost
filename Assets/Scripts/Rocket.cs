using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private bool _isTransitioning;
    private Rigidbody _rigidbody;
    private AudioSource _audio;
    private float _thrustThisFrame;
    private bool _collisionsEnabled = true;

    [SerializeField] float rotThrust, upThrust, levelLoadDelay;
    [SerializeField] AudioClip engineSound, successSound, deathSound;
    [SerializeField] ParticleSystem successParticles, deathParticles;
    [SerializeField] ParticleSystem[] engineParticles;
    [SerializeField] GameObject[] lights;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_isTransitioning)
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
            _collisionsEnabled = !_collisionsEnabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isTransitioning || !_collisionsEnabled) return;

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

    private void StartDeathSequence()
    {
        _isTransitioning = true;
        foreach (var light in lights)
        {
            light.SetActive(false);
        }

        _audio.Stop();
        _audio.PlayOneShot(deathSound);
        ToggleEngine(false);
        deathParticles.Play();
        Invoke(nameof(RestartLevel), levelLoadDelay);
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
        _isTransitioning = true;
        foreach (var light in lights)
        {
            light.SetActive(false);
        }

        _audio.Stop();
        _audio.PlayOneShot(successSound);
        successParticles.Play();
        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }

    private void LoadNextScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex =
            (currentSceneIndex + 1 == SceneManager.sceneCountInBuildSettings ? 0 : ++currentSceneIndex);

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            RotateManually(Time.deltaTime * rotThrust);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            RotateManually(-Time.deltaTime * rotThrust);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        _rigidbody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        _audio.Stop();
        ToggleEngine(false);
    }

    private void ApplyThrust()
    {
        _thrustThisFrame = upThrust * Time.deltaTime;
        _rigidbody.AddRelativeForce(Vector3.up * _thrustThisFrame);
        if (!_audio.isPlaying)
        {
            _audio.PlayOneShot(engineSound);
        }

        ToggleEngine(true);
    }
}