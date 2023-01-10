using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInputAction : MonoBehaviour
{

    public GameObject characterInfo;
    public StatUIHolder statUIHolder;

    private void Start()
    {
        characterInfo.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            statUIHolder.UpdateStatUIs();
            characterInfo.SetActive(!characterInfo.activeSelf);
        }
    }

}