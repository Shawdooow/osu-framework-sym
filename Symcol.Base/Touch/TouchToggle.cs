using osu.Framework.Graphics;

namespace Symcol.Base.Touch
{
    public class TouchToggle : TouchContainer
    {
        public bool Toggled { get; protected set; }

        protected override void Tapped()
        {
            Toggled = !Toggled;

            if (Toggled)
                Box.FadeTo(0.2f, 200);
            else
                Box.FadeTo(0, 200);
        }
    }
}
