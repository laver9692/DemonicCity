﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DemonicCity.BattleScene.Skill
{
    /// <summary>
    /// SelfRegeneration
    ///２３レベル：自己再生
    /// 街破壊数１２以上で発動　街破壊数×最大 HPの１％回復
    /// </summary>
    public class SelfRegeneration : PassiveSkill
    {
        /// <summary>MagiaのHPDrawの参照</summary>
        [SerializeField] HitPointGauge m_magiaHPGauge;

        protected override void Awake()
        {
            base.Awake();
            m_passiveSkill = Magia.PassiveSkill.SelfRegeneration; // フラグを設定
            m_timing = SkillManager.Timing.Enhancement; // フラグを設定
        }

        /// <summary>
        /// スキル発動
        /// </summary>
        protected override void SkillActivate()
        {
            Debug.Log("Activated the 自己再生");
            m_hitPointBuffer = m_panelCounter.DestructionCount * m_battleManager.m_magiaStats.Temp.m_hitPoint * m_incease; // 丸め込み対策の為に一度変数に保存
            m_battleManager.m_magiaStats.m_hitPoint += (int)m_hitPointBuffer; // ここでintに変換

            if (m_battleManager.m_magiaStats.m_hitPoint > m_battleManager.m_magiaStats.MaxHP) // もしMaxHPを越したら
            {
                m_battleManager.m_magiaStats.m_hitPoint = m_battleManager.m_magiaStats.MaxHP; // hpをmaxに戻す
            }
            m_magiaHPGauge.Sync(m_battleManager.m_magiaStats.m_hitPoint); // HPGaugeと同期
        }

        protected override void SkillDeactivate()
        {
        }
    }
}
