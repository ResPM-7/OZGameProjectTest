using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ItemDateBase : MonoBehaviour
{
    public static ItemDateBase instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public List<Item> itemDB = new List<Item>();
}
