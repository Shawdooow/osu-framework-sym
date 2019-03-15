using osu.Framework.Graphics.Sprites;

namespace Sym.Base.Graphics.Sprites
{
    public class SymcolSprite : Sprite
    {
        public override bool HandleNonPositionalInput => false;
        public override bool HandlePositionalInput => false;

        public virtual void Delete()
        {
            ClearTransforms();
            Expire();
        }
    }
}
