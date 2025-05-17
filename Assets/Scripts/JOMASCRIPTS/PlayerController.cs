using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float moveSpeed = 15f; // Velocidad de movimiento horizontal
    public float tiltSensitivity = 2f; // Sensibilidad del acelerómetro

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private float highestPoint = 0f;
    private float initialTilt; // Para calibrar el dispositivo

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Calibrar el acelerómetro al inicio
        initialTilt = Input.acceleration.x;
        // Inicializar el score a 0
        GameManager.Instance.UpdateScore(0);
    }

    void Update()
    {
        float height = transform.position.y;

        if (height > highestPoint)
        {
            highestPoint = height;
        }

        // Actualizar el score siempre, basado en la altura más alta
        GameManager.Instance.UpdateScore(Mathf.FloorToInt(highestPoint));

        // Saltar automáticamente cuando está en el suelo
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }


    void FixedUpdate()
    {
        // Movimiento basado en el acelerómetro
        float tilt = Input.acceleration.x - initialTilt;
        float moveDirection = Mathf.Clamp(tilt * tiltSensitivity, -1f, 1f);

        // Aplicar movimiento horizontal
        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        // Limitar los bordes de la pantalla
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f); // Dejar un pequeño margen
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Si cae por debajo de la pantalla
        if (collider.CompareTag("DeathZone"))
        {
            GameManager.Instance.GameOver(Mathf.FloorToInt(highestPoint));
        }
    }
}