using osu.Framework.Graphics;
using Symcol.Base.Graphics.Containers;

namespace Symcol.Base.Touch
{
    public class TouchWheelContainer : TouchContainer
    {
        public TouchWheel Wheel;

        public TouchWheelContainer()
        {
            RelativeSizeAxes = Axes.Both;
            Width = 0.5f;

            AlwaysPresent = true;
            //Alpha = 0;

            Child = Wheel = new TouchWheel
            {

            };
        }

        protected override void Tap()
        {
            base.Tap();
        }

        protected override void Release()
        {
            base.Release();
        }

        public class TouchWheel : SymcolCircularContainer
        {

        }
    }
}
