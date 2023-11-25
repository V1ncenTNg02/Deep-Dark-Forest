using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ORZ.UI
{
    public class MessageController : MonoBehaviour
    {
        public TextMeshProUGUI tmp;
        public float duration = 2.0f;

        /// <summary>
        /// Activate Global Message and set the message
        /// </summary>
        /// <param itemName="msg"></param>
        public void UpdateGM(String msg)
        {
            tmp.gameObject.SetActive(true);
            tmp.text = msg;
            StopAllCoroutines();
            StartCoroutine(DeactivateGM());
        }

        public IEnumerator DeactivateGM()
        {
            yield return new WaitForSecondsRealtime(duration);
            tmp.gameObject.SetActive(false);
        }
    }

}
