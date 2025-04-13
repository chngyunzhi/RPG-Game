using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler,ISaveManager
{
    private UI ui;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color LockedSkillColor;

    public bool unlock;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private Image skillImage;



    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }

    private void Awake()
    {
        
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot()); 
    }

    private void Start()
    {
        skillImage = GetComponent<Image>();

        ui = GetComponentInParent<UI>();

        skillImage.color = LockedSkillColor;

        if(unlock)
            skillImage.color = Color.white;

    }

    public void UnlockSkillSlot()
    {

        if(PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
            return; 

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlock == false)
            {
                return;
            }
        }
        for (int i = 0; i < shouldBeLocked.Length; ++i)
        {
            if (shouldBeLocked[i].unlock == true)
            {
                return;
            }

        }
        
        unlock = true;
        skillImage.color = Color.white;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowToolTip(skillDescription,skillName,skillCost);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        Debug.Log("load skill tree");

        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlock = value;
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlock);
        }
        else        
            _data.skillTree.Add(skillName, unlock);
        
    }
}
