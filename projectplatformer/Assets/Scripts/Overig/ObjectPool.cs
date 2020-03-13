using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("Pool name")]
    [SerializeField] public string poolName = "Pool";
    [Space]
    [Header("Pool settings")]
    [SerializeField] private int poolSize;
    [Tooltip("Ammount of items added when pool is empty")]
    [SerializeField] private int emptyIncrement;
    [SerializeField] private GameObject poolObject;


    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        addItemsToPool(poolSize);
    }

    private void addItemsToPool(int ammount)
    {
        for (int i = 0; i < ammount; i++)
        {
            GameObject item = Instantiate(poolObject, transform);
            pool.Add(item);
            item.SetActive(false);
        }
    }

    public GameObject getPoolItem(Vector3 pos, Quaternion rotation, Transform parent = null)
    {
        if (pool.Count <= 0)
        {
            addItemsToPool(emptyIncrement);
            print($"{emptyIncrement}x items added to pool");
        }
        GameObject item = pool[0];
        pool.RemoveAt(0);
        item.transform.position = pos;
        item.transform.rotation = rotation;
        if (parent == null) parent = transform;
        item.transform.parent = parent;
        item.SetActive(true);
        return item;
    }

    public void returnPoolItem(GameObject item)
    {
        pool.Add(item);
        item.SetActive(false);
        item.transform.parent = transform;
    }

    public void returnPoolItem(GameObject item, float time)
    {
        StartCoroutine(timerReturn(item, time));
    }
    private IEnumerator timerReturn(GameObject item, float _time)
    {
        yield return new WaitForSeconds(_time);
        if (item.active) returnPoolItem(item);
    }
}
