using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public Transform[] objectsToRotate;

    public bool addOffset = false;

    public float maxOffset = 0.5f;

    public int x = 0;
    public int y = 0;
    public int z = 0;

    public void RandomRotate()
    {
        float[] angles = { -90f, 0f, 90f, 180f };

        foreach (Transform obj in objectsToRotate)
        {
            foreach (Transform child in obj)
            {
                float randomAngle = angles[Random.Range(0, angles.Length)];
                Quaternion currentRotation = child.rotation;
                Quaternion newRotation = Quaternion.Euler(randomAngle * x, randomAngle * y, randomAngle * z);
                child.rotation = currentRotation * newRotation;
                if (addOffset)
                {
                    child.position += new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
                }
            }
        }
    }

}