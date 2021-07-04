using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveMove : MonoBehaviour
{
    public float moveSpeed = 1;
    private RawImage image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        WaveMoveOn();
    }

    void WaveMoveOn()
    {
        float addValue = Time.deltaTime * 0.001f * moveSpeed;

        Rect rect = image.uvRect;
        rect.x -= addValue;
        image.uvRect = rect;
    }
}
