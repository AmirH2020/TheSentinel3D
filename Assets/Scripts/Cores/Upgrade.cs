using System;
using UnityEngine.Events;

namespace TheSentinel.Cores
{
    public enum UpgradeColor
    {
        Red,
        Green,
        Blue,
        Purple
    }

    public class Upgrade
    {

        public string title { get; private set; }

        public Action action { get; private set; }

        public UnityAction SubAction { get; set; }

        public UpgradeColor color { get; private set; }

        public int level { get; private set; }

        public int index { get; private set; }

        public bool unlockCondition { get; private set; }

        public void setUnlockCondition(bool unlockCondition) => this.unlockCondition = unlockCondition;

        public Upgrade(string title, Action action,UpgradeColor color,bool unlockCondition,int level,int index)
        {
            this.title = title;
            this.action = action;
            this.color = color;
            this.unlockCondition = unlockCondition;
            this.level = level;
            this.index = index;
        }


    }

}
