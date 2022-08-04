using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    public float deactivateTime = 5.0f;          
    public float casingSpin = 1.0f;              
    public AudioClip[] audioClips;              

    private Rigidbody rigidbody3D;
    private AudioSource audioSource;
    private CasingPool Pool;

    public void Setup(CasingPool pool, Vector3 direction)
    {
        rigidbody3D = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        Pool = pool;


        rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        rigidbody3D.angularVelocity = new Vector3(Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin),
                                                  Random.Range(-casingSpin, casingSpin));

        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter (Collision collision)
    {
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);

        CasingPool.ReturnObject(this);
    }
}
