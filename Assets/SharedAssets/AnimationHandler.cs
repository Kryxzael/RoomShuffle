using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Obsolete]
[RequireComponent(typeof(SpriteRenderer))]
public class AnimationHandler : MonoBehaviour
{

    public Sprite[] Frames = new Sprite[0];
    public int Frame;
    public int Speed;
    int Await;

    SpriteRenderer _render;

	void Start()
    {
        _render = GetComponent<SpriteRenderer>();
	}
	
	void Update()
    {
        if (Frames.Length == 0)
        {
            Debug.LogWarning("Animation does not have any sprites");

            return;
        }

        if (Await++ >= Speed)
        {
            Await = 0;
            _render.sprite = Frames[Frame];

            Frame = (Frame + 1) % Frames.Length;
        }
	}

    public void Restart()
    {
       
        Frame = 0;
        Await = 0;

        if (Frames.Length == 0)
        {
            Debug.LogWarning("Animation does not have any sprites");

            return;
        }

        _render.sprite = Frames[0];
    }

    public void SetFrames(IEnumerable<Sprite> sprites)
    {
        if (sprites.SequenceEqual(Frames))
        {
            return;
        }

        Frames = sprites.ToArray();
        Restart();
    }
}
