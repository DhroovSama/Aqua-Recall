using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    [SerializeField] float speed = 80.0f;

    private void Update()
    {
        this.transform.Rotate(new Vector3(speed * Time.deltaTime, 0, 0));
    }
}
