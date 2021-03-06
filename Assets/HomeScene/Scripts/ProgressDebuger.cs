﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DemonicCity
{
    public class ProgressDebuger : MonoBehaviour
    {
        [SerializeField] GameObject panel;
        GameObject panelInstance;
        // Use this for initialization
        void Start()
        {
            Debug.Log("[P]キーで進捗操作");
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PanelDebugerOpen();
            }
        }


        public void PanelDebugerOpen()
        {
            if (panelInstance == null)
            {
                panelInstance = Instantiate(panel);
            }
        }
    }
}