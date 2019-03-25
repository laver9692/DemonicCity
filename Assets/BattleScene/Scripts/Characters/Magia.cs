﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using UnityEngine.SceneManagement;
using System.Linq;

namespace DemonicCity
{
    /// <summary>
    /// Magia.
    /// </summary>
    [Serializable]
    public class Magia : SavableSingletonBase<Magia>
    {
        #region Property
        /// <summary>パッシブスキルフラグのプロパティ</summary>
        public PassiveSkill MyPassiveSkill
        {
            get { return m_passiveSkill; }
        }
        /// <summary>初期レベルを1としたときの最大レベルを返す</summary>
        public int MaxLevel
        {
            get { return m_requiredExps.Length + 1; }
        }
        /// <summary>ステータスクラスのプロパティ</summary>
        public Status Stats
        {
            get
            {
                if(m_stats == null)
                {
                    var saveData = SaveData.Instance;
                    m_stats = saveData.magia.m_stats;
                }
                return m_stats;
            }
            set { m_stats = value; }
        }
        /// <summary>振り分けポイントのプロパティ</summary>
        public int AllocationPoint
        {
            get { return m_allocationPoint; }
            set { m_allocationPoint = value; }
        }
        /// <summary>属性フラグ</summary>
        public Attribute MyAttribute
        {
            get { return m_attribute; }
            set { m_attribute = value; }
        }

        /// <summary>総経験値</summary>
        public int TotalExperience
        {
            get { return m_totalExperience; }
            set { m_totalExperience = value; }
        }
        /// <summary>マギアのHP最大値</summary>
        public int MaxHP { get; private set; }

        /// <summary>属性別ユニークスキルの条件値を返す</summary>
        public int UniqueSkillConditionByAttribute
        {
            get
            {
                int condition;
                switch (MyAttribute)
                {
                    case Attribute.Standard:
                        condition = 30;
                        break;
                    case Attribute.SwordPrincess:
                        condition = 30;
                        break;
                    case Attribute.BladeEmperor:
                        condition = 30;
                        break;
                    case Attribute.Empress:
                        condition = 30;
                        break;
                    case Attribute.BlackKing:
                        condition = 30;
                        break;
                    case Attribute.DevilsGod:
                        condition = 30;
                        break;
                    default:
                        condition = 30;
                        Debug.LogError("属性が設定されていません。");
                        break;
                }
                return condition;
            }
        }
        #endregion

        #region Field
        /// <summary>経験値</summary>
        [SerializeField] int m_totalExperience;
        /// <summary>振り分けポイント</summary>
        [SerializeField] int m_allocationPoint;
        /// <summary>MyAttributeのバッキングフィールド</summary>
        [SerializeField] Attribute m_attribute = Attribute.Standard;
        /// <summary>レベルアップに必要な経験値(破壊したパネルの総数)</summary>
        [SerializeField] int[] m_requiredExps = { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 150, 200, 250, 300, 400, 500 };
        /// <summary>パッシブスキルフラグ</summary>     
        [SerializeField] PassiveSkill m_passiveSkill = PassiveSkill.Invalid;


        /// <summary>実際にセーブするステータスクラス</summary>
        [SerializeField] // ==============nullの時はロードする様プロパティに設定する予定======================
        Status m_stats = new Status()
        {
            Level = 1,
            HitPoint = 1000,
            Attack = 100,
            Defense = 100,
            Charm = 0,
            Sense = 0,
            Dignity = 0,
            Knowledge = 0,
            Durability = 0,
            MuscularStrength = 0,
        };

        /// <summary>1levelUP毎の固有ステータス用振り分けポイント追加量</summary>
        int m_addStatsPoint = 3;
        /// <summary>固有ステータスを基礎ステータスに変換する際の倍率</summary>
        int m_magnificationByStats = 5;
        /// <summary>固有ステータスを形態毎に基礎ステータスに変換する際の倍率</summary>
        int m_magnificationByAttribute = 50;

        #endregion

        #region Method

        /// <summary>
        /// 次のレベルに上がるために必要な経験値を返します
        /// </summary>
        /// <returns>The required exp to next level.</returns>
        /// <param name="currentLevel">Current level.</param>
        public int GetRequiredExpToNextLevel(int currentLevel)
        {
            return currentLevel >= MaxLevel ? 0 : m_requiredExps[currentLevel - 1];
        }

        /// <summary>
        /// レベル上限に達していない、且つ次のレベルに上がるのに必要な経験値を超えていたら
        /// 1レベルアップする.
        /// </summary>
        public void LevelUp()
        {
            var requiredExp = GetRequiredExpToNextLevel(Stats.Level); // 現在のレベルに必要な経験値(総パネル破壊枚数)

            if (MaxLevel >= Stats.Level && requiredExp <= Stats.Level) // レベル上限を越していない且つ必要経験値以上の経験値を取得している　
            {
                return;
            }

            // レベルアップする直前のレベルに合わせてステータスを上昇させる
            if (Stats.Level < 50) // レベル50以下なら
            {
                m_stats.HitPoint += 50;
                m_stats.Attack += 15;
                m_stats.Defense += 15;
            }
            else if (Stats.Level >= 50 && Stats.Level < 100) // レベル50~99なら
            {
                m_stats.HitPoint += 25;
                m_stats.Attack += 10;
                m_stats.Defense += 10;
            }
            else if (Stats.Level >= 100 && Stats.Level < 150) // レベル100~149なら
            {
                m_stats.HitPoint += 10;
                m_stats.Attack += 5;
                m_stats.Defense += 5;
            }
            else if (Stats.Level >= 150 && Stats.Level < 200) // レベル150~199なら
            {
                m_stats.HitPoint += 5;
                m_stats.Attack += 1;
                m_stats.Defense += 1;
            }

            m_stats.Level++; // levelを1上げる
            m_allocationPoint += m_addStatsPoint; // レベルが上がる毎にステータスに振り分ける事が可能なポイントを一定値渡す
        }

        /// <summary>
        /// 現在のマギアのステータスを取得する
        /// 参照渡しにならない様に各値を代入して新しいインスタンスを生成して返す
        /// </summary>
        /// <returns>現在のマギアのステータス</returns>
        public Status GetStats()
        {
            Status result = new Status()
            {
                Level = Stats.Level,
                HitPoint = Stats.HitPoint,
                Attack = Stats.Attack,
                Defense = Stats.Defense,
                Durability = Stats.Durability,
                MuscularStrength = Stats.MuscularStrength,
                Knowledge = Stats.Knowledge,
                Sense = Stats.Sense,
                Charm = Stats.Charm,
                Dignity = Stats.Dignity,
            };
            return result;
        }

        /// <summary>
        /// 強化画面で編集したStatsをmagiaにセットし、固有ステータスを基礎ステータスに反映させる
        /// </summary>
        public void Sync(Status stats = null)
        {
            if (stats != null)
            {
                Stats = stats;
            }
            Stats.Attack = Stats.Attack + (Stats.Sense * m_magnificationByStats); // センスを攻撃力に変換
            Stats.Attack = Stats.Attack + (Stats.MuscularStrength * m_magnificationByStats); // 筋力を攻撃力に変換
            Stats.Defense = Stats.Defense + (Stats.Durability * m_magnificationByStats); // 耐久力を防御力に変換
            Stats.Defense = Stats.Defense + (Stats.Knowledge * m_magnificationByStats); // 知識を防御力に変換
            Stats.HitPoint = Stats.HitPoint + (Stats.Charm * m_magnificationByAttribute); // 魅力をHPに変換
            Stats.HitPoint = Stats.HitPoint + (Stats.Dignity * m_magnificationByAttribute); // 威厳をHPに変換
        }

        /// <summary>
        /// ステージ開始時,InitStateの時にその時のマギアのHP最大値で初期化する
        /// </summary>
        /// <param name="maxHP">Max hp.</param>
        public void InitMaxHP(int maxHP)
        {
            MaxHP = maxHP;
        }



        #endregion

        #region Enum
        /// <summary>属性</summary>
        [Serializable]
        public enum Attribute
        {
            /// <summary>初期形態</summary>
            Standard,
            /// <summary>剣姫</summary>
            SwordPrincess,
            /// <summary>刀皇</summary>
            BladeEmperor,
            /// <summary>女帝</summary>
            Empress,
            /// <summary>黒王</summary>
            BlackKing,
            /// <summary>魔神</summary>
            DevilsGod,
        }

        /// <summary>
        /// レベルアップ獲得スキル。
        /// レベルが一定値上がったら対応したスキルが解放されて、以降永続的に使用可能となる。
        /// </summary>
        [Flags]
        [Serializable]
        public enum PassiveSkill
        {
            /// <summary>無効値</summary>
            Invalid = 0,
            /// <summary>魔拳</summary>
            DevilsFist = 1,
            /// <summary>高濃度魔力吸収,High concentration magical absorption</summary>
            HighConcentrationMagicalAbsorption = 2,
            /// <summary>自己再生</summary>
            SelfRegeneration = 4,
            /// <summary>爆炎熱風柱</summary>
            ExplosiveFlamePillar = 8,
            /// <summary>紅蓮障壁</summary>
            CrimsonBarrier = 16,
            /// <summary>魔拳烈火ノ型</summary>
            DevilsFistInfernoType = 32,
            /// <summary>心焔権現</summary>
            BraveHeartsIncarnation = 64,
            /// <summary>大紅蓮障壁</summary>
            GreatCrimsonBarrier = 128,
            /// <summary>豪炎爆砕掌</summary>
            InfernosFist = 256,
            /// <summary>魔王ノ細胞</summary>
            SatansCell = 512,
            /// <summary>天照権現</summary>
            AmaterasuIncanation = 1024,
            /// <summary>天照-爆炎-</summary>
            AmaterasuInferno = 2048,
            /// <summary>天照-焔壁-</summary>
            AmaterasuFlameWall = 4096,
            /// <summary>全てのスキルフラグ(全てのenumの論理和)</summary>
            AllSkill = 8191,
        }
        #endregion 
    }
}
