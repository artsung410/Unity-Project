using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmp : ItemBase
{
    public  GameObject  EmpEffectPrefeb;
    public  float       moveDistance = 0.2f;
    public  float       pingpongSpeed = 0.5f;
    public  float       rotateSpeed = 50;
    public  bool        isActiveEmp;

    private void Awake()
    {
        isActiveEmp = false;
    }

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while (true)
        {
            // y축을 기준으로 회전
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            // 처음 배치된 위치를 기준으로 y위치를 위, 아래로 이동
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(y, y + moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }

    // 아이템을 획득했을 때 호출되는 함수.
    public override void Use(GameObject entity)
    {
        isActiveEmp = true;

        GameManager.Instance.AddScore();

        GameObject particle = Instantiate(EmpEffectPrefeb, transform.position, Quaternion.identity);

        Destroy(particle.gameObject, 4f);

        Destroy(gameObject);
    }

}
