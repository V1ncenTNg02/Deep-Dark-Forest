using System.Collections.Generic;
using UnityEngine;

namespace ORZ
{
    public class PreInitializer : MonoBehaviour
    {
        public List<GameObject> activateGameObjects;
        public List<GameObject> deactivateGameObjects;

        void Start()
        {
            ActivateObjects();
            ObjectGetter.Initialize();
            DeActivateObjects();
        }

        void ActivateObjects()
        {
            foreach (var objects in activateGameObjects)
            {
                if (!objects.activeSelf)
                {
                    objects.SetActive(true);
                }
            }
        }

        void DeActivateObjects()
        {
            foreach (var objects in deactivateGameObjects)
            {
                if (objects.activeSelf)
                {
                    objects.SetActive(false);
                }
            }
        }
    }

}
