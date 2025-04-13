using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackholeSkillController blackhole;


    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy, BlackholeSkillController _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackhole = _myBlackhole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();

    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
