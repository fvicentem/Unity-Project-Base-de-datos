using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoScroller : MonoBehaviour
{

    public float scrollSpeed;
    [SerializeField] private MeshRenderer mesh;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = new Vector2(Time.time * scrollSpeed, 0);
        mesh.material.mainTextureOffset = offset;
    }
}

