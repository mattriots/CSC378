using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpSounds : MonoBehaviour
{
    public AudioClip[] jumpSounds; // Drag your audio clips here in inspector
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Assuming you use the "Jump" button for jumping
        {
            Jump();
        }
    }

    void Jump()
    {
        // Here goes your code to make the player jump

        PlayRandomJumpSound();
    }

    void PlayRandomJumpSound()
    {
        int index = Random.Range(0, jumpSounds.Length); // Choose a random index
        AudioClip clip = jumpSounds[index]; // Get the audio clip at the random index
        audioSource.PlayOneShot(clip); // Play the audio clip
    }
}
