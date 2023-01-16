using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public Transform[] objectsToRotate;

    public void RandomRotate()
    {
        float[] angles = { -90f, 0f, 90f, 180f };

        foreach (Transform obj in objectsToRotate)
        {
            foreach (Transform child in obj)
            {
                float randomAngle = angles[Random.Range(0, angles.Length)];
                child.rotation = Quaternion.Euler(0, randomAngle, 0);
            }
        }
    }

}