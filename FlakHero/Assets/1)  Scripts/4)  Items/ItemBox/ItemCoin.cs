using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : ItemBase
{
    [SerializeField]
    private float moveDistance = 0.2f;

    [SerializeField]
    private float pingpongSpeed = 0.5f;

    [SerializeField]
    private float rotateSpeed = 50;
    
    public GameObject meshBody;
    private AudioSource   audioSource;
    public  AudioClip     coinSound;
    private bool          isGetCoin = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while ( true )
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(y, y + moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }

    public override void Use(GameObject entity)
    {
        if (isGetCoin == false)
        {
            GameManager.Instance.AddScore();
            playSound(coinSound);
            meshBody.SetActive(false);
            Invoke("DeactiveObject", 3f);
            isGetCoin = true;
        }
    }

    private void playSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void DeactiveObject()
    {
        ItemCoinPool.ReturnObject(this);
    }
}
