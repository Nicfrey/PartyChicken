using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class SelectionPlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textJoin;
    [SerializeField] private Image leftArrowImage;
    [SerializeField] private Image rightArrowImage;
    [SerializeField] private Image checkMarkImage;
    
    [SerializeField] private GameObject[] skins;
    private int currentSkin = -1;

    private void Awake()
    {
        leftArrowImage.enabled = false;
        rightArrowImage.enabled = false;
        checkMarkImage.enabled = false;
        
        // Check which skin is active
        int index = 0;
        while (currentSkin == -1 && index < skins.Length)
        {
            if (skins[index].activeInHierarchy)
            {
                currentSkin = index;
            }
            ++index;
        }

        if (currentSkin == -1)
        {
            Debug.LogError("SelectionPlayerUI: No skins selected");
        }
    }

    public void StartSelecting()
    {
        textJoin.enabled = false;
        ArrowChange(true);
    }

    public void ArrowChange(bool left)
    {
        if (checkMarkImage.enabled)
            return;

        int previousSkin = currentSkin;
        currentSkin += left ? -1 : 1;
        currentSkin = Mathf.Clamp(currentSkin, 0, skins.Length - 1);
        ChangeSkin(previousSkin);
        
        leftArrowImage.enabled = currentSkin > 0;
        rightArrowImage.enabled = currentSkin < skins.Length - 1;
    }

    public void FinishSelection()
    {
        leftArrowImage.enabled = false;
        rightArrowImage.enabled = false;
        checkMarkImage.enabled = true;
    }

    private void ChangeSkin(int previousSkin)
    {
        skins[previousSkin].gameObject.SetActive(false);
        skins[currentSkin].gameObject.SetActive(true);
    }

    public void ReturnSkin()
    {
        if(!checkMarkImage.enabled && gameObject.layer == LayerMask.NameToLayer("Player1"))
        {
            GameManager.Instance.ChangeState(GameState.MainMenu);
        }
        StartSelecting();
    }

    public int GetCurrentSkin()
    {
        return currentSkin;
    }
}
