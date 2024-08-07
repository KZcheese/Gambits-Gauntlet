using UnityEngine;
using UnityEngine.Serialization;

namespace Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        public bool activated;
        public string message;
    }
}