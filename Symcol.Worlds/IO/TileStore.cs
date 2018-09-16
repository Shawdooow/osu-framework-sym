using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using OpenTK.Graphics.ES30;

namespace Symcol.Worlds.IO
{
    public class TileStore : TextureStore
    {
        public TileStore(IResourceStore<RawTexture> store = null, bool useAtlas = true, All filteringMode = All.Nearest)
            : base(store, useAtlas, filteringMode)
        {
        }
    }
}
