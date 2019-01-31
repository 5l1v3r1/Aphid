using Components.Aphid;
// <copyright file="FileCacheInfoSerializerFactory.cs">Copyright © AutoSec Tools LLC 2018</copyright>

using System;
using Microsoft.Pex.Framework;

namespace Components.Aphid
{
    /// <summary>A factory for Components.Caching.FileCacheInfoSerializer instances</summary>
    public static partial class FileCacheInfoSerializerFactory
    {
        /// <summary>A factory for Components.Caching.FileCacheInfoSerializer instances</summary>
        [PexFactoryMethod(typeof(FileCacheInfoSerializer))]
        public static FileCacheInfoSerializer Create(Version dependencyVersion_version) =>
            new FileCacheInfoSerializer(dependencyVersion_version);
    }
}
