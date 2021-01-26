using HtmlAgilityPack;
using LoginDemo.Servcices.Interfaces;
using System;
using System.Linq;

namespace LoginDemo.Servcices
{
    public class DocumentHelper : IDocumentHelper
    {
        private HtmlDocument _htmlDocument;
        private const string LOGIN_FORM = "/html[1]/body[1]/div[1]/div[1]/div[2]/div[1]/table[1]/tbody[1]/tr[1]/td[1]/div[2]/div[2]/form[1]";
        private const string DO_NOT_SAVE_FORM = "//form[2]";

        public DocumentHelper()
        {
            _htmlDocument = new HtmlDocument();
        }

        public Tuple<string[], string[]> GetParams(string pageContent, FacebookWebRequest.ActionParams action)
        {
            var parameters = new Tuple<string[], string[]>(new string[0], new string[0]);

            switch (action)
            {
                case FacebookWebRequest.ActionParams.Default:
                    //Do nothing
                    break;
                case FacebookWebRequest.ActionParams.Login:
                    parameters = GetLoginParams(pageContent);
                    break;
                case FacebookWebRequest.ActionParams.Logout:
                    parameters = GetLogoutParams(pageContent);
                    break;
                default:
                    //Do nothing
                    break;
            }

            return parameters;
        }

        private Tuple<string[], string[]> GetLoginParams(string pageContent)
        {
            //Hidden Form Iputs
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

            return new Tuple<string[], string[]>(names.ToArray(),values.ToArray());
        }

        private Tuple<string[], string[]> GetLogoutParams(string pageContent)
        {
            _htmlDocument.LoadHtml(pageContent);

            var inputs = _htmlDocument.DocumentNode.SelectNodes(DO_NOT_SAVE_FORM).First().ChildNodes;

            var attributes = inputs.Select((x => x.Attributes.Select(o => o.Value))).ToList();

            var names = attributes.Select(x => x.ElementAt(1)).ToList();
            var values = attributes.Select(x => x.ElementAt(2)).ToList();

            names.RemoveAt(2);
            values.RemoveAt(2);

            return new Tuple<string[], string[]>(names.ToArray(), values.ToArray());
        }
    }
}