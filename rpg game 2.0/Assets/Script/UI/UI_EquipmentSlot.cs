using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString(); 
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item == null || item.data == null) 
            return;

        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        ui.itemToolTips.HideToolTip();

        CleanUpSlot();
    }
}
