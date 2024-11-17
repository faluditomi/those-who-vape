using System.Collections;
using System.Linq;
using UnityEngine;

public class VapeController : MonoBehaviour
{
    public enum VapeType
    {
        MixedBerry,
        LushIce,
        Heisenberg
    }

    private GameObject[] enemies;
    protected EnemyController currentTarget;
    private Coroutine scanCoroutine;
    protected Coroutine shootCoroutine;
    private int scanSpeed = 30;
    public float range = 10f;
    public float shootFrequency = 2f;
    public float shootDurations = 2f;
    public float turnSpeed = 100f;
    public Transform vapeTransform;
    public VapeType vapeType;
    protected AudioManager audioManager;
    protected AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    protected void FixedUpdate()
    {
        if(currentTarget == null || !currentTarget.gameObject.activeInHierarchy || Vector3.Distance(transform.position, currentTarget.transform.position) > range)
        {
            currentTarget = GetClosestEnemy();
        }

        if(currentTarget != null && currentTarget.gameObject.activeInHierarchy)
        {
            Shoot();
        }
        else
        {
            Scan();
        }
    }

    protected virtual void Shoot()
    {
        if(scanCoroutine != null)
        {
            StopCoroutine(scanCoroutine);
            scanCoroutine = null;
        }

        if(shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(ShootBehaviour());
        }

        RotateTowards(currentTarget.transform.position);
    }

    protected virtual void Scan()
    {
        if(shootCoroutine != null)
        {
            StopCoroutine(shootCoroutine);
            shootCoroutine = null;
        }

        if(scanCoroutine == null)
        {
            scanCoroutine = StartCoroutine(ScanBehaviour());
        }
    }

    protected EnemyController GetClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemy = enemies
            .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= range +- 1)
            .OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position))
            .FirstOrDefault();
        
        if(enemy != null)
        {
            return enemy.GetComponent<EnemyController>();
        }
        else
        {
            return null;
        }
    }

    protected void RotateTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - vapeTransform.position).normalized;
        directionToTarget.y = 0;
        float angleToTarget = Vector3.Angle(vapeTransform.forward, directionToTarget);

        if(angleToTarget < 5f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            vapeTransform.rotation = Quaternion.Slerp(vapeTransform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Vector3 crossProduct = Vector3.Cross(vapeTransform.forward, directionToTarget);
            float turnDirection = Mathf.Sign(crossProduct.y);
            float rotationAmount = turnSpeed * Time.fixedDeltaTime * turnDirection;
            vapeTransform.Rotate(0, rotationAmount, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public VapeType GetVapeType()
    {
        return vapeType;
    }

    protected virtual IEnumerator ShootBehaviour()
    {
        yield return null;
    }

    protected IEnumerator ScanBehaviour()
    {
        float elapsedTime = 0f;
        float duration = Random.Range(0.8f, 3.2f);
        scanSpeed *= -1;

        while(elapsedTime < duration)
        {
            float rotationAmount = scanSpeed * Time.deltaTime;
            vapeTransform.Rotate(0f, rotationAmount, 0f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scanCoroutine = StartCoroutine(ScanBehaviour());
    }
}
