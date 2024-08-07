using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Object_Fader
{
    public class FadeBlockingObjects : MonoBehaviour
    {
        public LayerMask layerMask;
        public Camera faderCamera;
        private Transform _target;

        [Range(0, 1)] public float fadedAlpha = 0.25f;
        public bool retainShadows = true;
        public Vector3 targetOffset = Vector3.up;

        public float fadeSpeed = 1;

        private readonly List<FadingObject> _blockingObjects = new List<FadingObject>();
        private readonly Dictionary<FadingObject, Coroutine> _runningCoroutines = new Dictionary<FadingObject, Coroutine>();
        private readonly RaycastHit[] _hits = new RaycastHit[10]; //increase number for busier scenes at cost of performance

        // Used for setting material properties
        private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
        private static readonly int Surface = Shader.PropertyToID("_Surface");

        private void Start()
        {
            _target = transform;
            StartCoroutine(CheckForObjects());
        }

        private IEnumerator CheckForObjects()
        {
            while (true)
            {
                Vector3 cameraPos = faderCamera.transform.position;
                Vector3 targetPos = _target.transform.position;

                // Raycast gets all objects between target and camera
                int hits = Physics.RaycastNonAlloc(
                    cameraPos,
                    (targetPos + targetOffset - cameraPos).normalized,
                    _hits,
                    Vector3.Distance(cameraPos, targetPos + targetOffset),
                    layerMask
                );

                for (int i = 0; i < hits; i++)
                {
                    FadingObject fadingObject = GetObjectFromHit(_hits[i]);

                    // If objects is known, check if it's blocking already
                    if(!fadingObject || _blockingObjects.Contains(fadingObject)) continue;
                    
                    //if object is not blocking but known (so currently fading in) stop the fade in
                    if(_runningCoroutines.ContainsKey(fadingObject))
                    {
                        if(_runningCoroutines[fadingObject] != null)
                            StopCoroutine(_runningCoroutines[fadingObject]);
                        
                        _runningCoroutines.Remove(fadingObject);
                    }
                    // Add object to fade in
                    _runningCoroutines.Add(fadingObject, StartCoroutine(FadeOutObject(fadingObject)));
                    _blockingObjects.Add(fadingObject);
                }

                FadeClearObjects();
                ClearHits();
                yield return null;
            }
        }

        /// <summary>
        /// Starts fading out any objects that are no longer blocking, and remove them from the blocking objects list.
        /// </summary>
        private void FadeClearObjects()
        {
            List<FadingObject> clearObjects = new List<FadingObject>(_blockingObjects.Count);

            // LINQ hell basically looping through all objects that are not hit
            foreach (FadingObject fadingObject in from fadingObject in _blockingObjects
                     let objectIsHit = _hits
                         .Select(GetObjectFromHit)
                         .Any(hitObject => hitObject && fadingObject == hitObject)
                     where !objectIsHit
                     select fadingObject)
            {
                if(_runningCoroutines.ContainsKey(fadingObject))
                {
                    if(_runningCoroutines[fadingObject] != null)
                        StopCoroutine(_runningCoroutines[fadingObject]);
                        
                    _runningCoroutines.Remove(fadingObject);
                }

                _runningCoroutines.Add(fadingObject, StartCoroutine(FadeInObject(fadingObject)));
                clearObjects.Add(fadingObject);
            }

            foreach (FadingObject clearObject in clearObjects)
                _blockingObjects.Remove(clearObject);
        }


        private IEnumerator FadeOutObject(FadingObject fadingObject)
        {
            // Set material properties for transparency
            foreach (Material material in fadingObject.materials)
            {
                material.SetInt(SrcBlend, (int)BlendMode.SrcAlpha);
                material.SetInt(DstBlend, (int)BlendMode.OneMinusSrcAlpha);
                material.SetInt(ZWrite, 0);
                material.SetInt(Surface, 1);

                material.renderQueue = (int)RenderQueue.Transparent;

                material.SetShaderPassEnabled("DepthOnly", false);
                material.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

                material.SetOverrideTag("RenderType", "Transparent");

                material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            // fade out materials smoothly
            float time = 0;

            while (fadingObject.materials[0].color.a > fadedAlpha)
            {
                foreach (Material mat in fadingObject.materials.Where(mat => mat.HasProperty("_Color")))
                {
                    mat.color = new Color(
                        mat.color.r,
                        mat.color.g,
                        mat.color.b,
                        Mathf.Lerp(fadingObject.initAlpha, fadedAlpha, time * fadeSpeed)
                    );
                }

                time += Time.deltaTime;
                yield return null;
            }

            if(!_runningCoroutines.ContainsKey(fadingObject)) yield break;
            StopCoroutine(_runningCoroutines[fadingObject]);
            _runningCoroutines.Remove(fadingObject);
        }

        private IEnumerator FadeInObject(FadingObject fadingObject)
        {
            // fade in materials smoothly
            float time = 0;

            while (fadingObject.materials[0].color.a < fadedAlpha)
            {
                foreach (Material mat in fadingObject.materials.Where(mat => mat.HasProperty("_Color")))
                {
                    mat.color = new Color(
                        mat.color.r,
                        mat.color.g,
                        mat.color.b,
                        Mathf.Lerp(fadedAlpha, fadingObject.initAlpha, time * fadeSpeed)
                    );
                }

                time += Time.deltaTime;
                yield return null;
            }

            // set material properties for opacity
            foreach (Material material in fadingObject.materials)
            {
                material.SetInt(SrcBlend, (int)BlendMode.One);
                material.SetInt(DstBlend, (int)BlendMode.Zero);
                material.SetInt(ZWrite, 0);
                material.SetInt(Surface, 1);

                material.renderQueue = (int)RenderQueue.Geometry;

                material.SetShaderPassEnabled("DepthOnly", true);
                material.SetShaderPassEnabled("SHADOWCASTER", true);

                material.SetOverrideTag("RenderType", "Opaque");

                material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            }

            if(!_runningCoroutines.ContainsKey(fadingObject)) yield break;
            StopCoroutine(_runningCoroutines[fadingObject]);
            _runningCoroutines.Remove(fadingObject);
        }


        private void ClearHits()
        {
            Array.Clear(_hits, 0, _hits.Length);
        }

        private static FadingObject GetObjectFromHit(RaycastHit hit)
        {
            return hit.collider ? hit.collider.GetComponent<FadingObject>() : null;
        }
    }
}