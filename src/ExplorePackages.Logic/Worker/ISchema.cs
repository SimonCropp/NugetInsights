﻿using System;
using Newtonsoft.Json.Linq;

namespace Knapcode.ExplorePackages.Logic.Worker
{
    public interface ISchema
    {
        string Name { get; }
        Type Type { get; }
        ISerializedEntity Serialize(object message);
        object Deserialize(int schemaVersion, JToken data);
    }
}