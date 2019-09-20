using UnityEngine;
using System;
using System.Collections.Generic;

namespace Imoet.Unity.Animation
{
    [DisallowMultipleComponent]
    public sealed class ImoetTweenEngine : UnitySingleton<ImoetTweenEngine> {
        private Dictionary<Type, ITweener> m_dict;
    }
}