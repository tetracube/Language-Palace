using UnityEngine;

public class Transicion : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseInSine();
    }
    public void OnClose()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInSine().setOnComplete(() => gameObject.SetActive(false));
    }
}
