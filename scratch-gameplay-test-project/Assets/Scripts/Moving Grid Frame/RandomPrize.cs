using System.Collections.Generic;
using UnityEngine;

public class RandomPrize : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public List<Sprite> spritePool;

    void Start()
    {
        RollSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RollSprite();
        }
    }

    public void RollSprite()
    {
        int spriteIndex = Random.Range(0, spritePool.Count);
        spriteRenderer.sprite = spritePool[spriteIndex];
    }
}
