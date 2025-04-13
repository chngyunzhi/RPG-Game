using Unity.Cinemachine;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("Screen Shake")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;

    [Header("After Image Fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colourLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCoolDownTimer;

    [Space]
    [SerializeField] private ParticleSystem dustFx;



    protected override void Start()
    {
        base.Start();

        screenShake = GetComponent<CinemachineImpulseSource>();

    }
    private void Update()
    {
        afterImageCoolDownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }


    public void CreateAfterImage()
    {
        if (afterImageCoolDownTimer < 0)
        {
            afterImageCoolDownTimer = afterImageCooldown;

            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colourLooseRate, sr.sprite);
        }
    }

    public void PlayDustFx()
    {
        if (dustFx != null)
            dustFx.Play();
    }
}
