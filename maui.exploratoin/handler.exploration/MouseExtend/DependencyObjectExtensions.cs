
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Page = Microsoft.UI.Xaml.Controls.Page;

namespace handler.exploration.Interfaces
{
    public static class DependencyObjectExtensions
    {
        //
        // 摘要:
        //     Find any child of type T and Name childname. Searches down the tree first and
        //     then the siblings.
        //
        // 参数:
        //   parent:
        //
        //   childName:
        //
        // 类型参数:
        //   T:
        public static T FindChild<T>(this DependencyObject parent, string childName = null) where T : DependencyObject
        {
            if (parent == null)
            {
                return null;
            }

            T val = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if ((DependencyObject)(child as T) == (DependencyObject)null)
                {
                    val = child.FindChild<T>(childName);
                    if ((DependencyObject)val != (DependencyObject)null)
                    {
                        break;
                    }

                    continue;
                }

                if (!string.IsNullOrEmpty(childName))
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        val = (T)child;
                        break;
                    }

                    continue;
                }

                val = (T)child;
                break;
            }

            return val;
        }

        //
        // 摘要:
        //     Finds all children of type T.
        //
        // 参数:
        //   parent:
        //
        //   depth:
        //
        // 类型参数:
        //   T:
        public static IEnumerable<T> FindAllChildren<T>(this DependencyObject parent, int depth = 0) where T : DependencyObject
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                T val = child as T;
                if ((DependencyObject)val != (DependencyObject)null)
                {
                    yield return val;
                    continue;
                }

                foreach (T item in child.FindAllChildren<T>(depth + 1))
                {
                    yield return item;
                }
            }
        }

        //
        // 摘要:
        //     Returns the top most Microsoft.UI.Xaml.Controls.Page or root Microsoft.UI.Xaml.FrameworkElement.
        //
        //
        // 参数:
        //   view:
        public static FrameworkElement GetRoot(this FrameworkElement view)
        {
            Page page = null;
            FrameworkElement frameworkElement = view;
            while (frameworkElement != null)
            {
                view = frameworkElement;
                if (view is Page page2)
                {
                    page = page2;
                }

                frameworkElement = VisualTreeHelper.GetParent(frameworkElement) as FrameworkElement;
            }

            return page ?? view;
        }
    }
}
