using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;


    public void SetupCraftWindow(ItemDataEquipment _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterial.Count; i++)
        {
            if (_data.craftingMaterial.Count > materialImage.Length)
                Debug.LogWarning("you have more material amount than material slot in craft window");

            materialImage[i].sprite = _data.craftingMaterial[i].data.itemIcon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterial[i].stackSize.ToString();
            materialSlotText.color = Color.white;

        }

        itemIcon.sprite = _data.itemIcon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.CanCraft(_data , _data.craftingMaterial));
    }


}
