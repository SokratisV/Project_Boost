﻿using UnityEngine;
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
    [SerializeField] ParticleSystem engineParticles1 = default, engineParticles2 = default, successParticles = default, deathParticles = default;
    [SerializeField] GameObject[] lights = default;

    void Start()
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

    private void StartDeathSequence()
    {
        isTransitioning = true;
        foreach (var light in lights)
        {
            light.SetActive(false);
        }
        m_audio.Stop();
        m_audio.PlayOneShot(deathSound);
        ToggleEngine(false);
        deathParticles.Play();
        Invoke("RestartLevel", levelLoadDelay);
    }

    private void ToggleEngine(bool toggle)
    {
        if (toggle)
        {
            engineParticles1.Play();
            engineParticles2.Play();
        }
        else
        {
            engineParticles1.Stop();
            engineParticles2.Stop();
        }

    }

    private void StartSuccessSequence()
    {
        isTransitioning = true;
        foreach (var light in lights)
        {
            light.SetActive(false);
        }
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

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
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
