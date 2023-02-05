using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OnEndVideoSceneLoader : MonoBehaviour
{
    public VideoPlayer player;
    public int SceneIndex;

    // Start is called before the first frame update
    void Start()
    {
        player.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
