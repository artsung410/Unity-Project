using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoin : ItemBase
{
    //[SerializeField]
    //private GameObject CoinEffectPrefeb;

    [SerializeField]
    private float moveDistance = 0.2f;

    [SerializeField]
    private float pingpongSpeed = 0.5f;

    [SerializeField]
    private float rotateSpeed = 50;

    private IEnumerator Start()
    {
        float y = transform.position.y;

        while ( true )
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

    public override void Use(GameObject entity)
    {
        // 이벤트 호출
        GameManager.Instance.AddScore();

        // 플레이어 상태 업데이트
        //entity.GetComponent<Status>().IncreaseHP(increaseHP);

        // 이펙트 효과, 사운드 재생
        //Instantiate(hpEffectPrefeb, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
