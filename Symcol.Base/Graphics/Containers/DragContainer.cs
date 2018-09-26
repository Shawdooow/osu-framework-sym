using osu.Framework.Input.EventArgs;
using osu.Framework.Input.States;
using OpenTK;
using OpenTK.Input;

namespace Symcol.Base.Graphics.Containers
{
    public class DragContainer : SymcolContainer
    {
        protected override bool OnDragStart(InputState state) => true;

        public bool AllowLeftClickDrag { get; set; } = true;

        private bool leftDown;
        private bool rightDown;

        private Vector2 startPosition;

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            startPosition = Position;

            if (args.Button == MouseButton.Left && AllowLeftClickDrag)
                return leftDown = true;

            if (args.Button == MouseButton.Right)
                return rightDown = true;

            return base.OnMouseDown(state, args);
        }

        protected override bool OnDrag(InputState state)
        {
            if (leftDown | rightDown)
                Position = startPosition + state.Mouse.Position - state.Mouse.PositionMouseDown.GetValueOrDefault();

            return base.OnDrag(state);
        }

        protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
        {
            if (args.Button == MouseButton.Left && AllowLeftClickDrag)
            {
                leftDown = false;
                return true;
            }

            if (args.Button == MouseButton.Right)
            {
                rightDown = false;
                return true;
            }

            return base.OnMouseUp(state, args);
        }
    }
}
