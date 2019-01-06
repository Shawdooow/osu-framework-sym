using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using Symcol.Base.Graphics.Containers;

namespace Symcol.Base.Touch
{
    public class TouchWheelContainer : TouchContainer
    {
        public TouchWheel Wheel;

        public TouchWheelContainer()
        {
            Name = "TouchWheelContainer";
            RelativeSizeAxes = Axes.Both;
            Width = 0.5f;

            AlwaysPresent = true;
            Alpha = 1;

            Child = Wheel = new TouchWheel();

            BorderThickness = 0;
            EdgeEffect = new EdgeEffectParameters();
        }

        protected override void Tap()
        {
            //base.Tap();
            Tapped = true;
            Wheel.Alpha = 1;
        }

        protected override void Release()
        {
            //base.Release();
            Tapped = false;
            Wheel.Alpha = 0;
        }

        protected override bool OnHover(HoverEvent e)
        {
            Hovered = true;
            return true;
            //return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            Hovered = false;
            if (Tapped)
                Release();
            //base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Pressed = true;
            if (!Tapped)
                Tap();
            return true;
            //return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            Pressed = false;
            if (Tapped)
                Release();
            return true;
            //return base.OnMouseUp(e);
        }

        public class TouchWheel : SymcolCircularContainer
        {
            public SymcolContainer<TouchContainer> Buttons = new SymcolContainer<TouchContainer>
            {
                RelativeSizeAxes = Axes.Both,
            };

            public TouchWheel()
            {
                Name = "TouchWheel";
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                Size = new Vector2(600);

                AlwaysPresent = true;
                Masking = true;
                Alpha = 0;

                Children = new Drawable[]
                {
                    Buttons,
                    new SymcolCircularContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Alpha = 0.5f,

                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(0.25f),

                        Masking = true,
                        BorderColour = Color4.Cyan,
                        BorderThickness = 6,

                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                        },
                    },
                };
            }
        }

        public class TouchWheelButton : TouchContainer
        {
            protected readonly SymcolCircularContainer Mask;

            public TouchWheelButton(Anchor anchor)
            {
                Name = "TouchWheelButton";
                Add(Mask = new SymcolCircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.25f),
                    Alpha = 0,
                });

                const float corner_offset = 75f;

                switch (anchor)
                {
                    default:
                        throw new NotSupportedException($"{anchor.ToString()} not supported!");
                    case Anchor.TopLeft:
                        Position = new Vector2(-corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomRight;
                        break;
                    case Anchor.TopCentre:
                        Position = new Vector2(0, -corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomCentre;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomCentre;
                        break;
                    case Anchor.TopRight:
                        Position = new Vector2(corner_offset, -corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomLeft;
                        break;
                    case Anchor.CentreLeft:
                        Position = new Vector2(-corner_offset, 0);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.CentreRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.CentreRight;
                        break;
                    case Anchor.CentreRight:
                        Position = new Vector2(corner_offset, 0);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.CentreLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.CentreLeft;
                        break;
                    case Anchor.BottomLeft:
                        Position = new Vector2(-corner_offset, corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopRight;
                        break;
                    case Anchor.BottomCentre:
                        Position = new Vector2(0, corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopCentre;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopCentre;
                        break;
                    case Anchor.BottomRight:
                        Position = new Vector2(corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopLeft;
                        break;
                }
            }
        }

        public class TouchWheelToggle : TouchToggle
        {
            protected readonly SymcolCircularContainer Mask;

            public TouchWheelToggle(Anchor anchor)
            {
                Name = "TouchWheelToggle";
                Add(Mask = new SymcolCircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Size = new Vector2(0.25f),
                    Alpha = 0,
                });

                const float corner_offset = 75f;

                switch (anchor)
                {
                    default:
                        throw new NotSupportedException($"{anchor.ToString()} not supported!");
                    case Anchor.TopLeft:
                        Position = new Vector2(-corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomRight;
                        break;
                    case Anchor.TopCentre:
                        Position = new Vector2(0, -corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomCentre;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomCentre;
                        break;
                    case Anchor.TopRight:
                        Position = new Vector2(corner_offset, -corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.BottomLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.BottomLeft;
                        break;
                    case Anchor.CentreLeft:
                        Position = new Vector2(-corner_offset, 0);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.CentreRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.CentreRight;
                        break;
                    case Anchor.CentreRight:
                        Position = new Vector2(corner_offset, 0);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.CentreLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.CentreLeft;
                        break;
                    case Anchor.BottomLeft:
                        Position = new Vector2(-corner_offset, corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopRight;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopRight;
                        break;
                    case Anchor.BottomCentre:
                        Position = new Vector2(0, corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopCentre;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopCentre;
                        break;
                    case Anchor.BottomRight:
                        Position = new Vector2(corner_offset);
                        Mask.Position = -Position;

                        Mask.Anchor = Anchor.TopLeft;
                        Anchor = Anchor.Centre;
                        Origin = Anchor.TopLeft;
                        break;
                }
            }
        }
    }
}
