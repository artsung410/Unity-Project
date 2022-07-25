using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 Start_Pos;
    void Start()
    {
        Start_Pos = transform.localPosition;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shake(0.1f, 0.1f));
        }
    }
    // duration : 진동시간
    // magnitude : 진동세기
    public IEnumerator Shake(float duration, float magnitude)
    {
        float timer = 0;
        while (timer <= duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitSphere * magnitude + Start_Pos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = Start_Pos;
    }
}
