using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{

    public float offDistance = 30f;

    GameObject target;
    Light ourLight;

    private void Start()
    {
        ourLight = GetComponent<Light>();
        target = GameController.Instance.GetGameObjectFromID("MushPlayer");
    }

    private void Update()
    {
        if (!target)
        {
            ourLight.enabled = false;
            this.enabled = false;
            return;
        }

        if (ourLight.enabled && Vector3.Distance(target.transform.position, transform.position) > offDistance)
        {
            ourLight.enabled = false;
        }
        else if (!ourLight.enabled && Vector3.Distance(target.transform.position, transform.position) <= offDistance)
        {
            ourLight.enabled = true;
        }
    }

}
