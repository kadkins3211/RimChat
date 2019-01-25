using UnityEngine;

namespace RimChat.MainTab
{
    public abstract class ChatTab
    {
        public ChatTab()
        {

        }

        public virtual string Label()
        {
            return GetType().ToString();
        }

        public virtual bool Enabled => true;

        public virtual string DisabledReason => "";

        public abstract void DoWindowContents(Rect canvas);

        public virtual void PostClose()
        {
        }

        public virtual void PostOpen()
        {
        }

        public virtual void PreClose()
        {
        }

        public virtual void PreOpen()
        {
        }

        public virtual void Tick()
        {
        }
    }
}
