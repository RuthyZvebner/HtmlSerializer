using practicode2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace practicode2
{ 
    internal class HTMLserializer
    {
        public HtmlElement Serializer(string[] htmlLines)
        {
            HtmlElement root = new HtmlElement();
            HtmlElement currentElement = root;
            foreach (string line in htmlLines)
            {
                string firstWord = line.Split(' ').FirstOrDefault();
                if (firstWord == "/html")
                {
                    break;
                }
                if (firstWord.StartsWith("/"))
                {
                    currentElement = currentElement.Parent;
                }
                else
                {
                    if (HtmlHelper.Instance.AllTags.Contains(firstWord) || HtmlHelper.Instance.SelfClosingTags.Contains(firstWord))
                    {
                        HtmlElement newElement = new HtmlElement();
                        newElement.Name = firstWord;
                        newElement.Parent = currentElement;
                        currentElement.Children.Add(newElement);
                        ProcessAttributesAndClasses(newElement, line);
                        currentElement = newElement;
                    }
                    else
                        currentElement.InnerHtml = line;
                }
            }
            root = root.Children[0];
            return root;
        }
        private void ProcessAttributesAndClasses(HtmlElement element, string htmlElement)
        {
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(htmlElement);
            foreach (Match attributeMatch in attributes)
            {
                string attributeName = attributeMatch.Groups[1].Value;
                string attributeValue = attributeMatch.Groups[2].Value;
                if (attributeName.ToLower() == "class")
                {
                    string[] classes = attributeValue.Split(' ');
                    element.Classes.AddRange(classes);
                }
                else if (attributeName.ToLower() == "id")
                {
                    element.Id = attributeValue;
                }
            }
        }
    }
}