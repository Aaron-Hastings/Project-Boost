using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    [SerializeField] float maxMainThrust = 1000.0f;
    [SerializeField] float maxRotationThrust = 100.0f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem mainThrusterParticles;

    // CACHE - e.g. references for readability or speed
    Rigidbody rb;
    AudioSource audioSource;

    // STATE - e.g. bool isAlive

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            StartMainThruster();
        }
        else
        {
            StopMainThruster();
        }
    }

    void StartMainThruster()
    {
        Vector3 specificMainThrust = Time.deltaTime * maxMainThrust * Vector3.up;
        rb.AddRelativeForce(specificMainThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainThrusterParticles.isEmitting)
        {
            mainThrusterParticles.Play();
        }
    }

    private void StopMainThruster()
    {
        audioSource.Stop();
        mainThrusterParticles.Stop();
    }

    void ProcessRotation()
    {

        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            FireLeftRightThrusters();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            FireRightThruster();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            FireLeftThruster();
        }
        else
        {
            StopLeftRightThrusters();
        }

        void FireLeftRightThrusters()
        {
            Debug.Log("Pressed both Left and Right Rotation -- Doing Nothing");
            if (!rightThrusterParticles.isEmitting)
            {
                rightThrusterParticles.Play();
            }
            if (!leftThrusterParticles.isEmitting)
            {
                leftThrusterParticles.Play();
            }
        }

        void FireRightThruster()
        {
            ApplyRotation(1.0f);
            if (!rightThrusterParticles.isEmitting)
            {
                rightThrusterParticles.Play();
            }
            if (!leftThrusterParticles.isEmitting)
            {
                leftThrusterParticles.Stop(); // This should turn off the left thruster when A and D are pressed and then D is unpressed, but does not seem to work
            }
        }

        void FireLeftThruster()
        {
            ApplyRotation(-1.0f);
            if (!leftThrusterParticles.isEmitting)
            {
                leftThrusterParticles.Play();
            }
            if (!rightThrusterParticles.isEmitting)
            {
                rightThrusterParticles.Stop(); // See above
            }
        }

        void StopLeftRightThrusters()
        {
            rightThrusterParticles.Stop();
            leftThrusterParticles.Stop();
        }

        void ApplyRotation(float rotationDirection)
        {
            rb.freezeRotation = true; // Freezing physics rotation so we can manually rotate
            Vector3 specificRotationThrust = rotationDirection * maxRotationThrust * Time.deltaTime * Vector3.forward;
            transform.Rotate(specificRotationThrust);
            rb.freezeRotation = false; // Unfreezing physics rotation now that we are not controlling rotation
        }

    }
}
