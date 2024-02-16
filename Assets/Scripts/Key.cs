using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (transform.tag)
        {
            case "RedKey":
                bool hasRedKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasRedKey", 0));
                if (hasRedKey)
                {
                    gameObject.SetActive(false);
                }
                break;
            case "GreenKey":
                bool hasGreenKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasGreenKey", 0));
                if (hasGreenKey)
                {
                    gameObject.SetActive(false);
                }
                break;
            case "BlueKey":
                bool hasBlueKey = Convert.ToBoolean(PlayerPrefs.GetInt("hasBlueKey", 0));
                if (hasBlueKey)
                {
                    gameObject.SetActive(false);
                }
                break;

        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Time.deltaTime * 100, 0, Space.World);
    }
}
