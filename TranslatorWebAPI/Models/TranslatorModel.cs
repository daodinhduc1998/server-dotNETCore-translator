namespace TranslatorWebAPI.Models
{
    public class TranslatorModel
    {
        public string? chinese { get; set; }
        public int wrapType { get; set; }

        public int translationAlgorithm { get; set; }

        public int prioritizedName { get; set; }

        public int mode { get; set; }

    }
}
