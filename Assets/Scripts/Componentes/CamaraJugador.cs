using UnityEngine;

public class CamaraJugador : MonoBehaviour
{
    public float sensibilidadRaton = 5f;
    private float x = 0f;
    private float y = 0f;

    private bool frenado;


    [SerializeField]
    private Transform target;

    void Start()
    {
        JugadorManager.GetInstance().MostrarPuntero(false);
    }

    void Update()
    {
        if (!frenado)
        {
            x += -Input.GetAxis("Mouse Y") * sensibilidadRaton;
            x = Mathf.Clamp(x, -90f, 43);
            y += Input.GetAxis("Mouse X") * sensibilidadRaton;
        }
     

    }

    private void FixedUpdate()
    {
        if (!frenado)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(x, 0, 0);
            transform.localRotation = Quaternion.Euler(0, y, 0);
                        
        }
        Camera.main.transform.localPosition = new Vector3(0, target.localPosition.y * 1.2f + 0.6f, -target.localPosition.z * 1.5f);
    }

    public void Frenar(bool frenar)
    {
        this.frenado = frenar;
    }


}
