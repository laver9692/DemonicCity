﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Text>().text = DateTime.Now.ToString("yyyyMMddHHmmss");
        
	}

    void Atodekangaeru()
    {
        
    }


}
