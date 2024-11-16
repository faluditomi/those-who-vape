using System.Collections;
using UnityEngine;

public class MixedBerryController : VapeController
{
    private ParticleSystem vapeSmoke;
    private ParticleSystem.EmissionModule emission;
    private bool isShooting = false;

    protected void Awake()
    {
        vapeSmoke = gameObject.GetComponentInChildren<ParticleSystem>();
        emission = vapeSmoke.emission;
    }

    private void Start()
    {
        vapeSmoke.Play();
        emission.rateOverTime = 0;
    }

    protected override void Scan()
    {
        base.Scan();
        
        if(isShooting)
        {
            isShooting = false;
            emission.rateOverTime = 0;
        }
    }

    protected override IEnumerator ShootBehaviour()
    {
        isShooting = true;
        emission.rateOverTime = 100;
        yield return new WaitForSecondsRealtime(shootDurations);
        isShooting = false;
        emission.rateOverTime = 0;
        yield return new WaitForSecondsRealtime(shootFrequency);
        shootCoroutine = StartCoroutine(ShootBehaviour());
    }
}
