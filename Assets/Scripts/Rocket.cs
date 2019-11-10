using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private enum State { Alive, Dying, Transcending }
    private State state = State.Alive;
    private Rigidbody m_rigidbody;
    private AudioSource m_audio;
    private float rotationThisFrame, thrustThisFrame;

    [SerializeField] float rotThrust = default, upThrust = default, levelLoadDelay = default;
    [SerializeField] AudioClip engineSound = default, successSound = default, deathSound = default;
    [SerializeField] ParticleSystem engineParticles = default, successParticles = default, deathParticles = default;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_audio = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state != State.Alive) return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSoundSequence();
                break;
            default:
                StartDeathSoundSequence();
                break;
        }
    }

    private void StartDeathSoundSequence()
    {
        state = State.Dying;
        m_audio.Stop();
        m_audio.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSoundSequence()
    {
        state = State.Transcending;
        m_audio.Stop();
        m_audio.PlayOneShot(successSound);
        successParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToRotateInput()
    {
        m_rigidbody.freezeRotation = true;
        rotationThisFrame = Time.deltaTime * rotThrust;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        m_rigidbody.freezeRotation = false;
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else { m_audio.Stop(); engineParticles.Stop(); }
    }

    private void ApplyThrust()
    {
        thrustThisFrame = upThrust * Time.deltaTime;
        m_rigidbody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!m_audio.isPlaying) { m_audio.PlayOneShot(engineSound); }
        engineParticles.Play();
    }
}
