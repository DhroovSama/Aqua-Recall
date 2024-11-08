using UnityEngine;
using UnityEngine.Video;

public class VideoScaler : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;

    void Start()
    {
     
        videoPlayer.targetTexture = renderTexture;
        videoPlayer.Play();
    }

    void Update()
    {
      
        int newWidth = (int)(transform.localScale.x * renderTexture.width);
        int newHeight = (int)(transform.localScale.y * renderTexture.height);

   
        //renderTexture.Reinitialize(newWidth, newHeight);

     
        GetComponent<Renderer>().material.mainTexture = renderTexture;
    }
}
