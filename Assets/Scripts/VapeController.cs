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
    private float scanAngle;
    private float scanDirection = 1f;
    private float currentScanAngle = 0f;
    private Quaternion targetRotation;
    public float range = 10f;
    public float turnSpeed = 100f;
    public Transform vapeTransform;
    public VapeType vapeType;

    private void Start()
    {
        GetNewScanAngle();
    }

    private void FixedUpdate()
    {
        if(currentTarget == null || Vector3.Distance(transform.position, currentTarget.transform.position) > range)
        {
            currentTarget = GetClosestEnemy();
        }

        if(currentTarget != null)
        {
            RotateTowards(currentTarget.transform.position);
        }
        else
        {
            PerformSmoothScanningMotion();
        }

        vapeTransform.rotation = Quaternion.Slerp(vapeTransform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
    }

    private EnemyController GetClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject enemy = enemies
            .Where(enemy => Vector3.Distance(transform.position, enemy.transform.position) <= range)
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
        Vector3 target = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        Vector3 direction = (target - vapeTransform.position).normalized;
        targetRotation = Quaternion.LookRotation(direction);
    }

    private void PerformSmoothScanningMotion()
    {
        currentScanAngle += scanDirection * turnSpeed * Time.fixedDeltaTime;

        if (Mathf.Abs(currentScanAngle) > scanAngle)
        {
            scanDirection *= -1f;
            GetNewScanAngle();
        }

        Vector3 scanDirectionVector = Quaternion.Euler(0, currentScanAngle, 0) * Vector3.forward;
        targetRotation = Quaternion.LookRotation(scanDirectionVector);
    }

    private void GetNewScanAngle()
    {
        scanAngle = currentScanAngle += Random.Range(15f, 90f);
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
}
