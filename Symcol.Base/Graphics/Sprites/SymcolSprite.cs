using osu.Framework.Graphics.Sprites;

namespace Symcol.Base.Graphics.Sprites
{
    public class SymcolSprite : Sprite
    {
        public override bool HandleNonPositionalInput => false;
        public override bool HandlePositionalInput => false;

        public virtual void Delete()
        {
            ClearTransforms();
            Dispose();
        }

        public override bool UpdateSubTree()
        {
            if (IsDisposed) return false;
            try
            {
                return base.UpdateSubTree();
            }
            catch
            {
                return false;
            }
        }
    }
}
