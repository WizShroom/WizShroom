using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public List<GameObject> propsToSpawn;

    public void SpawnProp()
    {
        if (propsToSpawn.Count == 0)
        {
            return;
        }

        float randomAngle = Random.Range(0f, 360f);

        Quaternion rotation = Quaternion.Euler(0, randomAngle, 0);

        GameObject randomProp = propsToSpawn[Random.Range(0, propsToSpawn.Count)];

        GameObject prop = Instantiate(randomProp, randomProp.transform.position + transform.position, rotation);
        prop.transform.SetParent(transform);

        RotatePropChildren(prop);
    }

    public void RotatePropChildren(GameObject parent)
    {
        List<GameObject> children = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children.Add(parent.transform.GetChild(i).gameObject);
        }

        foreach (GameObject child in children)
        {
            float randomAngle = Random.Range(0f, 360f);

            child.transform.rotation = Quaternion.Euler(0, randomAngle, 0);
        }
    }
}
