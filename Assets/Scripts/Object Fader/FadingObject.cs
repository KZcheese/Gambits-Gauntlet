using System;
using System.Collections.Generic;
using UnityEngine;

namespace Object_Fader
{
    public class FadingObject : MonoBehaviour, IEquatable<FadingObject>
    {
        private Vector3 _position;
        
        public readonly List<Material> materials = new List<Material>();

        [HideInInspector] public float initAlpha;

        private void Awake()
        {
            _position = transform.position;

            var renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                materials.AddRange(r.materials);

            initAlpha = materials[0].color.a; //this is way less complicated than tracking all the materials;
        }

        public bool Equals(FadingObject o)
        {
            return o != null && _position.Equals(o._position);
        }

        public override int GetHashCode()
        {
            return _position.GetHashCode();
        }
    }
}