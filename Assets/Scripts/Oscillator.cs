using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    // [SerializeField] [Range(0,1)] float movementFactor;
    [SerializeField] float frequency = 0.125f;

    float movementFactor;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        Debug.Log(startingPosition);
    }

    // Update is called once per frame
    void Update()
    {
        float w = 2 * Mathf.PI * frequency;
        float t = Time.time;
        float rawSineWave = Mathf.Sin(w*t);
        // movementFactor = (rawSineWave + 1f) / 2f;
        movementFactor = rawSineWave;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
