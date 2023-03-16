using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    [SerializeField] private bool sortSpriteAllTheTime;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = UtilsClass.Utils.GetSortingOrderWithZ(transform.position, 1);
    }
    private void Update()
    {
        if(sortSpriteAllTheTime)
        {
            spriteRenderer.sortingOrder = UtilsClass.Utils.GetSortingOrderWithZ(transform.position, 1);
        }
    }
}
