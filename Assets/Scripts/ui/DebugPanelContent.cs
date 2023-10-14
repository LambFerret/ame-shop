using System.Collections.Generic;
using UnityEngine;

namespace ui
{
    public class DebugPanelContent : MonoBehaviour
    {
        public GameObject itemPrefab;
        public GameObject contentToAdd;

        public void AddItem(string title, string value)
        {
            var item = Instantiate(itemPrefab, contentToAdd.transform);
            item.GetComponent<DebugPanelItem>().Set(title, value);
        }

        public void SetTitle(string title, string description)
        {
            transform.Find("title").GetComponent<TMPro.TextMeshProUGUI>().text = title;
            transform.Find("description").GetComponent<TMPro.TextMeshProUGUI>().text = description;
        }

        public IEnumerable<float> GetFloatList()
        {
            var list = new List<float>();
            foreach (Transform child in contentToAdd.transform)
            {
                list.Add(float.Parse(child.GetComponent<DebugPanelItem>().Get()));
            }

            return list;
        }
    }
}