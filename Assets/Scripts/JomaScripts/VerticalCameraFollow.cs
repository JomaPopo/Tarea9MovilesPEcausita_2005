using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform target;       // Objetivo a seguir (jugador)
    public Vector3 offset = new Vector3(0, 2, -10); // Offset de la cámara
    private float highestY;        // Almacena la posición Y más alta alcanzada
    private float initialX;        // Almacena la posición X inicial

    private void Start()
    {
        if (target != null)
        {
            // Guarda la posición X inicial de la cámara
            initialX = transform.position.x;
            // Inicializa con la posición Y inicial del jugador + offset
            highestY = target.position.y + offset.y;
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Si el jugador sube por encima del valor guardado, actualiza highestY
            if (target.position.y + offset.y > highestY)
            {
                highestY = target.position.y + offset.y;
            }

            // Mueve la cámara manteniendo la X inicial, usando highestY en Y, y el offset en Z
            transform.position = new Vector3(
                initialX,                   // Mantiene la posición X fija
                highestY,                   // Solo sigue el eje Y (más alto)
                target.position.z + offset.z // Mantiene el offset en Z
            );
        }
    }
}