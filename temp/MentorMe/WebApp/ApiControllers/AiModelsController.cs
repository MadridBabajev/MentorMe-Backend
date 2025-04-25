using System.Net.Mime;
using App.BLL.Contracts;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Public.DTO.v1.ML;

namespace WebApp.ApiControllers;

/// <summary>
/// A controller responsible for running OCR and Summarization inferences via HTTP calls
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AiModelsController : ControllerBase
{
    private readonly IAppBLL _bll;
    private readonly ILogger<AiModelsController> _logger;

    /// <summary>
    /// ML Controller's constructor
    /// </summary>
    /// <param name="bll"></param>
    /// <param name="logger"></param>
    public AiModelsController(IAppBLL bll, ILogger<AiModelsController> logger)
    {
        _bll = bll;
        _logger = logger;
    }

    /// <summary>
    /// Extract handwritten text lines from the uploaded image.
    /// </summary>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Ocr([FromForm] IFormFile? file)
    {
        try
        {
            if (file == null) return BadRequest(new { Error = "Please upload an image file." });
            
            // Ensure the model is loaded
            await _bll.OcrInferenceService.LoadModelAsync();
            
            // Run inference on the image stream
            using var stream = file.OpenReadStream();
            var lines = await _bll.OcrInferenceService.RunInferenceAsync(stream);

            return Ok(lines);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error has occured during inference: {message}", e.Message);
            return BadRequest(new { Error = e.Message });
        }
    }
    
    /// <summary>
    /// Summarize the contents of the text.
    /// </summary>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Summarization([FromBody] SummarizationText? summarizationText)
    {
        try
        {
            var inferenceInput = summarizationText?.text;
            if (inferenceInput == null) return BadRequest(new { Error = "Please upload an image file." });
            // enforce your 2000-char rule
            const int CHAR_LIMIT = 2000;
            if (inferenceInput.Length > CHAR_LIMIT) throw new ArgumentException($"Input is {inferenceInput.Length} chars; max allowed is {CHAR_LIMIT}.");
            
            // Run inference on the text
            await _bll.SummarizationInferenceService.LoadModelAsync();
            var summarizedText = await _bll.SummarizationInferenceService.RunInferenceAsync(inferenceInput);

            return Ok(summarizedText);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error has occured during inference: {message}", e.Message);
            return BadRequest(new { Error = e.Message });
        }
    }
    
    /// <summary>
    /// Extract handwritten text lines from the uploaded image and performs summarization of extracted text.
    /// </summary>
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> Combined([FromForm] IFormFile? file)
    {
        try
        {
            if (file == null) return BadRequest(new { Error = "Please upload an image file." });
            
            // Ensure the model is loaded
            await _bll.OcrInferenceService.LoadModelAsync();
            await _bll.SummarizationInferenceService.LoadModelAsync();
            
            // Extract text from the image and summarize
            using var stream = file.OpenReadStream();
            var lines = await _bll.OcrInferenceService.RunInferenceAsync(stream);
            var summarizedText = await _bll.SummarizationInferenceService.RunInferenceAsync(String.Join(" ", lines));
            
            return Ok(summarizedText);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error has occured during inference: {message}", e.Message);
            return BadRequest(new { Error = e.Message });
        }
    }
}