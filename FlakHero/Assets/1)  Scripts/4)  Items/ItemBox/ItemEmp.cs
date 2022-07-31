using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmp : ItemBase
{
    public  float       moveDistance = 0.2f;
    public  float       pingpongSpeed = 0.5f;
    public  float       rotateSpeed = 50;
    public  GameObject  EmpPrefab;
    private IEnumerator Start()
    {
        float y = transform.position.y;

        while (true)
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
        GameManager.Instance.AddScore();
        Instantiate(EmpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
