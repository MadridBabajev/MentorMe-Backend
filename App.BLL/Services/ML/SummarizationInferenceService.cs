using Microsoft.ML;
using Microsoft.ML.Data;
using Tokenizers.DotNet;
using App.BLL.Contracts;

namespace App.BLL.Services.ML
{
    public class SummarizationInferenceService : ISummarizationInferenceService
    {
        private readonly MLContext _mlContext = new();
        private ITransformer? _mlModel;
        private PredictionEngine<SummarizationInput, SummarizationOutput>? _predictionEngine;
        private Tokenizer? _tokenizer;

        private const string MODEL_DIR = "Models/Summarization";
        private const string TOKENIZER_HUB_NAME = "t5-base";
        private const string TOKENIZER_FILENAME = "tokenizer.json";
        private const int MAX_ENC = 512;
        private const int MAX_DEC = 150;
        private const int PAD_TOKEN_ID = 0;
        private const int EOS_TOKEN_ID = 1;
        private const int VOCAB_SIZE = 32128;

        public SummarizationInferenceService()
            => LoadModelAsync().Wait();

        public async Task LoadModelAsync()
        {
            if (_mlModel != null && _tokenizer != null) return;

            // 1) tokenizer.json from HF hub (or local fallback)
            string tokenizerPath;
            try
            {
                tokenizerPath = await HuggingFace
                    .GetFileFromHub(TOKENIZER_HUB_NAME, TOKENIZER_FILENAME, MODEL_DIR);
            }
            catch
            {
                tokenizerPath = Path.Combine(MODEL_DIR, TOKENIZER_FILENAME);
            }

            _tokenizer = new Tokenizer(vocabPath: tokenizerPath);

            // 2) load SavedModel
            var tfModel = _mlContext.Model.LoadTensorFlowModel(MODEL_DIR);

            // 3) wire up pipeline with the TF node names
            var pipeline = tfModel.ScoreTensorFlowModel(
                outputColumnNames: ["StatefulPartitionedCall"],
                inputColumnNames:
                [
                    "serving_default_encoder_input_ids",
                    "serving_default_encoder_attention_mask",
                    "serving_default_decoder_input_ids"
                ],
                addBatchDimensionInput: false);

            // 4) bind to an empty IDataView
            var emptyDv = _mlContext.Data.LoadFromEnumerable(new List<SummarizationInput>());
            _mlModel = pipeline.Fit(emptyDv);
            _predictionEngine = _mlContext.Model
                .CreatePredictionEngine<SummarizationInput, SummarizationOutput>(_mlModel);
        }

        public Task<IEnumerable<string>> RunInferenceAsync(string inferenceInput)
        {
            if (_predictionEngine == null || _tokenizer == null) throw new InvalidOperationException("Model or tokenizer not loaded.");

            // 1) prefix & tokenize
            var prompt = $"summarize: {inferenceInput}";
            var rawIds = _tokenizer.Encode(prompt);
            var inputIds = rawIds.Select(id => (int)id).ToArray();
            var mask = inputIds.Select(_ => 1).ToArray();

            // 2) pad to exactly MAX_ENC
            if (inputIds.Length < MAX_ENC)
            {
                inputIds = inputIds
                    .Concat(Enumerable.Repeat(PAD_TOKEN_ID, MAX_ENC - inputIds.Length))
                    .ToArray();
                mask = mask
                    .Concat(Enumerable.Repeat(0, MAX_ENC - mask.Length))
                    .ToArray();
            }

            // 4) prepare decoder buffer & collector
            var decoderIds = new int[MAX_DEC]; // all 0 = PAD_TOKEN_ID
            var generated = new List<int>();

            // 5) greedy, step-by-step decode
            for (int step = 0; step < MAX_DEC; step++)
            {
                var sample = new SummarizationInput
                {
                    EncoderInputIds = inputIds,
                    EncoderAttentionMask = mask,
                    DecoderInputIds = decoderIds
                };
                var result = _predictionEngine.Predict(sample);
                var logits = result.StatefulPartitionedCall;

                // find argmax for this time-step
                int best = 0;
                float top = float.MinValue;
                int offset = step * VOCAB_SIZE;
                for (int v = 0; v < VOCAB_SIZE; v++)
                {
                    float score = logits[offset + v];
                    if (score > top)
                    {
                        top = score;
                        best = v;
                    }
                }

                // stop on EOS
                if (best == EOS_TOKEN_ID) break;

                generated.Add(best);
                decoderIds[step + 1] = best;
            }

            // 6) decode back to text (needs uint[])
            var decoded = generated
                .Select(i => (uint)i)
                .ToArray();
            var summary = _tokenizer.Decode(decoded);

            return Task.FromResult<IEnumerable<string>>([summary]);
        }

        private class SummarizationInput
        {
            [VectorType(MAX_ENC)]
            [ColumnName("serving_default_encoder_input_ids")]
            public int[] EncoderInputIds { get; set; } = new int[MAX_ENC];

            [VectorType(MAX_ENC)]
            [ColumnName("serving_default_encoder_attention_mask")]
            public int[] EncoderAttentionMask { get; set; } = new int[MAX_ENC];

            [VectorType(MAX_DEC)]
            [ColumnName("serving_default_decoder_input_ids")]
            public int[] DecoderInputIds { get; set; } = new int[MAX_DEC];
        }

        private class SummarizationOutput
        {
            [VectorType(MAX_DEC, VOCAB_SIZE)]
            [ColumnName("StatefulPartitionedCall")]
            public float[] StatefulPartitionedCall { get; set; }
                = new float[MAX_DEC * VOCAB_SIZE];
        }
    }
}