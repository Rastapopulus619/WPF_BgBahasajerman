using Microsoft.Web.WebView2.Wpf;
using System.Windows;

/*
public static class WebView2Behaviors
{
    public static readonly DependencyProperty HtmlContentProperty =
        DependencyProperty.RegisterAttached(
            "HtmlContent",
            typeof(string),
            typeof(WebView2Behaviors),
            new PropertyMetadata(null, OnHtmlContentChanged));

    public static string GetHtmlContent(WebView2 webView)
    {
        return (string)webView.GetValue(HtmlContentProperty);
    }

    public static void SetHtmlContent(WebView2 webView, string value)
    {
        webView.SetValue(HtmlContentProperty, value);
    }

    private static void OnHtmlContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is WebView2 webView && e.NewValue is string newContent)
        {
            webView.NavigateToString(newContent);
        }
    }
}
*/