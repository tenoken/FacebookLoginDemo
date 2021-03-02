using HtmlAgilityPack;
using LoginDemo.Domain.Model;
using LoginDemo.Domain.Models.Interfaces;
using LoginDemo.Servcices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoginDemo.Servcices
{
    public class DocumentHelper : IDocumentHelper
    {
        private HtmlDocument _htmlDocument;
        private const string LOGIN_FORM = "//form[1]";
        private const string DO_NOT_SAVE_FORM = "//form[2]";
        private Dictionary<string, string> _dictionary;

        public DocumentHelper()
        {
            _htmlDocument = new HtmlDocument();
            _dictionary = new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetParams(string pageContent, ActionParams action, Credential credential)
        {
            //TODO: create a specific document helper for all actions
            switch (action)
            {
                case ActionParams.Default:
                    return _dictionary;
                case ActionParams.Login:
                    _dictionary = GetLoginParams(pageContent, credential);
                    break;
                case ActionParams.Logout:
                    _dictionary = GetLogoutParams(pageContent);
                    break;
                default:
                    throw new NotImplementedException($"There is no a specialized parameters to referenced action: {action}");
            }

            return _dictionary;
        }

        private Dictionary<string, string> GetLoginParams(string pageContent, Credential credential)
        {
            _htmlDocument.LoadHtml(pageContent);

            var formNode = _htmlDocument.DocumentNode.SelectNodes(LOGIN_FORM).First();

            var inputsNodes = formNode.ChildNodes.Where(x => x.Name.Equals("input")).ToList();
            var attributes = inputsNodes.Select((x => x.Attributes.Select(o => o.Value))).ToList();

            var names = attributes.Select(x => x.ElementAt(1)).ToList();

            if (names.Count == 7)
            {
                attributes.RemoveAt(6);
                names = attributes.Select(x => x.ElementAt(1)).ToList();
            }

            var values = attributes.Select(x => x.ElementAt(2)).ToList();

            names.Add("login");
            values.Add("Entrar");

            SafeAdd(names, values);
            AddLoginDataParameter(credential);
            return _dictionary;
        }

        private Dictionary<string, string> GetLogoutParams(string pageContent)
        {
            _htmlDocument.LoadHtml(pageContent);

            var inputs = _htmlDocument.DocumentNode.SelectNodes(DO_NOT_SAVE_FORM).First().ChildNodes;

            var attributes = inputs.Select((x => x.Attributes.Select(o => o.Value))).ToList();

            var names = attributes.Select(x => x.ElementAt(1)).ToList();
            var values = attributes.Select(x => x.ElementAt(2)).ToList();

            names.RemoveAt(2);
            values.RemoveAt(2);

            SafeAdd(names, values);
            return _dictionary;
        }

        private void AddLoginDataParameter(Credential credential)
        {
            SafeAdd("email", credential.GetUser());
            SafeAdd("pass", credential.GetPassword());
        }

        private void SafeAdd(IList<string> keys, IList<string> values)
        {

            for (int i = 0; i < keys.Count; i++)
            {
                if (_dictionary.Where(x => x.Key == keys[i]).Count() == 0)
                    _dictionary.Add(keys[i],values[i]);
            }            
        }

        private void SafeAdd(string key, string value)
        {
            if (_dictionary.Where(x => x.Key == key).Count() == 0)
                _dictionary.Add(key, value);
        }
    }
}