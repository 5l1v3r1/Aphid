using Components.Aphid;
using System;
using Microsoft.Pex.Framework;

namespace Components.Aphid
{
    /// <summary>A factory for Components.Caching.FileCacheInfo instances</summary>
    public static partial class FileCacheInfoFactory
    {
        /// <summary>A factory for Components.Caching.FileCacheInfo instances</summary>
        [PexFactoryMethod(typeof(FileCacheInfo))]
        public static FileCacheInfo Create(FileCacheSource[] sources_fileCacheSources) =>
            new FileCacheInfo(sources_fileCacheSources);
    }
}
