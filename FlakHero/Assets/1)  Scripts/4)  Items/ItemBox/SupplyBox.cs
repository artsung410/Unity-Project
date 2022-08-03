using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SupplyBox : MonoBehaviour
{
    public enum ItemType { Heal = 0, Coin, Emp, Flak }

    int randNum;

    private void Start()
    {
        int flakCount = GameManager.Instance.calculateFlakCount();

        randNum = (flakCount == 4) ? Random.Range(0, 3) : Random.Range(0, 4);
    }

    public void Destruction()
    {
        if (gameObject.transform.position.y < 0.3f)
        {
            SupplyBoxPool.ReturnObject(this);

            Vector3 itemDropPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            PickingItem().transform.position = itemDropPosition;
        }
    }

    private GameObject PickingItem()
    {
        GameObject pickItem = null;

        switch (randNum)
        {
            case 0:
                ItemHeal Heal = ItemHealPool.GetObject(); // Heal
                pickItem = Heal.gameObject;
                break;
            case 1:
                ItemCoin Coin = ItemCoinPool.GetObject(); // Coin
                pickItem = Coin.gameObject;
                break;
            case 2:
                ItemEmp Emp = ItemEmpPool.GetObject(); // Emp
                pickItem = Emp.gameObject;
                break;
            case 3:
                ItemFlak Flak = ItemFlakPool.GetObject(); // Flak
                pickItem = Flak.gameObject;
                break;
        }

        return pickItem;
    }
}
