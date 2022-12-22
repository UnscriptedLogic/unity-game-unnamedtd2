using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnitManagement;

namespace GameManagement
{
    public enum ModificationType
    {
        Add,
        Subtract,
        Set,
        Divide,
        Multiply
    }

    public class CurrencyManager : MonoBehaviour, IListensToUnitHealthModified
    {
        [SerializeField] private UnitEventHandler unitEventHandler;
        [SerializeField] private float startCash;
        [SerializeField] private TextMeshProUGUI cashText;
        private float currentCash;

        public float Cash => currentCash;

        public Action<ModificationType, float, float> OnCashModified;

        public void InitCash()
        {
            ModifyCurrency(ModificationType.Set, startCash);
            unitEventHandler.OnUnitHealthModified += OnUnitHealthDeducted;
        }

        public void ModifyCurrency(ModificationType modificationType, float amount)
        {
            switch (modificationType)
            {
                case ModificationType.Add:
                    currentCash += amount;
                    break;
                case ModificationType.Subtract:
                    currentCash -= amount;
                    break;
                case ModificationType.Set:
                    currentCash = amount;
                    break;
                case ModificationType.Divide:
                    currentCash /= amount;
                    break;
                case ModificationType.Multiply:
                    currentCash *= amount;
                    break;
                default:
                    break;
            }

            OnCashModified?.Invoke(modificationType, amount, currentCash);
            cashText.text = $"${currentCash}";
        }

        public void AddCash(float amount)
        {
            ModifyCurrency(ModificationType.Add, amount);
        }

        public void OnUnitHealthDeducted(ModificationType modificationType, string id, float modAmount, float currHealth)
        {
            if (id.Contains("enemy"))
            {
                if (modificationType == ModificationType.Subtract)
                {
                    ModifyCurrency(ModificationType.Add, modAmount);
                }
            }
        }
    }
}