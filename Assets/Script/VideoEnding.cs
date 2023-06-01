using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VideoEnding : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Asignamos el VideoPlayer a la cámara principal, para ello hay que buscarle
        GameObject camera = GameObject.Find("Main Camera");

        //Añadimos un VideoPlayer a la MainCamera
        var videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();

        videoPlayer.playOnAwake = true;
        //Por defecto, el videoplayer se va a posicionar en el NearPlane de los Cipping Planes de la MainCamera
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        //videoPlayer.targetCameraAlpha = 0.5f;

        videoPlayer.url = Application.dataPath + "/StreamingAssets/Créditos.mp4";

        videoPlayer.loopPointReached += LoadScene;

        videoPlayer.Play();
    }

    void LoadScene(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("MainMenú");
    }
}
