using System;
using System.Collections.Generic;
using System.Linq;

namespace practicode2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue();
                yield return currentElement;
                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindElementsBySelectorRecorsive(this, selector, result);
            return result;
        }
        
        public static IEnumerable<HtmlElement> FindElementsBySelectorRecorsive(HtmlElement element, Selector selector, HashSet<HtmlElement> s)
        {
            if (selector == null)
            {
                return null;
            }
            IEnumerable<HtmlElement> descendants = element.Descendants();
            foreach (HtmlElement d in descendants)
            {
                if (selector.TagName != null && selector.TagName != "")
                {
                    if (selector.TagName != d.Name)
                        continue;
                }
                if (selector.Id != null)
                {
                    if (selector.Id != d.Id)
                        continue;
                }
                if (selector.Classes != null)
                {
                    if (d.Classes != null)
                    {
                        if (!selector.Classes.All(c => d.Classes.Contains(c)))
                            continue;
                    }
                    else
                        continue;
                }
                if (selector.Child == null)
                {
                    s.Add(d);
                }
                else
                {
                    FindElementsBySelectorRecorsive(d, selector.Child, s);
                }
            }
            return s;
        }

    }
}