﻿using UnityEngine;
using TMPro;
using System;
using DemonicCity.BattleScene;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace DemonicCity.StrengthenScene
{
    public class Strengthen_Part1 : MonoBehaviour
    {
        /// <summary>Magiaクラスのインスタンス</summary>
        Magia magia;
        /// <summary>TouchGestureDetectorクラスのインスタンス</summary>
        TouchGestureDetector touchGestureDetector;

        [SerializeField]private TutorialsPopper popupSystem;

        /// <summary>現在の体力</summary>
        private int currentHp;
        /// <summary>現在の攻撃力</summary>
        private int currentAttack;
        /// <summary>現在の防御力</summary>
        private int currentDefence;

        /// <summary>変動後の体力</summary>
        private int updatedHitPoint;
        /// <summary>変動後の攻撃力</summary>
        private int updatedAttack;
        /// <summary>変動後の防御力</summary>
        private int updatedDefence;

        /// <summary>現在の耐久値</summary>
        private int durability;
        /// <summary>現在の筋力値</summary>
        private int muscularStrength;
        /// <summary>現在の知識値</summary>
        private int knowledge;
        /// <summary>現在の感覚値</summary>
        private int sense;
        /// <summary>現在の魅力値</summary>
        private int charm;
        /// <summary>現在の威厳値</summary>
        private int dignity;

        /// <summary>割り振りポイント(魔力値)</summary>
        private int statusPoint;
        /// <summary>割り振られた魅力値</summary>
        private int addCharm;
        /// <summary>割り振られた耐久値</summary>
        private int addDurability;
        /// <summary>割り振られた筋力値</summary>
        private int addMuscularStrength;
        /// <summary>割り振られた知識値</summary>
        private int addKnowledge;
        /// <summary>割り振られた感覚値</summary>
        private int addSense;
        /// <summary>割り振られた威厳値</summary>
        private int addDignity;

        /// <summary>現在の基礎ステータステキスト</summary>
        TextMeshProUGUI[] currentBasicStatusTexts = new TextMeshProUGUI[3];
        /// <summary>変動後の基礎ステータステキスト</summary>
        TextMeshProUGUI[] updatedBasicStatusTexts = new TextMeshProUGUI[3];
        /// <summary>現在の固有ステータステキスト</summary>
        TextMeshProUGUI[] currentUniqueStatusTexts = new TextMeshProUGUI[6];
        /// <summary>割り振った固有ステータステキスト</summary>
        TextMeshProUGUI[] addUniqueStatusTexts = new TextMeshProUGUI[6];
        /// <summary>割り振りポイントテキスト(魔力値)</summary>
        TextMeshProUGUI statusPointText;

        /// <summary>ステータス確定前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject confirmMessageWindow;
        /// <summary>ステータス初期化前のメッセージウィンドウ</summary>
        [SerializeField] private GameObject resetMessageWindow;
        /// <summary>確定ボタンとリセットボタン</summary>
        [SerializeField] private GameObject confirmAndResetButtons;
        /// <summary>習得済みスキル一覧ウィンドウ</summary>
        [SerializeField] private GameObject skillListWindow;
        /// <summary>各スキル名</summary>
        private GameObject[] skillNameTexts;
        /// <summary>各スキルの説明</summary>
        private GameObject[] skillDescriptionTexts = null;

        /// <summary>ポップアップウィンドウの表示/非表示</summary>
        private bool activePopUpWindowFlag = false;
        /// <summary>スキル説明テキストの表示/非表示</summary>
        private int activeSkillDescriptionText;
        private bool setActiveSkillDescriptionText = false;
        /// <summary>確定/中止ボタンの表示/非表示</summary>
        private bool changedStatusFlag = false;

        /// <summary>シーンがロードされたか</summary>
        private bool sceneLoaded = false;

        [SerializeField] private GameObject backGround = null;

        public enum PopUpAnimation
        {
            Close_PopUpWindow
        }

        private void Awake()
        {
            magia = Magia.Instance;
            touchGestureDetector = TouchGestureDetector.Instance;
        }

        /// <summary>ウィンドウが閉じるときのアニメーション処理</summary>
        private IEnumerator ClosePopUpAnimation(GameObject window)
        {
            window.GetComponent<Animator>().CrossFadeInFixedTime(PopUpAnimation.Close_PopUpWindow.ToString(), 0);
            yield return new WaitForSeconds(0.5f);
            window.SetActive(false);
        }

        private void Start()
        {
            GetGameObjects();
            ResetStatus();

           
            touchGestureDetector.onGestureDetected.AddListener((gesture, touchInfo) =>
            {
                if (gesture == TouchGestureDetector.Gesture.TouchBegin)
                {
                    GameObject button;
                    touchInfo.HitDetection(out button);

                    if (button != null)
                    {
                        switch (button.name)
                        {
                            case "BackToHomeSceneButton":
                                StartCoroutine(ClosePopUpAnimation(backGround));
                                //SceneChanger.SceneChange(SceneName.Home);
                                Destroy(gameObject);
                                break;

                            case "ShowSkillButton":
                                if (!activePopUpWindowFlag)
                                {
                                    skillListWindow.SetActive(true);

                                    skillDescriptionTexts = GameObject.FindGameObjectsWithTag("SkillDescriptionText");
                                    for (int i = 0; i < skillDescriptionTexts.Length; i++)
                                    {
                                        skillDescriptionTexts[i].SetActive(false);
                                    }
                                    activePopUpWindowFlag = true;
                                }
                                break;

                            case "BackToAllocationWindow":
                                if (activePopUpWindowFlag)
                                {
                                    StartCoroutine(ClosePopUpAnimation(skillListWindow));
                                    activePopUpWindowFlag = false;
                                }
                                break;

                            //ここから各スキル名の処理
                            case "DevilsFist":
                                SkillDescriptionManager(0);
                                activeSkillDescriptionText = 0;
                                break;
                            case "HighConcentrationMagicalAbsorption":
                                SkillDescriptionManager(1);
                                activeSkillDescriptionText = 1;
                                break;
                            case "SelfRegeneration":
                                SkillDescriptionManager(2);
                                activeSkillDescriptionText = 2;
                                break;
                            case "ExplosiveFlamePillar":
                                SkillDescriptionManager(3);
                                activeSkillDescriptionText = 3;
                                break;
                            case "CrimsonBarrier":
                                SkillDescriptionManager(4);
                                activeSkillDescriptionText = 4;
                                break;
                            case "DevilsFistInfernoType":
                                SkillDescriptionManager(5);
                                activeSkillDescriptionText = 5;
                                break;
                            case "BraveHeartsIncarnation":
                                SkillDescriptionManager(6);
                                activeSkillDescriptionText = 6;
                                break;
                            case "GreatCrimsonBarrier":
                                SkillDescriptionManager(7);
                                activeSkillDescriptionText = 7;
                                break;
                            case "InfernosFist":
                                SkillDescriptionManager(8);
                                activeSkillDescriptionText = 8;
                                break;
                            case "SatansCell":
                                SkillDescriptionManager(9);
                                activeSkillDescriptionText = 9;
                                break;
                            case "AmaterasuIncanation":
                                SkillDescriptionManager(10);
                                activeSkillDescriptionText = 10;
                                break;
                            case "AmaterasuInferno":
                                SkillDescriptionManager(11);
                                activeSkillDescriptionText = 11;
                                break;
                            case "AmaterasuFlameWall":
                                SkillDescriptionManager(12);
                                activeSkillDescriptionText = 12;
                                break;
                            case "AllSkill":
                                SkillDescriptionManager(13);
                                activeSkillDescriptionText = 13;
                                break;
                            case "SuzakuFlameFist":
                                SkillDescriptionManager(14);
                                activeSkillDescriptionText = 14;
                                break;
                            case "DevilsEye":
                                SkillDescriptionManager(15);
                                activeSkillDescriptionText = 15;
                                break;

                            //ここまで各スキル名の処理

                            case "AddCharmButton":
                                ChangeUniqueStatus(ref charm, ref addCharm, ref addUniqueStatusTexts[0]);
                                break;

                            case "AddDignityButton":
                                ChangeUniqueStatus(ref dignity, ref addDignity, ref addUniqueStatusTexts[1]);
                                break;

                            case "AddMuscularStrengthButton":
                                ChangeUniqueStatus(ref muscularStrength, ref addMuscularStrength, ref addUniqueStatusTexts[2]);
                                break;

                            case "AddSenseButton":
                                ChangeUniqueStatus(ref sense, ref addSense, ref addUniqueStatusTexts[3]);
                                break;

                            case "AddDurabilityButton":
                                ChangeUniqueStatus(ref durability, ref addDurability, ref addUniqueStatusTexts[4]);
                                break;

                            case "AddKnowledgeButton":
                                ChangeUniqueStatus(ref knowledge, ref addKnowledge, ref addUniqueStatusTexts[5]);
                                break;

                            case "ConfirmButton":
                                if (!activePopUpWindowFlag)
                                {
                                    confirmMessageWindow.SetActive(true);
                                    activePopUpWindowFlag = true;
                                }
                                break;

                            case "ResetButton":
                                if (!activePopUpWindowFlag)
                                {
                                    resetMessageWindow.SetActive(true);
                                    activePopUpWindowFlag = true;
                                }
                                break;

                            case "YesConfirm":
                                ConfirmStatus();
                                break;

                            case "YesReset":
                                ResetStatus();
                                break;

                            case "No":
                                if (activePopUpWindowFlag)
                                {
                                    StartCoroutine(ClosePopUpAnimation(confirmMessageWindow));
                                    StartCoroutine(ClosePopUpAnimation(resetMessageWindow));
                                    activePopUpWindowFlag = false;
                                }
                                break;
                        }
                    }
                }
            });
        }


        private void Update()
        {
            if (changedStatusFlag)
            {
                confirmAndResetButtons.SetActive(true);
            }
            else
            {
                StartCoroutine(ClosePopUpAnimation(confirmAndResetButtons));
            }

        }

        /// <summary>スキルの説明テキストを表示/非表示</summary>
        private void SkillDescriptionManager(int index)
        {
            if (activeSkillDescriptionText == index && !setActiveSkillDescriptionText)
            {
                skillDescriptionTexts[index].SetActive(true);
                setActiveSkillDescriptionText = true;
            }
            else if (activeSkillDescriptionText == index && setActiveSkillDescriptionText)
            {
                StartCoroutine(ClosePopUpAnimation(skillDescriptionTexts[index]));
                setActiveSkillDescriptionText = false;
            }
        }

        /// <summary>魔力値を固有ステータスに割り振り、基礎ステータスに変換する</summary>
        /// <param name="addUniqueStatus">固有ステータス</param>
        /// <param name="uniqueStatusText">固有ステータスのテキスト</param>
        /// <param name="addStatus">ステータスの増減判定</param>
        private void ChangeUniqueStatus(ref int uniqueStatus, ref int addUniqueStatus, ref TextMeshProUGUI uniqueStatusText)
        {
            if (uniqueStatus + addUniqueStatus < 200)
            {
                addUniqueStatus += AddStatusPoint(addUniqueStatus);
                if (statusPoint >= 0)
                {
                    uniqueStatusText.text = "+" + addUniqueStatus.ToString();
                    //固有ステータスを基礎ステータスに変換
                    updatedHitPoint = currentHp + (addCharm * 50) + (addDignity * 50);
                    updatedAttack = currentAttack + (addSense * 5) + (addMuscularStrength * 5);
                    updatedDefence = currentDefence + (addDurability * 5) + (addKnowledge * 5);

                    updatedBasicStatusTexts[0].text = updatedHitPoint.ToString();
                    updatedBasicStatusTexts[1].text = updatedAttack.ToString();
                    updatedBasicStatusTexts[2].text = updatedDefence.ToString();

                    changedStatusFlag = true;
                }
            }
        }

        /// <summary>ステータスの変動値を初期化</summary>
        private void ResetStatus()
        {
            var status = magia.GetStats();

            currentHp = status.HitPoint;
            currentAttack = status.Attack;
            currentDefence = status.Defense;
            charm = status.Charm;
            dignity = status.Dignity;
            muscularStrength = status.MuscularStrength;
            sense = status.Sense;
            durability = status.Durability;
            knowledge = status.Knowledge;

            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;
            statusPoint = magia.AllocationPoint;
            // statusPoint = 300;//debug

            updatedBasicStatusTexts[0].text = currentHp.ToString();
            updatedBasicStatusTexts[1].text = currentAttack.ToString();
            updatedBasicStatusTexts[2].text = currentDefence.ToString();

            UpdateText();
            for (int i = 0; i < updatedBasicStatusTexts.Length; i++)
            {
                updatedBasicStatusTexts[i].text = "";
            }

            for (int i = 0; i < addUniqueStatusTexts.Length; i++)
            {
                addUniqueStatusTexts[i].text = "";
            }

            if (!sceneLoaded)
            {
                skillListWindow.SetActive(false);
                resetMessageWindow.SetActive(false);
                confirmMessageWindow.SetActive(false);
                confirmAndResetButtons.SetActive(false);

                sceneLoaded = true;
            }

            StartCoroutine(ClosePopUpAnimation(skillListWindow));
            StartCoroutine(ClosePopUpAnimation(resetMessageWindow));
            StartCoroutine(ClosePopUpAnimation(confirmMessageWindow));
            StartCoroutine(ClosePopUpAnimation(confirmAndResetButtons));

            activePopUpWindowFlag = false;
            changedStatusFlag = false;
        }

        /// <summary>変更したステータスを確定する</summary>
        private void ConfirmStatus()
        {
            currentHp = updatedHitPoint;
            currentAttack = updatedAttack;
            currentDefence = updatedDefence;

            charm += addCharm;
            dignity += addDignity;
            muscularStrength += addMuscularStrength;
            sense += addSense;
            durability += addDurability;
            knowledge += addKnowledge;

            addCharm = 0;
            addDignity = 0;
            addMuscularStrength = 0;
            addSense = 0;
            addDurability = 0;
            addKnowledge = 0;

            magia.Stats.HitPoint = currentHp;
            magia.Stats.Attack = currentAttack;
            magia.Stats.Defense = currentDefence;
            magia.Stats.Charm = charm;
            magia.Stats.Dignity = dignity;
            magia.Stats.MuscularStrength = muscularStrength;
            magia.Stats.Sense = sense;
            magia.Stats.Durability = durability;
            magia.Stats.Knowledge = knowledge;
            magia.AllocationPoint = statusPoint;

            SavableSingletonBase<Magia>.Instance.Save();

            UpdateText();
            for (int i = 0; i < updatedBasicStatusTexts.Length; i++)
            {
                updatedBasicStatusTexts[i].text = "";
            }

            for (int i = 0; i < addUniqueStatusTexts.Length; i++)
            {
                addUniqueStatusTexts[i].text = "";
            }
            StartCoroutine(ClosePopUpAnimation(confirmMessageWindow));
            activePopUpWindowFlag = false;
            changedStatusFlag = false;
        }

        /// <summary>割り振りポイント-1、固有ステータスポイント+1</summary>
        /// <param name="uniqueStatus">固有ステータス</param>
        private int AddStatusPoint(int uniqueStatus)
        {
            if (statusPoint > 0)
            {
                statusPoint -= 1;
                statusPointText.text = statusPoint.ToString();
                uniqueStatus = 1;
            }
            else
            {
                uniqueStatus = 0;
                statusPoint = 0;
            }
            return uniqueStatus;
        }

        /// <summary>テキストを更新</summary>
        private void UpdateText()
        {
            currentBasicStatusTexts[0].text = currentHp.ToString();
            currentBasicStatusTexts[1].text = currentAttack.ToString();
            currentBasicStatusTexts[2].text = currentDefence.ToString();

            currentUniqueStatusTexts[0].text = charm.ToString();
            currentUniqueStatusTexts[1].text = dignity.ToString();
            currentUniqueStatusTexts[2].text = muscularStrength.ToString();
            currentUniqueStatusTexts[3].text = sense.ToString();
            currentUniqueStatusTexts[4].text = durability.ToString();
            currentUniqueStatusTexts[5].text = knowledge.ToString();

            addUniqueStatusTexts[0].text = addCharm.ToString();
            addUniqueStatusTexts[1].text = addDignity.ToString();
            addUniqueStatusTexts[2].text = addMuscularStrength.ToString();
            addUniqueStatusTexts[3].text = addSense.ToString();
            addUniqueStatusTexts[4].text = addDurability.ToString();
            addUniqueStatusTexts[5].text = addKnowledge.ToString();

            statusPointText.text = statusPoint.ToString();
        }

        /// <summary>シーン上にあるゲームオブジェクトを取得</summary>
        private void GetGameObjects()
        {
            currentBasicStatusTexts[0] = GameObject.Find("CurrentHpText").GetComponent<TextMeshProUGUI>();
            currentBasicStatusTexts[1] = GameObject.Find("CurrentAttackText").GetComponent<TextMeshProUGUI>();
            currentBasicStatusTexts[2] = GameObject.Find("CurrentDefenceText").GetComponent<TextMeshProUGUI>();

            updatedBasicStatusTexts[0] = GameObject.Find("UpdatedHpText").GetComponent<TextMeshProUGUI>();
            updatedBasicStatusTexts[1] = GameObject.Find("UpdatedAttackText").GetComponent<TextMeshProUGUI>();
            updatedBasicStatusTexts[2] = GameObject.Find("UpdatedDefenceText").GetComponent<TextMeshProUGUI>();

            currentUniqueStatusTexts[0] = GameObject.Find("CurrentCharmText").GetComponent<TextMeshProUGUI>();
            currentUniqueStatusTexts[1] = GameObject.Find("CurrentDignityText").GetComponent<TextMeshProUGUI>();
            currentUniqueStatusTexts[2] = GameObject.Find("CurrentMuscularStrengthText").GetComponent<TextMeshProUGUI>();
            currentUniqueStatusTexts[3] = GameObject.Find("CurrentSenseText").GetComponent<TextMeshProUGUI>();
            currentUniqueStatusTexts[4] = GameObject.Find("CurrentDurabilityText").GetComponent<TextMeshProUGUI>();
            currentUniqueStatusTexts[5] = GameObject.Find("CurrentKnowledgeText").GetComponent<TextMeshProUGUI>();


            addUniqueStatusTexts[0] = GameObject.Find("AddCharmText").GetComponent<TextMeshProUGUI>();
            addUniqueStatusTexts[1] = GameObject.Find("AddDignityText").GetComponent<TextMeshProUGUI>();
            addUniqueStatusTexts[2] = GameObject.Find("AddMuscularStrengthText").GetComponent<TextMeshProUGUI>();
            addUniqueStatusTexts[3] = GameObject.Find("AddSenseText").GetComponent<TextMeshProUGUI>();
            addUniqueStatusTexts[4] = GameObject.Find("AddDurabilityText").GetComponent<TextMeshProUGUI>();
            addUniqueStatusTexts[5] = GameObject.Find("AddKnowledgeText").GetComponent<TextMeshProUGUI>();

            statusPointText = GameObject.Find("StatusPointText").GetComponent<TextMeshProUGUI>();
          
        }
    }
}