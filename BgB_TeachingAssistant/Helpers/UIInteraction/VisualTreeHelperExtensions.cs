using System;
using System.Windows;
using System.Windows.Media;

namespace BgB_TeachingAssistant.Helpers.UIInteraction
{
    public static class VisualTreeHelperExtensions
    {
        /// <summary>
        /// Finds a child of the specified type in the visual tree of the given parent.
        /// </summary>
        /// <typeparam name="T">The type of child to find.</typeparam>
        /// <param name="parent">The parent element to search from.</param>
        /// <param name="condition">An optional condition to match the child.</param>
        /// <returns>The first child that matches the specified type and condition, or null if no match is found.</returns>
        public static T FindVisualChild<T>(DependencyObject parent, Func<T, bool> condition = null) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild && (condition == null || condition(tChild)))
                {
                    return tChild;
                }

                var result = FindVisualChild<T>(child, condition);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
