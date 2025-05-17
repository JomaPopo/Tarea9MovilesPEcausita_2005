using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private float movementRange = 3f; // Rango de movimiento (puedes cambiarlo en el Inspector)
    [SerializeField] private float movementSpeed = 1f; // Velocidad de movimiento (puedes cambiarlo en el Inspector)
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (movementRange > 0 && movementSpeed > 0)
        {
            transform.position = new Vector3(
                startPosition.x + Mathf.PingPong(Time.time * movementSpeed, movementRange * 2) - movementRange,
                transform.position.y,
                transform.position.z
            );
        }
    }


}