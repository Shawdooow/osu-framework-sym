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

        public abstract void DeSerialize(string worldSerial);

        public abstract void Serialize();

        protected Storage Storage { get; private set; }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Storage = storage;
        }

        public virtual string Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            fileName = fileName + FileExtension;

            try
            {
                using (Stream stream = Storage.GetStream(fileName, FileAccess.Read, FileMode.Open))
                using (StreamReader r = new StreamReader(stream))
                    return r.ReadLine();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to load world from file");
                return null;
            }
        }

        public virtual void Save(string fileName)
        {
            
        }
    }
}
