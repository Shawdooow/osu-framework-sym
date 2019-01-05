using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osuTK;
using osuTK.Graphics;
using Symcol.Base.Graphics.Containers;

namespace Symcol.Base.Touch
{
    /// <summary>
    /// This class is a hack, don't take it seriously (but it should work on ios so...)
    /// </summary>
    public class TouchContainer : SymcolContainer
    {
        //public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        protected readonly Box Box;

        public readonly SpriteText SpriteText;

        public Action OnTap;

        public Action OnRelease;

        public bool Tapped { get; set; }

        protected bool Hovered { get; private set; }

        protected static bool Pressed { get; private set; }

        public TouchContainer()
        {
            Clock = new DecoupleableInterpolatingFramedClock();

            Size = new Vector2(100);

            Children = new Drawable[]
            {
                Box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    Colour = Color4.White,
                },
                (SpriteText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = "Venera",
                    TextSize = 32,
                    Alpha = 0.75f,
                    Text = "Text",
                }).WithEffect(new GlowEffect
                {
                    Colour = Color4.Transparent,
                    PadExtent = true,
                }),
            };

            Masking = true;
            Masking = true;
            BorderColour = Color4.White;
            BorderThickness = 6;
            EdgeEffect = new EdgeEffectParameters
            {
                Hollow = true,
                Colour = Color4.Cyan,
                Type = EdgeEffectType.Shadow,
                Radius = 8,
            };
        }

        protected virtual void Tap()
        {
            Tapped = true;
            OnTap?.Invoke();
        }

        protected virtual void Release()
        {
            Tapped = false;
            OnRelease?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            Hovered = true;
            if (Pressed && !Tapped)
                Tap();
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Hovered = false;
            if (Tapped)
                Release();
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Pressed = true;
            if (Hovered && !Tapped)
                Tap();
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            Pressed = false;
            if (Hovered && Tapped)
                Release();
            return base.OnMouseUp(e);
        }
    }
}
