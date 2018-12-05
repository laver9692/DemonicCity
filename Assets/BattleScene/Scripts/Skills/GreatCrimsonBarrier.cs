﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{

    /// <summary>
    /// Great crimson barrier.
    /// ４４レベル：紅蓮障壁　
    /// 街破壊数１８以上で発動　次の敵の攻撃を１０％軽減
    /// </summary>
    public class GreatCrimsonBarrier : PassiveSkill
    {
        /// <summary>任意の増加割合(%)</summary>
        [SerializeField] float m_increase = 0.01f;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.GreatCrimsonBarrier; // フラグを設定
        }

        /// <summary>
        /// 魔拳
        /// 街破壊数1以上で発動.
        /// 街破壊数 * 攻撃力の1% を加算して攻撃
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 魔拳");
            var count = m_panelCounter.GetCityDestructionCount(); // 街破壊数

        }
    }
}

