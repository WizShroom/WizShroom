using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInputAction : MonoBehaviour
{

    public StatUIHolder statUIHolder;

    private void Start()
    {
        UIHandler.Instance.DisableUIByTypeList(new List<UIType>(){
            UIType.CharacterInfo,
            UIType.Dialogue,
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            statUIHolder.UpdateStatUIs();
            UIHandler.Instance.ToggleUIType(UIType.CharacterInfo);
        }
    }

}