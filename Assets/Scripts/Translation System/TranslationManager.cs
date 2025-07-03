using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Managers
{
    public class TranslationManager : MonoBehaviour
    {
        [SerializeField] private string _defaultLanguage = "spanish";
        private Dictionary<string, string> _textDictionary;
    
        private static TranslationManager _instance;
        public static TranslationManager Instance => _instance;

        public static Action OnTextLoaded;
    
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    
        private void Start()
        {
            string systemLanguage = Application.systemLanguage.ToString();
    
            TextAsset textAsset = Resources.Load<TextAsset>(systemLanguage);
    
            if (textAsset == null)
            {
                textAsset = Resources.Load<TextAsset>(_defaultLanguage);
            }
    
            XmlDocument xml = new XmlDocument();
    
            xml.LoadXml(textAsset.text);
    
            LoadText(xml);
        }

        private void LoadText(XmlDocument xml)
        {
            _textDictionary = new Dictionary<string, string>();

            XmlElement element = xml.DocumentElement["lang"];

            IEnumerator elementEnum = element.GetEnumerator();

            while (elementEnum.MoveNext())
            {
                XmlElement xmlItem = (XmlElement)elementEnum.Current;

                string text = xmlItem.InnerText.Replace('[', '<').Replace(']', '>');

                _textDictionary.Add(xmlItem.GetAttribute("key"), text);
            }

            OnTextLoaded?.Invoke();
        }
    
        public string GetText(string key)
        {
            if (!_textDictionary.ContainsKey(key))
            {
                Debug.LogWarning($"Key {key} does not exits");
    
                return key;
            }
    
            return _textDictionary[key];
        }
    }
}
