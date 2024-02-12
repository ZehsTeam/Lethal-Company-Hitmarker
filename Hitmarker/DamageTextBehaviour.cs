using TMPro;
using UnityEngine;

namespace com.github.zehsteam.Hitmarker;

internal class DamageTextBehaviour : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetText(string value)
    {
        gameObject.GetComponent<TMP_Text>().text = value;
    }
}
