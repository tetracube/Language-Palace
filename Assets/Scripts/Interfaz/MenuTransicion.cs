using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTransicion : MonoBehaviour
{

    private void OnEnable()
    {
        FadeIn();
    }

    public void FadeIn() { StartCoroutine(FadeImage(false, gameObject, ()=>{ })); }
    public void FadeOut(Action action) { StartCoroutine(FadeImage(true, gameObject, action)); }
    public void FadeOut() { StartCoroutine(FadeImage(true, gameObject, () => { gameObject.SetActive(false); })); }
    private IEnumerator FadeImage(bool fadeAway, GameObject gameObject, Action action)
    {
        // fade from opaque to transparent
        if (fadeAway)
        {
            // loop over 1 second backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime * 10)
            {
                // set color with i as alpha
                gameObject.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
            }
            gameObject.GetComponent<CanvasGroup>().alpha = 0;
            yield return null;
            action();
        }
        // fade from transparent to opaque
        else
        {
            // loop over 1 second
            for (float i = 0; i <= 1; i += Time.deltaTime * 10)
            {
                // set color with i as alpha
                gameObject.GetComponent<CanvasGroup>().alpha = i;
                yield return null;
                
            }
            gameObject.GetComponent<CanvasGroup>().alpha = 1;
            yield return null;
        }
    }

}
