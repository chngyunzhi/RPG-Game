using UnityEngine;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine.Lumin;
using UnityEditor.Build;
using Unity.Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer sr;
    protected Player player;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;



    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material orgMat;

    [Header("Ailment Colour")]
    [SerializeField] private Color[] chillColour;
    [SerializeField] private Color[] igniteColour;
    [SerializeField] private Color[] shockColour;

    [Header("Ailment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit Fx")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;


    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;
        orgMat = sr.material;
    }

    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);


        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpTextPrefab,transform.position + positionOffset,Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;

    }



    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }


    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        Color orginalColour = sr.color;
        
        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.material = orgMat;
        sr.color = orginalColour;


    }

    private void RedColourBlink()
    {
        if (sr.color != Color.white) sr.color = Color.white;
        else sr.color = Color.red;
    }

    private void CancelColourChange()
    {
        CancelInvoke();
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void ShockFxFor(float _seconds)
    {
        shockFX.Play();
        InvokeRepeating("ShockColourFx", 0, .2f);
        Invoke("CancelColourChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFX.Play();
        InvokeRepeating("ChillColourFx", 0, .2f);
        Invoke("CancelColourChange", _seconds);
    }

    public void IgniteFxFor(float _seconds)
    {
        igniteFX.Play();
        InvokeRepeating("IgniteColourFx", 0, .2f);
        Invoke("CancelColourChange", _seconds);
    }

    private void IgniteColourFx()
    {
        if (sr.color != igniteColour[0])
            sr.color = igniteColour[0];
        else 
            sr.color = igniteColour[1];
    }

    private void ChillColourFx()
    {
        if (sr.color != chillColour[0])
            sr.color = chillColour[0];
        else
            sr.color = chillColour[1];

    }

    private void ShockColourFx()
    {
        if (sr.color != shockColour[0])
            sr.color = shockColour[0];
        else
            sr.color = shockColour[1];

    }

    public void CreateHitFX(Transform _target,bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if (_critical)
        {
            hitPrefab = criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45,45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition,yPosition),Quaternion.identity,_target);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx,.5f);
    }

}
