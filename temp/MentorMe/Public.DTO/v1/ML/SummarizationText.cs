namespace Public.DTO.v1.ML;

/// <summary>
/// A DTO that includes the text to be summarized
/// </summary>
public class SummarizationText
{
    /// <summary>
    /// The text for to run summarization inference on.
    /// </summary>
    public string text { get; set; }
}