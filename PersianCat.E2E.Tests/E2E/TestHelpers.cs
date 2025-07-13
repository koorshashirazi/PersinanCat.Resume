namespace PersianCat.E2E.Tests.E2E;

public static class TestHelpers
{
    public static class Selectors
    {
        public const string Header = "header.site-header";
        public const string Footer = "footer";
        public const string NavbarNav = ".navbar-nav";
        public const string NavbarToggler = ".navbar-toggler";
        public const string ContainerFluid = ".container-fluid";
        public const string ScrollToTop = ".scroll-to-top";
        public const string ProgressBar = ".progress-bar";
        public const string AosElement = "[data-aos]";
        public const string ToastBody = ".toast-body";
        public const string ValidationMessage = ".validation-message";
        public const string LanguageDropdown = "[data-testid='language-dropdown']";
        public const string ThemeToggleButton = "[data-testid='theme-toggle-button']";
    }

    public static class Sections
    {
        public const string About = "#about";
        public const string Specializations = "#specializations";
        public const string Skills = "#skills";
        public const string Experience = "#experience";
        public const string Roadmap = "#roadmap";
        public const string Contact = "#contact";
    }

    public static class Languages
    {
        public const string German = "de-DE";
        public const string Persian = "fa-IR";
        public const string English = "en-US";
    }

    public static class Messages
    {
        public const string SuccessMessage = "Your message has been sent. Thank you!";
        public const string ValidationErrorMessage = "Please fill in all required fields correctly";
    }

    public static class SocialMedia
    {
        public static readonly Dictionary<string, string> Links = new()
        {
            { "https://github.com/koorshashirazi", "GitHub" },
            { "https://www.linkedin.com/in/koorsha-shirazi/", "LinkedIn" },
            { "https://www.xing.com/profile/Javad_Bagheri01657/web_profiles?expandNeffi=true", "Xing" }
        };
    }

    public static class Timeouts
    {
        public const int Default = 5000;
        public const int Long = 10000;
        public const int Performance = 3000;
    }

    public static class ViewportSizes
    {
        public static readonly (int width, int height) Mobile = (375, 667); // iPhone 8
        public static readonly (int width, int height) Tablet = (768, 1024); // iPad
        public static readonly (int width, int height) Desktop = (1920, 1080); // Full HD
    }
}
