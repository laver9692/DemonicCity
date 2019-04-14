﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DemonicCity
{
    public class TutorialsPopper : MonoBehaviour
    {

        public TutorialItems.TutorialItem NextItem
        {
            get
            {
                var index = tutorialObject.Items.IndexOf(currentItem);
                if (index < tutorialObject.Items.Count - 1)
                {
                    return tutorialObject.Items[++index];
                }
                return null;
            }
        }

        public TutorialItems.TutorialItem PreviousItem
        {
            get
            {
                var index = tutorialObject.Items.IndexOf(currentItem);
                if (index > 0)
                {
                    return tutorialObject.Items[--index];
                }
                return null;
            }
        }



        /// <summary>表示させるチュートリアルの素材群</summary>
        [SerializeField] TutorialItems tutorialObject;
        /// <summary>次へボタン</summary>
        [SerializeField] Button toNextButton;
        /// <summary>前へボタン</summary>
        [SerializeField] Button toPreviousButton;
        /// <summary>閉じるボタン</summary>
        [SerializeField] Button closeButton;
        /// <summary>itween animationに使う時間</summary>
        [SerializeField] float fadingTime = .5f;

        /// <summary>popup system</summary>
        PopupSystem popupSystem;
        /// <summary>現在対象となっている素材</summary>
        TutorialItems.TutorialItem currentItem;
        /// <summary>popup materials</summary>
        List<PopupSystemMaterial> popupMaterials;
        /// <summary>チュートリアル画像を生成するオブジェクトの親</summary>
        GameObject tutorialImagesParent;
        /// <summary>次へボタン</summary>
        Button popupedToNextButton;
        /// <summary>前へボタン</summary>
        Button popupedToPreviousButton;
        /// <summary>閉じるボタン</summary>
        Button popupedCloseButton;
        AudioSource audioSource;


        const int width = 1080;
        const int height = 1920;


        private void Start()
        {
            popupSystem = GetComponent<PopupSystem>();
            popupMaterials = new List<PopupSystemMaterial>
            {
                new PopupSystemMaterial(OnPushNextButton,toNextButton.gameObject.name,false),
                new PopupSystemMaterial(OnPushPreviousButton,toPreviousButton.gameObject.name,false),
                new PopupSystemMaterial(Close,closeButton.gameObject.name,true),
            };
        }

        public void Popup()
        {
            popupSystem.Popup();
            popupMaterials.ForEach(material => popupSystem.SubscribeButton(material));
            var imageSize = new Vector2(width, height);
            float xPos = 0;
            tutorialObject.Items.ForEach(item =>
            {
                var go = new GameObject();
                go.transform.parent = popupSystem.popupedObject.transform.GetChild(0);
                var image = go.AddComponent<Image>();
                image.sprite = item.Sprite;
                image.rectTransform.sizeDelta = imageSize;
                image.rectTransform.position = new Vector2(xPos + (width * 0.5f), 0);
                xPos += width;
                image.rectTransform.localScale = Vector2.one;
            });

            OnPopup();
        }

        void OnPopup()
        {
            // on pupuped.
            currentItem = tutorialObject.Items.First();
            audioSource = popupSystem.popupedObject.GetComponent<AudioSource>();
            popupedToNextButton = GameObject.Find(toNextButton.gameObject.name).GetComponent<Button>();
            popupedToPreviousButton = GameObject.Find(toPreviousButton.gameObject.name).GetComponent<Button>();
            popupedCloseButton = GameObject.Find(closeButton.gameObject.name).GetComponent<Button>();
            tutorialImagesParent = popupSystem.popupedObject.transform.GetChild(0).gameObject;
            OnChangeItem();
        }

        /// <summary>
        /// fading image
        /// </summary>
        void FadingImage(Index index)
        {

            switch (index)
            {
                case Index.Next:
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(-width, 0), "time", fadingTime));
                    break;
                case Index.Previous:
                    iTween.MoveBy(tutorialImagesParent, iTween.Hash("amount", new Vector3(width, 0), "time", fadingTime));
                    break;
                case Index.Last:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 閉じるボタンのイベントハンドラ
        /// </summary>
        protected virtual void Close() { }

        /// <summary>
        /// 次へボタンのイベントハンドラ
        /// </summary>
        void OnPushNextButton()
        {
            ChangeItem(Index.Next);
            FadingImage(Index.Next);
            OnChangeItem();
        }

        /// <summary>
        /// 前へボタンのイベントハンドラ
        /// </summary>
        void OnPushPreviousButton()
        {
            ChangeItem(Index.Previous);
            FadingImage(Index.Previous);
            OnChangeItem();
        }

        /// <summary>
        /// Itemを変更した時
        /// </summary>
        void OnChangeItem()
        {


            // ボタンが表示可能かどうか判断し,ボタンを表示するかしないか決定する
            CheckButtonVibible(popupedToNextButton, Index.Next);
            CheckButtonVibible(popupedToPreviousButton, Index.Previous);
            CheckButtonVibible(popupedCloseButton, Index.Last);

            // 前の画面で再生されていた音声を停止して次の音声が存在する場合音声を再生させる
            audioSource.Stop();
            if (currentItem.useVoice)
            {
                audioSource.Play(currentItem.VoiceClip);
            }
        }


        /// <summary>
        /// ボタンが表示可能かどうかチェックし,trueならコンポーネントを有効に,falseなら無効にする
        /// /// </summary>
        /// <param name="button"></param>
        /// <param name="direction"></param>
        void CheckButtonVibible(Button button, Index direction)
        {
            switch (direction)
            {
                case Index.Next:
                    if (NextItem != null)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                case Index.Previous:
                    if (PreviousItem != null)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                case Index.Last:
                    var index = tutorialObject.Items.IndexOf(currentItem);
                    if (index == tutorialObject.Items.Count - 1)
                    {
                        button.gameObject.SetActive(true);
                    }
                    else
                    {
                        button.gameObject.SetActive(false);
                    }
                    break;
                default:
                    throw new System.ArgumentException("Directionが適切に指定されていません.");
            }
        }

        /// <summary>
        /// Itemを適切な要素へ変更する
        /// </summary>
        /// <param name="direction"></param>
        void ChangeItem(Index direction)
        {
            var index = tutorialObject.Items.IndexOf(currentItem);
            switch (direction)
            {
                case Index.Next:
                    currentItem = tutorialObject.Items[++index];
                    break;
                case Index.Previous:
                    currentItem = tutorialObject.Items[--index];
                    break;
                default:
                    throw new System.ArgumentException("Directionが適切に指定されていません.");
            }
        }

        ///// <summary>
        ///// 指定した要素が存在するかどうか
        ///// </summary>
        ///// <param name="direction"></param>
        ///// <returns></returns>
        //bool HasElement(Direction direction)
        //{
        //    var index = tutorialObject.Items.IndexOf(currentItem);
        //    switch (direction)
        //    {
        //        case Direction.Next:
        //            return ++index < tutorialObject.Items.Count-1;
        //        case Direction.Previous:
        //            return --index >= 0;
        //        default:
        //            throw new System.ArgumentException("Directionが適切に指定されていません.");
        //    }
        //}
    }
    enum Index
    {
        Next,
        Previous,
        Last,
    }
}