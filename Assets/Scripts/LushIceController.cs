using System.Collections;
using UnityEngine;

public class LushIceController : VapeController
{
    public GameObject vapeBombPrefab;
    public GameObject vapeBombSmokePrefab;
    public Transform firePoint;
    public float flightTime = 2f;
    public float arcHeight = 5f;

    private IEnumerator ArcMovement(GameObject projectile, Vector3 targetPosition)
    {
        Vector3 startPosition = projectile.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < flightTime)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / flightTime;

            // Interpolate position linearly for x and z
            Vector3 currentPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // Add the arc effect for y
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight; // Sin curve for arc
            currentPosition.y += height;

            // Move the projectile
            projectile.transform.position = currentPosition;

            yield return null;
        }

        GameObject smoke = Instantiate(vapeBombSmokePrefab, projectile.transform.position, Quaternion.identity);
        Destroy(projectile);
        yield return new WaitForSecondsRealtime(5);
        Destroy(smoke);
    }

    protected override IEnumerator ShootBehaviour()
    {
        audioManager.Play("mixed_berry_fire", audioSource);
        GameObject projectile = Instantiate(vapeBombPrefab, firePoint.position, Quaternion.identity);
        StartCoroutine(ArcMovement(projectile, currentTarget.transform.position));
        yield return new WaitForSecondsRealtime(shootFrequency);
        shootCoroutine = StartCoroutine(ShootBehaviour());
    }
}
