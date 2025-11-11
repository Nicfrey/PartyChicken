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

    private bool _canSelect = false;

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
    
    private void CanSelect()
    {
        _canSelect = true;
    }

    public void StartSelecting()
    {
        Invoke(nameof(CanSelect),1.5f);
        textJoin.enabled = false;
        ArrowChange(true);
    }

    public void ArrowChange(bool left)
    {
        if (!IsAbleToChoose())
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
        if (!IsAbleToSelect())
            return;
        
        Debug.Log($"SelectionPlayerUI - FinishSelection: Player selected skin {currentSkin}");
        EnableCheckMark(true);
    }
    
    private void EnableCheckMark(bool enable)
    {
        leftArrowImage.enabled = !enable;
        rightArrowImage.enabled = !enable;
        checkMarkImage.enabled = enable;    }

    private void ChangeSkin(int previousSkin)
    {
        skins[previousSkin].gameObject.SetActive(false);
        skins[currentSkin].gameObject.SetActive(true);
    }

    private bool IsAbleToSelect()
    {
        return !checkMarkImage.enabled && _canSelect;
    }

    private bool IsAbleToChoose()
    {
        return !checkMarkImage.enabled;
    }

    public void ReturnSkin()
    {
        if (IsAbleToSelect())
            return;
        
        if (checkMarkImage.enabled)
        {
            EnableCheckMark(false);
        }
    }

    public int GetCurrentSkin()
    {
        return currentSkin;
    }
}
