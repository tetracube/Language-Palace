using UnityEngine;

public class TransicionPanel : MonoBehaviour
{
      private void OnEnable()
      {
          transform.localScale = Vector3.zero;
          LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseInOutBack();
      }
      public void OnClose()
      {
          LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() => transform.parent.gameObject.SetActive(false));
      }
   
}
