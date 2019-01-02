﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DemonicCity
{
    /// <summary>
    /// Game manager.
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        [SerializeField] private float m_fadeTime = 1f;

        SceneFader fader;
        private void Start()
        {
            fader = SceneFader.Instance;
            SceneManager.sceneLoaded += ((scene, mode) =>
            {
                fader.FadeIn(m_fadeTime);
            });
        }


        /// <summary>
        /// 作ったゲームオブジェクトをシーン遷移しても壊されない様にする
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();
            DontDestroyOnLoad(Instance);
        }
    }
}
