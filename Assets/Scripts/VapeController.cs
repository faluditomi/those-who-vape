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
    private EnemyController currentTarget;
    private Quaternion targetRotation;
    private Coroutine scanCoroutine;
    private int scanSpeed = 30;
    public float range = 10f;
    public float turnSpeed = 100f;
    public Transform vapeTransform;
    public VapeType vapeType;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > range)
        {
            currentTarget = GetClosestEnemy();
        }

        if(currentTarget != null)
        {
            if(scanCoroutine != null)
            {
                StopCoroutine(scanCoroutine);
                scanCoroutine = null;
            }

            RotateTowards(currentTarget.transform.position);
        }
        else if(scanCoroutine == null)
        {
            scanCoroutine = StartCoroutine(ScanBehaviour());
        }
    }

    private EnemyController GetClosestEnemy()
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

    private void RotateTowards(Vector3 targetPosition)
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

    private IEnumerator ScanBehaviour()
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
