using osu.Framework.Graphics;
using OpenTK;

namespace Symcol.Base.Graphics.Containers
{
    public class AspectLockedContainer : AspectLockedContainer<Drawable>
    {
    }

    public class AspectLockedContainer<T> : SymcolContainer<T>
        where T : Drawable
    {
        public new float Margin = 1;

        protected virtual Vector2 AspectRatio => Vector2.One;

        public AspectLockedContainer()
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Scale = new Vector2(Parent.DrawSize.Y * AspectRatio.X / AspectRatio.Y / Size.X, Parent.DrawSize.Y / Size.Y) * Margin;
        }
    }
}
