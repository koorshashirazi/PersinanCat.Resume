namespace PersianCat.E2E.Tests.E2E
{
    public static class TestRunContext
    {
        public static WebAppDriver? Driver { get; set; }
        public static TranslationService? Translations { get; set; }
        public static TestConfiguration? Config { get; set; }
        public static ResumeAppContainer? ResumeApp { get; set; }
    }
}
