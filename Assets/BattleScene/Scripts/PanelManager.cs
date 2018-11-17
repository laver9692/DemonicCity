﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene
{
    /// <summary>
    /// Panel manager.
    /// </summary>
    public class PanelManager : MonoBehaviour
    {
        /// <summary>タップ,フリック,Raycast管理クラス</summary>
        TouchGestureDetector m_touchGestureDetector;

        /// <summary>同オブジェクトにアタッチされている[パネルを生成して同時にパネル種類の振り分けもしてくれるクラス]の参照</summary>
        InstantiatePanels m_instantiatePanels;
        /// <summary>パネルが処理中かどうか表すフラグ</summary>
        bool m_isPanelProcessing;


        void Awake()
        {
            // 同オブジェクトにアタッチされているコンポーネントの取得
            m_touchGestureDetector = GetComponent<TouchGestureDetector>();
            m_instantiatePanels = GetComponent<InstantiatePanels>();
        }

        void Start()
        {
            m_instantiatePanels.GeneratePanels(); // パネル生成処理
            // タッチによる任意の処理をイベントに登録する
            m_touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (m_isPanelProcessing)
                {
                    return;
                }
                switch (gesture)
                {
                    case TouchGestureDetector.Gesture.Click: // クリックジェスチャをした時
                        if (touchInfo.m_hitResult.tag == "Panel") // タッチしたオブジェクトのタグがパネルなら
                        {
                            var panel = touchInfo.m_hitResult.GetComponent<Panel>(); // タッチされたパネルのPanelクラスの参照
                            panel.Open(); // panelを開く

                        }
                        break;
                }

            });
        }
    }
}
