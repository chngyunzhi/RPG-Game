using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;      //drop how many item
    [SerializeField] private ItemData[] posibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < posibleDrop.Length; i++)
        {
            if(Random.Range(0,100) <= posibleDrop[i].dropChance)
                dropList.Add(posibleDrop[i]);
        }

        for (int i = 0; i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0,dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }

    protected void DropItem(ItemData _itemData)
    {
         GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVecocity = new Vector2(Random.Range(-5,5),Random.Range(12,15));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVecocity);
    }
}
