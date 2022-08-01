using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
	public float despawnTime = 2.0f;
	public float lightDuration = 0.02f;

	[Header("Light")]
	public Light lightFlash;

	[Header("Audio")]
	public AudioClip[] explosionSounds;
	public AudioSource audioSource;

	void Start()
	{
		StartCoroutine("DeactiveTimer");
		StartCoroutine("LightFlash");

		audioSource.clip = explosionSounds[Random.Range(0, explosionSounds.Length)];
		audioSource.Play();
	}

	private IEnumerator LightFlash()
	{
		//Show the light
		lightFlash.GetComponent<Light>().enabled = true;
		//Wait for set amount of time
		yield return new WaitForSeconds(lightDuration);
		//Hide the light
		lightFlash.GetComponent<Light>().enabled = false;
	}

	private IEnumerator DeactiveTimer()
	{
		//Destroy the explosion prefab after set amount of seconds
		yield return new WaitForSeconds(despawnTime);
		ProjectileExplosionPool.ReturnObject(this);
	}
}
