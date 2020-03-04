using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Imoet.Unity.U2D {
    [SerializeField]
    public class ImoetSpriteAnimationData
    {
        public string name;
        public float speed;
        public List<Sprite> sprites;

        public ImoetSpriteAnimationData() {
            sprites = new List<Sprite>();
        }
    }
}

