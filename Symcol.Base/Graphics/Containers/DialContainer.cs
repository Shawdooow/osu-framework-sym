using System;
using osu.Framework.Graphics;
using osu.Framework.Input.Events;
using osuTK;

namespace Symcol.Base.Graphics.Containers
{
    public class DialContainer : SymcolCircularContainer
    {
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        private Vector2 mousePosition;

        private float lastAngle;
        private float currentRotation;
        public float RotationAbsolute;

        private int completeTick;

        // ReSharper disable once UnusedMember.Local
        private bool updateCompleteTick() => completeTick != (completeTick = (int)(RotationAbsolute / 360));

        private bool rotationTransferred;

        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            mousePosition = Parent.ToLocalSpace(e.CurrentState.Mouse.Position);
            return base.OnMouseMove(e);
        }

        protected override void Update()
        {
            base.Update();

            var thisAngle = -(float)MathHelper.RadiansToDegrees(Math.Atan2(mousePosition.X - DrawSize.X / 2, mousePosition.Y - DrawSize.Y / 2));


            if (!rotationTransferred)
            {
                currentRotation = Rotation * 2;
                rotationTransferred = true;
            }

            if (thisAngle - lastAngle > 180)
                lastAngle += 360;
            else if (lastAngle - thisAngle > 180)
                lastAngle -= 360;

            currentRotation += thisAngle - lastAngle;
            RotationAbsolute += Math.Abs(thisAngle - lastAngle);

            lastAngle = thisAngle;

            foreach(Drawable drawable in Children)
                drawable.RotateTo(currentRotation / 2, 200, Easing.OutExpo);
        }
    }
}
