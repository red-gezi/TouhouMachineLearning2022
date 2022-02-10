﻿using TouhouMachineLearningSummary.Command;
using TouhouMachineLearningSummary.GameEnum;
using TouhouMachineLearningSummary.Info;
using TouhouMachineLearningSummary.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TouhouMachineLearningSummary.Manager
{
    public class CardManager : MonoBehaviour
    {
        public CardAbilityBoardManager cardAbilityBoardManager;
        int gap_step = 0;
        Card thisCard => GetComponent<Card>();
        GameObject gap => transform.GetChild(1).gameObject;
        Material gapMaterial => gap.GetComponent<Renderer>().material;
        Material cardMaterial => GetComponent<Renderer>().material;
        private void OnMouseEnter()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                AgainstInfo.playerFocusCard = thisCard;
                NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
        private void OnMouseExit()
        {
            if (AgainstInfo.playerFocusCard == thisCard)
            {
                AgainstInfo.playerFocusCard = null;
                NetCommand.AsyncInfo(NetAcyncType.FocusCard);
            }
        }
        private void OnMouseDown() => CardCommand.OnMouseDown(thisCard);
        private void OnMouseUp() => CardCommand.OnMouseUp(thisCard);
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonUp(1) && thisCard.IsCanSee)
            {
                cardAbilityBoardManager.LoadCardFromGameCard(gameObject);
            }
        }
        private void Update()
        {
            if (gap_step == 1)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 1.5f, Time.deltaTime * 20));
            }
            else if (gap_step == 2)
            {
                gapMaterial.SetFloat("_gapWidth", Mathf.Lerp(gapMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
                cardMaterial.SetFloat("_gapWidth", Mathf.Lerp(cardMaterial.GetFloat("_gapWidth"), 10, Time.deltaTime * 2));
            }

        }
        public void CreatGap()
        {
            gap.SetActive(true);
            gap_step = 1;
        }
        public void FoldGap()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            gap_step = 2;
        }
        public void DestoryGap()
        {
            gap.SetActive(false);
            gap_step = 0;
            Destroy(gameObject);
        }
    }
}