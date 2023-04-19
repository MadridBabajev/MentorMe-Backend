using AngleSharp.Html.Dom;

namespace ProjectTests.Helpers;
public static class HttpClientExtensions
{
    private static IHtmlFormElement SetFormValues(IHtmlFormElement form,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        foreach (var (key, value) in formValues)
        {
            switch (form[key])
            {
                case IHtmlInputElement:
                {
                    (form[key] as IHtmlInputElement)!.Value = value;
                    if ((form[key] as IHtmlInputElement)!.Type == "checkbox" && bool.Parse(value))
                    {
                        (form[key] as IHtmlInputElement)!.IsChecked = true;
                    }

                    break;
                }
                case IHtmlSelectElement:
                {
                    (form[key] as IHtmlSelectElement)!.Value = value;
                    break;
                }
            }
        }

        return form;
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IHtmlElement submitButton)
    {
        return client.SendAsync(form, submitButton, new Dictionary<string, string>());
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client,
        IHtmlFormElement form,
        IEnumerable<KeyValuePair<string, string>> formValues)
    {
        var submitElement = Assert.Single(form.QuerySelectorAll("[type=submit]"));
        var submitButton = Assert.IsAssignableFrom<IHtmlElement>(submitElement);

        return client.SendAsync(form, submitButton, formValues);
    }

    public static Task<HttpResponseMessage> SendAsync(
        this HttpClient client, IHtmlFormElement form,
        IHtmlElement submitButton, IEnumerable<KeyValuePair<string, string>> formValues)
    {
        form = SetFormValues(form, formValues);

        var submit = form.GetSubmission(submitButton);
        var target = (Uri) submit!.Target;
        if (submitButton.HasAttribute("formaction"))
        {
            var formAction = submitButton.GetAttribute("formaction")!;
            target = new Uri(formAction, UriKind.Relative);
        }

        var submission = new HttpRequestMessage(new HttpMethod(submit.Method.ToString()), target)
        {
            Content = new StreamContent(submit.Body)
        };

        foreach (var (key, value) in submit.Headers)
        {
            submission.Headers.TryAddWithoutValidation(key, value);
            submission.Content.Headers.TryAddWithoutValidation(key, value);
        }

        return client.SendAsync(submission);
    }
}