using osu.Framework.Input.Events;
using osuTK;
using osuTK.Input;

namespace Symcol.Base.Graphics.Containers
{
    public class DragContainer : SymcolContainer
    {
        protected override bool OnDragStart(DragStartEvent e) => true;

        public bool AllowLeftClickDrag { get; set; } = true;

        private bool leftDown;
        private bool rightDown;

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            if (e.Button == MouseButton.Left && AllowLeftClickDrag)
                return leftDown = true;

            if (e.Button == MouseButton.Right)
                return rightDown = true;

            return base.OnMouseDown(e);
        }

        protected override bool OnDrag(DragEvent e)
        {
            if (leftDown || rightDown)
                Position += e.Delta;

            return base.OnDrag(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            if (e.Button == MouseButton.Left && AllowLeftClickDrag)
            {
                leftDown = false;
                return true;
            }

            if (e.Button == MouseButton.Right)
            {
                rightDown = false;
                return true;
            }

            return base.OnMouseUp(e);
        }
    }
}
