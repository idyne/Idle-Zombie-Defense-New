using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.Core
{
    [CreateAssetMenu(menuName = "Fate/Variable/Save Data", fileName = "Save Data Variable")]
    public class SaveDataVariable : Variable<SaveData>
    {
        public UnityEvent<int, int> OnMoneyChanged = new();
        public UnityEvent<int, int> OnToolsChanged = new();

        public bool SpendMoney(int cost)
        {
            if (!CanAffordMoney(cost)) return false;
            int previousMoney = Value.Money;
            Value.Money -= cost;
            OnMoneyChanged.Invoke(previousMoney, Value.Money);
            return true;
        }
        public void AddMoney(int money)
        {
            int previousMoney = Value.Money;
            Value.Money += money;
            OnMoneyChanged.Invoke(previousMoney, Value.Money);
        }
        public void SetMoney(int money)
        {
            int previousMoney = Value.Money;
            Value.Money = money;
            OnMoneyChanged.Invoke(previousMoney, Value.Money);
        }
        public bool CanAffordMoney(int cost)
        {
            return Value.Money >= cost;
        }

        public bool SpendTools(int cost)
        {
            if (!CanAffordTools(cost)) return false;
            int previousTools = Value.Tools;
            Value.Tools -= cost;
            OnToolsChanged.Invoke(previousTools, Value.Tools);
            return true;
        }
        public void AddTools(int tools)
        {
            int previousTools = Value.Tools;
            Value.Tools += tools;
            OnToolsChanged.Invoke(previousTools, Value.Tools);
        }
        public void SetTools(int tools)
        {
            int previousTools = Value.Tools;
            Value.Tools = tools;
            OnToolsChanged.Invoke(previousTools, Value.Tools);
        }
        public bool CanAffordTools(int cost)
        {
            return Value.Tools >= cost;
        }
    }
}
