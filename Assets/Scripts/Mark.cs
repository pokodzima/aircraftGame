using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Mark : MonoBehaviour
{

    public float paddingPercent;
    
    public Transform relatedEnemy;

    private Transform _player;

    private Camera _camera;

    private GameObject _distance;

    private GameObject _icon;

    private Text _distanceText;

    private const string PlayerTag = "Player";
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag(PlayerTag) != null)
            _player = GameObject.FindGameObjectWithTag(PlayerTag).transform;
        else Debug.LogErrorFormat("There is no GameObject with {0} tag!", PlayerTag);

        _player = FindObjectOfType<PlaneController>().transform;
        
        if (Camera.main != null)
            _camera = Camera.main;
        else Debug.LogError("There is no Camera with MainCamera tag!");

        _distance = transform.GetChild(0).gameObject;
        _icon = transform.GetChild(1).gameObject;

        _distanceText = _distance.GetComponent<Text>();
    }

    private float Padding()
    {
        return paddingPercent / 100;
    }
    
    void Update()
    {
        if (!relatedEnemy) Destroy(gameObject);
        
        var newPosition = _camera.WorldToViewportPoint(relatedEnemy.position);

        if (newPosition.x > 0f + Padding() && newPosition.x < 1f - Padding() && newPosition.y > 0f + Padding() && newPosition.y < 1f - Padding() && newPosition.z >= 0f)
        {
            _distance.SetActive(true);
            _icon.SetActive(false);
            _distanceText.text = ((int)Vector3.Distance(relatedEnemy.position, _player.position)).ToString();
        }
        else
        {
            newPosition = ToScreenBorder(newPosition);

            _distance.SetActive(false);
            _icon.SetActive(true);
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
