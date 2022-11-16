using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSong : MonoBehaviour
{
    public string song;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play(song);
    }

}
