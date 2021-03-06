﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemonicCity.BattleScene;

namespace DemonicCity
{
    [Serializable]
    /// <summary>
    ///ストーリーの進行度、進捗
    ///</summary>
    public class Progress : SavableSingletonBase<Progress>
    {
        [Flags]
        public enum TutorialFlag
        {
            Home = 1,
            Battle = 2,
            Strengthen = 4,
        }

        [Flags]
        public enum TutorialFlagInBattleScene
        {
            OpenFirstPanel,
            OpenedPanel,
            MoreOpenPanel,
        }

        /// <summary>ストーリーの進行度</summary>
        [Flags]
        public enum StoryProgress
        {
            /// <summary>序章</summary>
            Prologue = 1,
            /// <summary>1章</summary>
            Phoenix = 2,
            /// <summary>2章</summary>
            Nafla = 4,
            /// <summary>3章</summary>
            ZAKO1 = 8,
            /// <summary>4章</summary>
            Amon = 16,
            /// <summary>5章</summary>
            ZAKO2 = 32,
            /// <summary>6章</summary>
            Ashmedy = 64,
            /// <summary>7章</summary>
            ZAKO3 = 128,
            /// <summary>8章</summary>
            Faulus = 256,
            /// <summary>9章</summary>
            ZAKO4 = 512,
            /// <summary>10章</summary>
            Barl = 1024,
            /// <summary>11章</summary>
            InvigoratedPhoenix = 2048,
            /// <summary>12章</summary>
            Ixmagina = 4096,
            All = 8191,
            //Test = 8192,
        }
        /// <summary>1クエスト内での進行度</summary>

        public enum QuestProgress
        {
            /// <summary>戦闘前のストーリー</summary>
            Prologue = 0,
            /// <summary>戦闘</summary>
            Battle,
            /// <summary>戦闘後のストーリー</summary>
            Epilogue,
            /// <summary>ストーリー外</summary>
            None,

            All,
            Test,
        }

        /// <summary>ストーリーの進行度</summary>
        [SerializeField] StoryProgress storyProgress = 0;

        /// <summary>現在進行しているクエスト</summary>
        [SerializeField] StoryProgress thisStoryProgress = StoryProgress.Prologue;
        /// <summary>現在進行しているクエストの進行度</summary>
        [SerializeField] QuestProgress questProgress = QuestProgress.Prologue;

        /// <summary>Tutorialを出すかどうかを決定するフラグ</summary>
        [SerializeField] TutorialFlag tutorialProgress = 0;
        /// <summary>バトルシーンのTutorialを出すかどうかを決定するフラグ</summary>
        [SerializeField] Subject tutorialInBattleScene = Subject.AllFlag;

        /// <summary>ストーリーの進行度のプロパティ</summary>
        public StoryProgress MyStoryProgress
        {
            get { return storyProgress; }
            set { storyProgress = value; Save(); }
        }

        /// <summary>現在進行しているクエストのプロパティ</summary>
        public StoryProgress ThisStoryProgress
        {
            get { return thisStoryProgress; }
            set { thisStoryProgress = value; Save(); }
        }

        /// <summary>現在進行しているクエストの進行度のプロパティ</summary>
        public QuestProgress ThisQuestProgress
        {
            get { return questProgress; }
            set { questProgress = value; Save(); }
        }

        public TutorialFlag TutorialProgress
        {
            get { return tutorialProgress; }
            set { tutorialProgress = value; Save(); }
        }
        public Subject TutorialProgressInBattleScene
        {
            get { return tutorialInBattleScene; }
            set { tutorialInBattleScene = value; Save(); }
        }


        /// <summary>
        /// 指定したFlagが立っているか
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool TutorialCheck(TutorialFlag flag)
        {
            if ((tutorialProgress & flag) == flag)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// チュートリアル用のFlagのセット
        /// </summary>
        /// <param name="flag">対象のFlag</param>
        /// <param name="isTrue">Trueなら立てる、Falseなら降ろす</param>
        public void SetTutorialProgress(TutorialFlag flag, bool isTrue)
        {
            if (isTrue)
            {
                TutorialProgress = tutorialProgress | flag;
            }
            else
            {
                TutorialProgress = tutorialProgress & (~flag);
            }
        }

        /// <summary>
        /// チュートリアル用のFlagのセット
        /// </summary>
        /// <param name="flag">対象のFlag</param>
        /// <param name="isTrue">Trueなら立てる、Falseなら降ろす</param>
        public void SetTutorialProgress(Subject flag, bool isTrue)
        {
            if (isTrue)
            {
                TutorialProgressInBattleScene = tutorialInBattleScene | flag;
            }
            else
            {
                TutorialProgressInBattleScene = tutorialInBattleScene & (~flag);
            }
        }

        /// <summary>
        /// 現在の進行度のクエストのクリアフラグを立てる
        /// </summary>
        public void QuestClear()
        {
            MyStoryProgress = storyProgress | thisStoryProgress;
        }


        public StoryProgress NextStory(StoryProgress nowStory)
        {
            int tmpStory = 0;
            foreach (StoryProgress story in Enum.GetValues(typeof(StoryProgress)))
            {
                if ((nowStory & story) == story)
                {
                    tmpStory = (int)story;
                }
            }
            tmpStory = tmpStory << 1;
            return (StoryProgress)tmpStory;
        }


        public bool IsClear
        {
            get
            {
                if ((storyProgress & StoryProgress.Ixmagina) == StoryProgress.Ixmagina)
                {
                    return true;
                }
                return false;
            }
        }
        public Progress()
        {
            storyProgress = 0;
            thisStoryProgress = 0;
            questProgress = 0;
            tutorialProgress = 0;
            tutorialInBattleScene = Subject.AllFlag;
        }


    }
}
