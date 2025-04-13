using UnityEngine;

public class AfterImageFx : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colourLooseRate;

    public void SetupAfterImage(float _loosingSpeed, Sprite _spriteImage)
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = _spriteImage;
        colourLooseRate = _loosingSpeed;
    }

    private void Update()
    {

        float alpha = sr.color.a - (colourLooseRate * Time.deltaTime);  
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if(sr.color.a <= 0)
            Destroy(gameObject);


        
    }
}
