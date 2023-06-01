using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VideoFondoNivel1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Asignamos el VideoPlayer a la c�mara principal, para ello hay que buscarle
        GameObject fondo = GameObject.Find("VideoFondo");

        //A�adimos un VideoPlayer a la MainCamera
        var videoPlayer = fondo.AddComponent<UnityEngine.Video.VideoPlayer>();

        videoPlayer.playOnAwake = true;
        //Por defecto, el videoplayer se va a posicionar en el NearPlane de los Cipping Planes de la MainCamera
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;

        //videoPlayer.targetCameraAlpha = 0.5f;

        videoPlayer.url = Application.dataPath + "/StreamingAssets/Videojuegos y Animaci�n en ESI 2019-2020_1.mp4";

        videoPlayer.Play();
    }
}
