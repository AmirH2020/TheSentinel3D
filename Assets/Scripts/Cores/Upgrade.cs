using System;

namespace TheSentinel.Cores
{
    public class Upgrade
    {
        public string title;
        public Action action { get; set; }
  
        public Upgrade (string title, Action action)
        {
            this.title = title;
            this.action = action;
        }
    }
}