using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    Rigidbody m_rigidbody;
    AudioSource m_audio;
    [SerializeField] float rotThrust = default, upThrust = default;
    [SerializeField] AudioClip mainEngine, success, death;
    private float rotationThisFrame;
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        m_audio.Stop();
        m_audio.PlayOneShot(death);
        Invoke("LoadFirstLevel", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        m_audio.Stop();
        m_audio.PlayOneShot(success);
        Invoke("LoadNextScene", 1f);
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
        else { m_audio.Stop(); }
    }

    private void ApplyThrust()
    {
        m_rigidbody.AddRelativeForce(Vector3.up * upThrust);
        if (!m_audio.isPlaying) { m_audio.PlayOneShot(mainEngine); }
    }
}
