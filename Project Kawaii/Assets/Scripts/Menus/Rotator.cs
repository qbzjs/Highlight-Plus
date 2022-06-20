using UnityEngine;

public class Rotator : MonoBehaviour
{
    public int rotateSpeed = 30;

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public void AdjustActivation()
    {
        enabled = !enabled;
    }
}