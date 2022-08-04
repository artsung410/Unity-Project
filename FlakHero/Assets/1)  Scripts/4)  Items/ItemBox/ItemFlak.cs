using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFlak : ItemBase
{
    [SerializeField]
    private float moveDistance = 0.2f;

    [SerializeField]
    private float pingpongSpeed = 0.5f;

    [SerializeField]
    private float rotateSpeed = 50;

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

        int flakeCount = GameManager.Instance.calculateFlakCount();
        int maxFlakCount = 4;

        if (flakeCount < maxFlakCount)
        {
            FlakPool.GetObject();
        }

        ItemFlakPool.ReturnObject(this);
    }
}
