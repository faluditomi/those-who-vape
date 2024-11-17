using UnityEngine;

public class HeisenbergController : VapeController
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

    protected override void Shoot()
    {
        base.Shoot();

        if(!isShooting)
        {
            isShooting = true;
            emission.rateOverTime = 100;
        }
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
}
