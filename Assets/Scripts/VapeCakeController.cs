using UnityEngine;

public class VapeCakeController : MonoBehaviour
{
    private void FixedUpdate()
    {
        float rotationAmount = 15 * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f);
    }
}
