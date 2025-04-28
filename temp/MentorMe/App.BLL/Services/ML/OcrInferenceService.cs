using System.Text;
using Microsoft.ML;
using Microsoft.ML.Data;
using OpenCvSharp;
using App.BLL.Contracts;

namespace App.BLL.Services.ML
{
    public class OcrInferenceService : IOcrInferenceService
    {
        private readonly MLContext _mlContext = new();
        private ITransformer? _mlModel;
        private PredictionEngine<OcrInput, OcrOutput>? _predictionEngine;
        
        private const string MODEL_DIR = "Models/OCR";
        private const int ImageHeight = 128;
        private const int ImageWidth  = 800;
        private const int TimeSteps   = ImageWidth / 16;
        private const int VocabSize   = 80;

        // Same char-map used in training:
        private static readonly char[] CharMap =
        [
            ' ', '!', '"', '#', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0',
            '1','2','3','4','5','6','7','8','9',':',';','?','A','B','C','D','E','F',
            'G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X',
            'Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p',
            'q','r','s','t','u','v','w','x','y','z','_'
        ];

        public OcrInferenceService() => LoadModelAsync().Wait();

        public Task LoadModelAsync()
        {
            if (_mlModel != null) return Task.CompletedTask;

            // 1) Load your SavedModel directory
            var tfModel = _mlContext.Model.LoadTensorFlowModel(MODEL_DIR);

            // 2) Wire up the pipeline
            var pipeline = tfModel.ScoreTensorFlowModel(
                outputColumnNames: ["StatefulPartitionedCall"],
                inputColumnNames: ["serving_default_image_input"],
                addBatchDimensionInput: true);

            // 3) Fit an empty IDataView just to bind it
            var emptyDv  = _mlContext.Data.LoadFromEnumerable(new List<OcrInput>());
            _mlModel = pipeline.Fit(emptyDv);

            // 4) Finally get your PredictionEngine
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<OcrInput, OcrOutput>(_mlModel);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> RunInferenceAsync(Stream imageStream)
        {
            if (_predictionEngine == null) throw new InvalidOperationException("Model not loaded.");

            using var mat = Mat.FromStream(imageStream, ImreadModes.Color);
            var boxes = DetectLines(mat);
            var results = new List<string>();

            foreach (var rect in boxes)
            {
                // Resize and convert ROI to gray
                using var roiBgr   = new Mat(mat, rect);
                using var resized  = new Mat();
                Cv2.Resize(roiBgr, resized, new Size(ImageWidth, ImageHeight));
                using var grayMat = new Mat();
                Cv2.CvtColor(resized, grayMat, ColorConversionCodes.BGR2GRAY);

                // Flatten grayMat (which is 1‐channel 128×800)
                var pixels = new float[ImageHeight * ImageWidth];
                for (int y = 0; y < ImageHeight; y++)
                    for (int x = 0; x < ImageWidth; x++)
                        pixels[y * ImageWidth + x] = grayMat.At<byte>(y, x) / 255f;
                var input = new OcrInput { Image = pixels };

                // Run
                var output = _predictionEngine.Predict(input);

                // Decode CTC
                results.Add(DecodeCtcGreedy(output.Logits));
            }

            return Task.FromResult(results.AsEnumerable());
        }

        private static Rect[] DetectLines(Mat image)
        {
            using var gray = new Mat();
            Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(gray, gray, 200, 255, ThresholdTypes.BinaryInv);
            Cv2.Dilate(gray, gray, null, iterations: 1);

            var contours = Cv2.FindContoursAsArray(
                gray, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            return contours
                .Select(c => Cv2.BoundingRect(c))
                .Where(r => r.Width > 30 && r.Height > 10)
                .OrderBy(r => r.Y)
                .ToArray();
        }

        private static string DecodeCtcGreedy(float[] flatLogits)
        {
            var sb   = new StringBuilder();
            var last = -1;

            for (int t = 0; t < TimeSteps; t++)
            {
                int best    = 0;
                float max   = float.MinValue;
                int offset  = t * VocabSize;

                for (int v = 0; v < VocabSize; v++)
                {
                    var score = flatLogits[offset + v];
                    if (score > max) { max = score; best = v; }
                }

                if (best != VocabSize - 1 && best != last) sb.Append(CharMap[best]);
                last = best;
            }

            return sb.ToString();
        }

        private class OcrInput
        {
            [VectorType(ImageHeight, ImageWidth, 1)]
            [ColumnName("serving_default_image_input")]
            public float[] Image { get; set; } = null!;
        }

        private class OcrOutput
        {
            [VectorType(TimeSteps, VocabSize)]
            [ColumnName("StatefulPartitionedCall")]
            public float[] Logits { get; set; } = null!;
        }
    }
}
