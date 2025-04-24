using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapHandler : MonoBehaviour
{
    public GameObject curInActiveObj;
    public GameObject nextActiveObj;

    private SpriteRenderer spriteRenderer;

    public Sprite normal_sprite;

    public Sprite highlight_sprite;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void OnMouseUp()
    {
        if (spriteRenderer != null && normal_sprite != null)
        {
            spriteRenderer.sprite = normal_sprite;
        }

        doAction();

    }

    void OnMouseEnter()
    {
        if (spriteRenderer != null && highlight_sprite != null)
        {
            spriteRenderer.sprite = highlight_sprite;
        }
    }

    void OnMouseExit()
    {
        if (spriteRenderer != null && normal_sprite != null)
        {
            spriteRenderer.sprite = normal_sprite;
        }
    }

    void doAction(){
        curInActiveObj.SetActive(false);
        nextActiveObj.SetActive(true);
    }
}