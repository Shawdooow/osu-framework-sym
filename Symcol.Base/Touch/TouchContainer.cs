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
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        protected readonly Box Box;

        public readonly SpriteText SpriteText;

        public Action OnTapped;

        public bool Pressed { get; set; }

        public double DeadZone { get; set; } = 200d;

        protected bool Hovered { get; private set; }

        public TouchContainer()
        {
            Clock = new DecoupleableInterpolatingFramedClock();

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
                    TextSize = 40,
                    Alpha = 0.75f,
                }).WithEffect(new GlowEffect
                {
                    Colour = Color4.Transparent,
                    PadExtent = true,
                }),
            };

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

        protected override void Update()
        {
            base.Update();

            if (Time.Current >= Dead)
            {
                Dead = Time.Current + DeadZone;
                Tapped();
            }
        }

        protected double Dead = double.MinValue;

        protected virtual void Tapped()
        {
            OnTapped?.Invoke();
        }

        protected override bool OnHover(HoverEvent e)
        {
            Hovered = true;
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Hovered = false;
            base.OnHoverLost(e);
        }
    }
}
