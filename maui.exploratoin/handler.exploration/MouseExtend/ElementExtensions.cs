using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    public static class ElementExtensions
    {
        //
        // 摘要:
        //     Finds the first parent Element of type T.
        //
        // 参数:
        //   element:
        //
        // 类型参数:
        //   T:
        //     The type of the parent element you want to find.
        public static T FindParent<T>(this Element element) where T : Element
        {
            element = element.Parent;
            while (element != null && !(element is T))
            {
                element = element.Parent;
            }

            return element as T;
        }

        //
        // 摘要:
        //     Returns whether the given parent is a parent of this Element.
        //
        // 参数:
        //   element:
        //
        //   parent:
        public static bool HasParent(this Element element, Element parent)
        {
            element = element.Parent;
            while (element != null && element != parent)
            {
                element = element.Parent;
            }

            return element == parent;
        }

        //
        // 摘要:
        //     Returns a printable hierarchy path for this element.
        //
        // 参数:
        //   element:
        public static string TreeHierarchy(this Element element)
        {
            string text = element.GetType().Name;
            if (element.Parent != null)
            {
                text = element.Parent.TreeHierarchy() + " / " + text;
            }

            return text;
        }

        public static T FindParent<T>(this IGestureAwareControl element) where T : Element
        {
            return ((Element)element).FindParent<T>();
        }

        public static bool HasParent(this IGestureAwareControl element, Element parent)
        {
            return ((Element)element).HasParent(parent);
        }
    }
}
