using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    private Rigidbody rigidBody;
    private readonly float moveSpeed = 2.5f;
    private readonly float runSpeed = 3.5f;
    private readonly float crouchSpeed = 1.5f;

    private bool parado;
    

    private Animator animador;

    private float x;
    private float y;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animador = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!parado)
        {
            animador.SetBool("isWalking", Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
            animador.SetBool("isRunning", Input.GetKey("left shift"));
            animador.SetBool("isCrouching", Input.GetKey("left ctrl"));
            float velocidad = Input.GetKey("left shift") ? runSpeed : moveSpeed;
            velocidad = Input.GetKey("left ctrl") ? crouchSpeed : velocidad;

            x = Input.GetAxis("Horizontal") * velocidad;
            y = Input.GetAxis("Vertical") * velocidad;
        }
        else {
            animador.SetBool("isWalking", false);
            animador.SetBool("isRunning", false);
        }
    }
    void FixedUpdate()
    {
        if (!parado)
        {
            Vector3 posicion = transform.right * x + transform.forward * y;
            Vector3 movimientoPosicion = new Vector3(posicion.x, rigidBody.velocity.y, posicion.z);

            rigidBody.velocity = movimientoPosicion;
        }
        else { rigidBody.velocity = Vector2.zero; }

    }
    public void Parar(bool parar) {
        this.parado = parar;
    }

}
