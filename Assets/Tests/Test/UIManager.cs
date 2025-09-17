using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test
{
    public class UIManager : MonoBehaviour
    {
        public List<Button> map;
        public List<Button> battle;

        public void OnMap()
        {
            CloseAll();
            foreach (var item in map)
            {
                item.interactable = true;
            }
        }
        public void OnBattle()
        {
            CloseAll();
            foreach (var item in battle)
            {
                item.interactable = true;
            }

        }
        public void CloseAll()
        {
            foreach (var item in map)
            {
                item.interactable = false;
            }
            foreach (var item in battle)
            {  item.interactable = false; 
            }
        }
    }
}