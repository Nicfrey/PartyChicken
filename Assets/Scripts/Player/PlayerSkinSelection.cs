using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject[] skins;

    [SerializeField] private GameObject defaultSkin;
    public void SelectSkin(int index)
    {
        defaultSkin.SetActive(false);
        skins[index].SetActive(true);
    }
}
