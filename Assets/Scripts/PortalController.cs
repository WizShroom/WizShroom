using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PortalController : MonoBehaviour
{
    [Scene]
    public string sceneToGo;

    public bool activeOnTrigger = false;
    public string signalToActivate;
    public string signalToDisable;

    public VisualEffect portalEffect;
    public SpriteRenderer portalQuad;
    public BoxCollider portalCollider;

    private void Awake()
    {
        GameSignalHandler.Instance.OnSignalReceived += OnSignalReceived;
    }

    private void OnDestroy()
    {
        GameSignalHandler.Instance.OnSignalReceived -= OnSignalReceived;
    }

    private void Start()
    {
        if (activeOnTrigger)
        {
            portalCollider.enabled = false;
            portalEffect.Stop();
            portalQuad.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.LoadScene(sceneToGo);
        }
    }

    public void OnSignalReceived(GameObject source, string signal)
    {
        if (signal == signalToActivate)
        {
            portalCollider.enabled = true;
            portalEffect.Play();
            portalQuad.enabled = true;
        }

        if (signal == signalToDisable)
        {
            portalCollider.enabled = false;
            portalEffect.Stop();
            portalQuad.enabled = false;
        }
    }
}
