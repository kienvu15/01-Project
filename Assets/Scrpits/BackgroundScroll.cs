using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{

    [SerializeField] private RawImage _img;
    [SerializeField] private float x, y;
    void Start()
    {
        //hjghghjgjhghjgjh
    }

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(x, y) * Time.deltaTime, _img.uvRect.size);
    }

    
    
}