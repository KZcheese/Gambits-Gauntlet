using System.Collections.Generic;
using UnityEngine;

namespace Interactables
{
    public class ButtonController : MonoBehaviour
    {
        public List<Interactable> interactables;
        public ButtonMode mode;
        public string message;
        private Material _mat;

        public enum ButtonMode
        {
            Hold,
            Toggle,
        }

        protected virtual void Start()
        {
            _mat = GetComponent<Renderer>().material;
            ToggleEmission(false);

            foreach (Interactable interactable in interactables)
            {
                interactable.activated = false;
                interactable.message = "";
            }
        }

        public void SetActivate(bool active)
        {
            foreach (Interactable interactable in interactables)
            {
                interactable.activated = active;
                // interactable.message = interactable.activated ? message : "";
                interactable.message = message;
            }

            ToggleEmission(active);
        }

        public void Toggle()
        {
            foreach (Interactable interactable in interactables)
            {
                interactable.activated = !interactable.activated;
                interactable.message = interactable.activated ? message : "";
            }

            ToggleEmission(interactables[0].activated);
        }

        private void ToggleEmission(bool active)
        {
            if(!_mat) return;
            if(active) _mat.EnableKeyword("_EMISSION");
            else _mat.DisableKeyword("_EMISSION");
        }
    }
}