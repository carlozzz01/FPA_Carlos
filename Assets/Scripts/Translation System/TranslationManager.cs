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

        public string CurrentLanguage { get; private set; }

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
            string language = PlayerPrefs.GetString("lang");

            if (string.IsNullOrEmpty(language)) language = Application.systemLanguage.ToString();

            TextAsset textAsset = Resources.Load<TextAsset>(language);

            if (textAsset == null)
            {
                language = _defaultLanguage;
                textAsset = Resources.Load<TextAsset>(_defaultLanguage);
            }

            CurrentLanguage = language;
            PlayerPrefs.SetString("lang", language);
            PlayerPrefs.Save();

            XmlDocument xml = new XmlDocument();

            xml.LoadXml(textAsset.text);

            LoadLanguage(xml);
        }

        /// <summary>
        /// Switches languages
        /// </summary>
        /// <param name="languageKey"></param>
        public void ChangeLanguage(string languageKey)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(languageKey);

            if (textAsset == null) return;

            CurrentLanguage = languageKey;
            PlayerPrefs.SetString("lang", languageKey);
            PlayerPrefs.Save();

            XmlDocument xml = new XmlDocument();

            xml.LoadXml(textAsset.text);

            LoadLanguage(xml);
        }


        /// <summary>
        /// Loads the language xml
        /// </summary>
        /// <param name="xml"></param>
        private void LoadLanguage(XmlDocument xml)
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

        /// <summary>
        /// Returns text with given key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            if (!_textDictionary.ContainsKey(key))
            {
                Debug.LogWarning($"Key {key} does not exits");

                return key;
            }

            return _textDictionary[key];
        }

        /// <summary>
        /// Calls for all TextTranslators to update
        /// </summary>
        public void InvokeOnTextLoaded()
        {
            OnTextLoaded?.Invoke();
        }
    }
}
