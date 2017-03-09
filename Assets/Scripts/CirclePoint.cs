using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclePoint : MonoBehaviour
{
    public Sprite[] DefSprites;
    public Sprite[] ActiveSprites;

    public bool Type { get; set; }
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Initialize(bool type)
    {
        //Debug.Log("Initialize: " + gameObject.name);
        Type = type;
        var renderer = GetComponent<SpriteRenderer>();
        
        if (type)
        {
            renderer.sprite = ActiveSprites[Random.Range(0, ActiveSprites.Length - 1)];
        }
        else
        {
            renderer.sprite = DefSprites[Random.Range(0, DefSprites.Length - 1)];
        }
    }

}
