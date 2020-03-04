using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Imoet.Unity.Events;

namespace Imoet.Unity.U2D {
    [SerializeField]
    internal class _SpriteMethod : ImoetComponentMethod<Sprite> { }

    [SerializeField]
    public class ImoetSpriteAnimationPack
    {
        [SerializeField]
        private _SpriteMethod m_target;
        [SerializeField]
        private List<ImoetSpriteAnimationData> m_data;
    }
}
