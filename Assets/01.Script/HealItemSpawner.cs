using System.Collections.Generic;
using UnityEngine;

public class HealItemSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> healItems = new List<GameObject>();
    [SerializeField] private float respawnTime;

    private float[] timers;

    private void Start()
    {
        timers = new float[healItems.Count];
    }

    private void Update()
    {
        for (int i = 0; i < healItems.Count; i++)
        {
            if (!healItems[i].activeSelf)
            {
                timers[i] += Time.deltaTime;

                if (timers[i] >= respawnTime)
                {
                    healItems[i].SetActive(true);
                    timers[i] = 0;
                }
            }
        }
    }
}
