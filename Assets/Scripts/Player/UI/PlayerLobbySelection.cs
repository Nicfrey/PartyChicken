using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLobbySelection : MonoBehaviour
{
    private SelectionPlayerUI selectionPlayerUI;
    public int SkinSelected { get; private set; }
    public bool Selected { get; private set; }

    public void GetSelectionPlayerUI()
    {
        SelectionPlayerUI[] selectionPlayerUIs = FindObjectsOfType<SelectionPlayerUI>();
        int index = 0;
        while (selectionPlayerUI == null && index < selectionPlayerUIs.Length)
        {
            if (selectionPlayerUIs[index].gameObject.layer == gameObject.layer)
            {
                selectionPlayerUI = selectionPlayerUIs[index];
            }
            index++;
        }

        if (selectionPlayerUI == null)
        {
            Debug.LogError("Can't find SelectionPlayerUI with the layer " + LayerMask.LayerToName(gameObject.layer));
        }
        else
        {
            selectionPlayerUI.StartSelecting();
        }
    }

    public void ActivateCameraPlayer()
    {
        CinemachineVirtualCamera[] cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>(true);
        int index = 0;
        while (cinemachineVirtualCameras != null && index < cinemachineVirtualCameras.Length)
        {
            if (cinemachineVirtualCameras[index].gameObject.layer == gameObject.layer)
            {
                cinemachineVirtualCameras[index].gameObject.SetActive(true);
            }
            ++index;
        }
    }

    public void SelectSkin(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(selectionPlayerUI != null)
            {
                selectionPlayerUI.FinishSelection();
                SkinSelected = selectionPlayerUI.GetCurrentSkin();
                Selected = true;
            }
        }
    }

    public void ChangeSkin(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        int axis = (int) context.ReadValue<float>();
        if (axis == 0)
            return;
        
        bool left = axis < 0;
        if (selectionPlayerUI != null)
            selectionPlayerUI.ArrowChange(left);
    }

    public void ReturnSkin(InputAction.CallbackContext context)
    {
        if (context.performed && selectionPlayerUI != null)
        {
            selectionPlayerUI.ReturnSkin();
            Selected = false;
        }
    }
}
