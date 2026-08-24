using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] private GameObject InventoryUI;

    private bool isInventoryOpen = false;

    private void Start()
    {
        InventoryUI.SetActive(false);
    }


    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        InventoryUI.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
