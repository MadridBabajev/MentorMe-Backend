namespace App.BLL.Contracts;

public interface IMLService<in TInput>
{
    Task LoadModelAsync();
    Task<IEnumerable<string>> RunInferenceAsync(TInput inferenceInput);
}