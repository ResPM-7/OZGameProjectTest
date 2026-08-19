using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDateBase : Singleton<ItemDateBase>
{
    public List<Item> itemDB = new List<Item>();

    //아이템 번호에 해당 데이터를 받는코드
    public Item GetItemByID(int id)
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (itemDB[i].itemID == id)
            {
                return itemDB[i]; 
            }
        }

        Debug.LogWarning($"{id}에 해당하는 아이템이 존재하지 않습니다");
        return null;
    }
}
