using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour
{

    public Texture2D[] frames;
    public int fps = 10;

    void Update()
    {
        int index = (int)(Time.time * fps) % frames.Length;
        GetComponentInChildren<RawImage>().texture = frames[index];
    }
}
