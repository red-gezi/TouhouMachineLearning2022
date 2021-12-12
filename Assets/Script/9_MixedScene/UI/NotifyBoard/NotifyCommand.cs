﻿using System;
using System.Threading.Tasks;
using TouhouMachineLearningSummary.Thread;
using TouhouMachineLearningSummary.Extension;
using TouhouMachineLearningSummary.GameEnum;
using UnityEngine;
using UnityEngine.UI;
namespace TouhouMachineLearningSummary.Command.GameUI
{
    public class NoticeCommand : MonoBehaviour
    {
        string language = "Ch";
        static Func<Task> okAction;
        static Func<Task> cancelAction;
        static Func<string, Task> inputAction;

        static Image image => Info.GameUI.UiInfo.Notice.transform.GetComponent<Image>();
        static Transform noticeTransform => Info.GameUI.UiInfo.Notice.transform;
        static Text noticeText => noticeTransform.GetChild(1).GetComponent<Text>();
        static Transform okButton => noticeTransform.GetChild(2);
        static Transform cancelButton => noticeTransform.GetChild(3);
        static Transform inputlButton => noticeTransform.GetChild(4);
        static Transform inputField => noticeTransform.GetChild(5);
        static bool isShowOver = true;
        public static async Task OkAsync()
        {
            _ = Command.AudioCommand.PlayAsync(GameEnum.GameAudioType.UiButton);
            await CloseAsync();
            await Task.Delay(1000);
            if (okAction != null)
            {
                await okAction();
            }
            await Task.Delay(500);
            isShowOver = true;
        }

        public static async Task CancaelAsync()
        {
            _ = Command.AudioCommand.PlayAsync(GameEnum.GameAudioType.UiButton);
            await CloseAsync();
            await Task.Delay(1000);
            if (cancelAction != null)
            {
                await cancelAction();
            }
            await Task.Delay(500);
            isShowOver = true;
        }

        public static async Task InputAsync()
        {
            await CloseAsync();
            if (inputAction != null)
            {
                string newDeckName = inputField.GetChild(2).GetComponent<Text>().text;

                if (newDeckName == "")
                {
                    newDeckName = inputField.GetChild(1).GetComponent<Text>().text;
                }
                await inputAction(newDeckName);
            }
            isShowOver = true;
        }

        public static async Task ShowAsync
            (
            string text,
            NotifyBoardMode notifyBoardMode = NotifyBoardMode.Ok_Cancel,
            Func<Task> okAction = null,
            Func<Task> cancelAction = null,
            Func<string, Task> inputAction = null,
            string inputField = ""
            )
        {
            isShowOver = false;
            NoticeCommand.okAction = okAction;
            NoticeCommand.cancelAction = cancelAction;
            NoticeCommand.inputAction = inputAction;
            Color color = image.color;
            noticeText.text = text;
            Info.GameUI.UiInfo.Notice.SetActive(true);
            okButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            inputlButton.gameObject.SetActive(false);
            NoticeCommand.inputField.gameObject.SetActive(false);
            switch (notifyBoardMode)
            {
                case NotifyBoardMode.Ok:
                    okButton.gameObject.SetActive(true);
                    okButton.localPosition = new Vector3(0, -100, 0);
                    break;
                case NotifyBoardMode.Ok_Cancel:
                    okButton.gameObject.SetActive(true);
                    cancelButton.gameObject.SetActive(true);
                    okButton.localPosition = new Vector3(-130, -100, 0);
                    cancelButton.localPosition = new Vector3(130, -100, 0);
                    break;
                case NotifyBoardMode.Cancel:
                    okButton.gameObject.SetActive(false);
                    cancelButton.gameObject.SetActive(true);
                    cancelButton.localPosition = new Vector3(0, -100, 0);
                    break;
                case NotifyBoardMode.Input:
                    cancelButton.gameObject.SetActive(true);
                    inputlButton.gameObject.SetActive(true);
                    NoticeCommand.inputField.gameObject.SetActive(true);
                    NoticeCommand.inputField.GetComponent<InputField>().text = "";
                    NoticeCommand.inputField.GetChild(1).GetComponent<Text>().text = inputField;
                    inputlButton.localPosition = new Vector3(-130, -100, 0);
                    cancelButton.localPosition = new Vector3(130, -100, 0);

                    break;
                default:
                    break;
            }
            await CustomThread.TimerAsync(0.3f, runAction: process => //在0.5秒内不断缩小并降低透明度
            {
                noticeTransform.localScale = new Vector3(1, process, 1);
                image.color = color.SetA(process);
            });
            while (!isShowOver)
            {
                await Task.Delay(10);
            }
        }
        public static async Task CloseAsync()
        {
            Color color = image.color;
            await CustomThread.TimerAsync(0.3f,runAction: process =>
                 {
                     noticeTransform.localScale = new Vector3(1, 1 - process, 1);
                     image.color = color.SetA(1 - process);
                 });
            Info.GameUI.UiInfo.Notice.SetActive(false);
        }
    }
}