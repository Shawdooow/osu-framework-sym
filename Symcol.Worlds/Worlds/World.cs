using System;
using System.IO;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Framework.Platform;
using Symcol.Base.Graphics.Containers;

namespace Symcol.Worlds.Worlds
{
    public abstract class World : SymcolContainer
    {
        public virtual string FileExtension => ".world";

        /// <summary>
        /// Converts worldSerial to the world
        /// </summary>
        /// <param name="worldSerial"></param>
        public abstract void DeSerialize(string worldSerial);

        /// <summary>
        /// Serializes the current world
        /// </summary>
        /// <returns></returns>
        public abstract string Serialize();

        protected Storage Storage { get; private set; }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Storage = storage;
        }

        /// <summary>
        /// Gets a world serial from specifiecd file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public virtual string GetSerialFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;

            try
            {
                using (Stream stream = Storage.GetStream(fileName + FileExtension, FileAccess.Read, FileMode.Open))
                using (StreamReader r = new StreamReader(stream))
                    return r.ReadToEnd();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to load world from file");
                return null;
            }
        }

        /// <summary>
        /// Saves a Serialized world to file
        /// </summary>
        /// <param name="fileName"></param>
        public virtual void SaveToFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            try
            {
                using (Stream stream = Storage.GetStream(fileName + FileExtension, FileAccess.Write, FileMode.Create))
                using (StreamWriter w = new StreamWriter(stream))
                    w.Write(Serialize());
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to save world to file");
                return;
            }
        }
    }
}
