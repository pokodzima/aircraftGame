using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Zenject;

public class Mark : MonoBehaviour
{

    public float paddingPercent;
    
    public Transform relatedEnemyTransform;

    public Enemy.Enemy relatedEnemy;

    private Transform _player;

    private Camera _camera;

    public GameObject distance;

    public GameObject icon;

    private Text _distanceText;

    private const string PlayerTag = "Player";
    // Start is called before the first frame update
    
    [Inject]
    private void Construct(PlaneController player, Camera camera)
    {
        _player = player.transform;
        _camera = camera;
    }
    
    void Start()
    {
        _distanceText = distance.GetComponent<Text>();
    }

    private float Padding()
    {
        return paddingPercent / 100;
    }
    
    void Update()
    {
        if (relatedEnemy.isDestroying) Destroy(gameObject);

        var newPosition = _camera.WorldToViewportPoint(relatedEnemyTransform.position);

        if (newPosition.x > 0f + Padding() && newPosition.x < 1f - Padding() && newPosition.y > 0f + Padding() && newPosition.y < 1f - Padding() && newPosition.z >= 0f)
        {
            distance.SetActive(true);
            icon.SetActive(false);
            _distanceText.text = ((int)Vector3.Distance(relatedEnemyTransform.position, _player.position)).ToString();
        }
        else
        {
            newPosition = ToScreenBorder(newPosition);

            distance.SetActive(false);
            icon.SetActive(true);
        }

        newPosition.z = _camera.nearClipPlane;
        newPosition.x *= _camera.pixelWidth;
        newPosition.y *= _camera.pixelHeight;
        
        transform.position = newPosition;
    }
    


    private Vector3 ToScreenBorder(Vector3 pos)
    {
        //Debug.Log("Pos: " + pos.x + " " + pos.y);
        pos.x -= 0.5f;
        pos.y -= 0.5f;

        var slope = pos.y / pos.x;
        //Debug.Log("Slope: " + slope);

        if (pos.x >= 0 && pos.y >= 0)
        {
            if (pos.x >= pos.y)
            {
                
                pos.x = 0.5f - Padding();
                pos.y = pos.x * slope;
            }
            else
            {
                
                pos.y = 0.5f - Padding();
                pos.x = pos.y / slope;
            }
        }
        else if (pos.x >= 0 && pos.y < 0)
        {
            if (pos.x > -pos.y)
            {
                
                pos.x = 0.5f - Padding();
                pos.y = pos.x * slope;
                
                
            }
            else
            {
                
                pos.y = -0.5f + Padding();
                pos.x = pos.y / slope;
            }
        }
        else if (pos.x < 0 && pos.y >= 0)
        {
            if (-pos.x > pos.y)
            {
                
                pos.x = -0.5f + Padding();
                pos.y = pos.x * slope ;
                
            }
            else
            {
                
                pos.y = 0.5f - Padding();
                pos.x = pos.y / slope;
            }
        }
        else
        {
            if (-pos.x > pos.y)
            {
                pos.y = -0.5f + Padding();
                pos.x = pos.y / slope;
            }
            else
            {
                pos.x = -0.5f + Padding();
                pos.y = pos.x * slope;
                
            }
        }
        //Debug.Log(pos.x + " " + pos.y);

        if (pos.z < 0f)
        {
            pos.x = -pos.x;
            pos.y = -pos.y;
        }

        pos.x += 0.5f;
        pos.y += 0.5f;
        

        return pos;
    }
}
