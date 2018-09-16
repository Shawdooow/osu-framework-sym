using Newtonsoft.Json;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using OpenTK;
using Symcol.Worlds.Tiles;

namespace Symcol.Worlds.Chunks
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Chunk : Container
    {
        public const int TILES_PER_CHUNK = 16;
        public const int TILE_SIZE = 64;
        public const int CHUNK_SIZE = TILES_PER_CHUNK * TILE_SIZE;

        [JsonProperty("tiles")]
        public int?[,] Tiles
        {
            get => TileContainer.Tiles;
            set => TileContainer.Tiles = value;
        }

        public TileAtlas Atlas
        {
            get => TileContainer.Atlas;
            set => TileContainer.Atlas = value;
        }

        public readonly DrawableTiles TileContainer;

        public Chunk()
        {
            Size = new Vector2(CHUNK_SIZE);
            Add(TileContainer = new DrawableTiles
            {
                RelativeSizeAxes = Axes.Both
            });
        }
    }
}
