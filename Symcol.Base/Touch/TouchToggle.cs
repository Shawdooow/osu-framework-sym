﻿using osu.Framework.Graphics;
using osu.Framework.Input.Events;

namespace Symcol.Base.Touch
{
    public class TouchToggle : TouchContainer
    {
        protected override void Tap()
        {
            base.Tap();
            Box.FadeTo(0.2f, 200);
        }

        protected override void Release()
        {
            base.Release();
            Box.FadeTo(0, 200);
        }

        protected override bool OnHover(HoverEvent e)
        {
            Hovered = true;

            if (Pressed && !Tapped)
            {
                Tap();
                return true;
            }

            if (Pressed && Tapped)
            {
                Release();
                return true;
            }

            return false;
            //return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Hovered = false;
            //base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Pressed = true;

            if (Hovered && !Tapped)
            {
                Tap();
                return true;
            }

            if (Hovered && Tapped)
            {
                Release();
                return true;
            }

            return false;
            //return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            Pressed = false;
            return true;
            //return base.OnMouseUp(e);
        }
    }
}