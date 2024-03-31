using System.Collections;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] Vector2 scrollVelocity;
    Material _material;

    void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }
    
    IEnumerator Start()
    {
        while (GameManager.GameState != GameState.GameOver)
        {
            _material.mainTextureOffset += scrollVelocity * Time.fixedDeltaTime;
            yield return null;
        }
    }
}