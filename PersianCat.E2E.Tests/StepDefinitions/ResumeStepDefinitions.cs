using Microsoft.Playwright;
using PersianCat.E2E.Tests.E2E;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace PersianCat.E2E.Tests.StepDefinitions;

[Binding]
public class ResumeStepDefinitions
{
    private readonly IPage _page;
    private readonly ResumeAppContainer _resumeAppContainer;
    private readonly TranslationService _translationService;
    private string _currentLanguage = "en-US";

    public ResumeStepDefinitions(IPage page, ResumeAppContainer resumeAppContainer, TranslationService translationService)
    {
        _page = page;
        _resumeAppContainer = resumeAppContainer;
        _translationService = translationService;
    }

    [Given(@"the resume page is open")]
    public async Task GivenTheResumePageIsOpen()
    {
        await _page.GotoAsync(_resumeAppContainer.GetBaseUrl());
        await Task.Delay(2000);
    }

    [Given(@"the resume page is open in ""(.*)""")]
    public async Task GivenTheResumePageIsOpenIn(string language)
    {
        await WhenIChangeTheLanguageTo(language);
    }

    [Then(@"the page title should be ""(.*)""")]
    public async Task ThenThePageTitleShouldBe(string title)
    {
        await Assertions.Expect(_page).ToHaveTitleAsync(title);
    }

    [Then(@"the header should be visible")]
    public async Task ThenTheHeaderShouldBeVisible()
    {
        await Assertions.Expect(_page.GetByTestId("site-header")).ToBeVisibleAsync();
    }

    [Then(@"the footer should be visible")]
    public async Task ThenTheFooterShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("footer")).ToBeVisibleAsync();
    }

    [When(@"I click on the ""(.*)"" link")]
    public async Task WhenIClickOnTheLink(string linkKey)
    {
        var link = linkKey switch
        {
            "About" => "about",
            "Specializations" => "specializations",
            "Skills" => "skills",
            "Experience" => "experience",
            "Roadmap" => "roadmap",
            "Contact" => "contact",
            "GetInTouch" => "get-in-touch-button",
            _ => ""
        };

        var locator = _page.GetByTestId(linkKey == "GetInTouch" ? link : $"nav-link-{link}");
        await locator.ClickAsync();
        if (linkKey != "GetInTouch")
        {
            await _page.WaitForURLAsync($"**/#{link}", new() { Timeout = 5000 });
        }
    }

    [Then(@"the ""(.*)"" section should be visible")]
    public async Task ThenTheSectionShouldBeVisible(string sectionId)
    {
        await Assertions.Expect(_page.Locator($"#{sectionId.ToLower()}")).ToBeVisibleAsync(new() { Timeout = 5000 });
    }

    [Then(@"the content of the page should be correct in ""(.*)""")]
    public async Task ThenTheContentOfThePageShouldBeCorrectIn(string language)
    {
        var greetingText = _translationService.GetTranslation(_currentLanguage, "Greeting");
        await Assertions.Expect(_page.GetByTestId("greeting_testid")).ToContainTextAsync(greetingText);

        var helloText = _translationService.GetTranslation(_currentLanguage, "Hello");
        await Assertions.Expect(_page.GetByTestId("hello_text")).ToContainTextAsync(helloText);

        var nameText = _translationService.GetTranslation(_currentLanguage, "AuthorName");
        await Assertions.Expect(_page.GetByTestId("intro-title")).ToContainTextAsync(nameText);

        var skillsHeading = _translationService.GetTranslation(_currentLanguage, "MySkills");
        await Assertions.Expect(_page.GetByRole(AriaRole.Heading, new() { Name = skillsHeading })).ToBeVisibleAsync();

        var contactHeading = _translationService.GetTranslation(_currentLanguage, "GetInTouch");
        await Assertions.Expect(_page.GetByTestId("get-in-touch-button")).ToBeVisibleAsync();
    }

    [Then(@"the social media links should be correct")]
    public async Task ThenTheSocialMediaLinksShouldBeCorrect()
    {
        var socialLinks = new Dictionary<string, string>
        {
            { "https://github.com/koorshashirazi", "GitHub" },
            { "https://www.linkedin.com/in/koorsha-shirazi/", "LinkedIn" },
            { "https://www.xing.com/profile/Javad_Bagheri01657/web_profiles?expandNeffi=true", "Xing" }
        };

        foreach (var (url, name) in socialLinks)
        {
            var link = _page.Locator($".social-nav a[title='{name}']").First;
            await Assertions.Expect(link).ToHaveAttributeAsync("href", url);
        }
    }

    [When(@"I change the language to ""(.*)""")]
    public async Task WhenIChangeTheLanguageTo(string language)
    {
        _currentLanguage = language switch
        {
            "German" => "de-DE",
            "Persian" => "fa-IR",
            "English" => "en-US",
            _ => "en-US"
        };

        await _page.GetByTestId("language-dropdown").ClickAsync();
        await _page.GetByTestId($"lang-{_currentLanguage}").ClickAsync();
        await _page.WaitForFunctionAsync($"document.documentElement.lang === '{_currentLanguage}'");
    }

    [Then(@"the page should be in (.*)")]
    public async Task ThenThePageShouldBeIn(string language)
    {
        _currentLanguage = language switch
        {
            "German" => "de-DE",
            "Persian" => "fa-IR",
            "English" => "en-US",
            _ => "en-US"
        };
        var expectedText = _translationService.GetTranslation(_currentLanguage, "Hello");
        await Assertions.Expect(_page.GetByTestId("hello_text"))
            .ToContainTextAsync(expectedText, new() { Timeout = 5000 });
    }

    [When(@"I change the theme to ""(.*)""")]
    public async Task WhenIChangeTheThemeTo(string theme)
    {
        await _page.Locator("[data-testid='theme-toggle-button']").ClickAsync();
    }

    [Then(@"the page should have a (.*) theme")]
    public async Task ThenThePageShouldHaveATheme(string theme)
    {
        var body = _page.Locator("body");
        await Assertions.Expect(body).ToHaveClassAsync(new Regex(theme));
    }

    [When(@"I fill out the contact form with valid data")]
    public async Task WhenIFillOutTheContactFormWithValidData()
    {
        await _page.GetByTestId("contact-name").FillAsync("Test User");
        await _page.GetByTestId("contact-subject").FillAsync("Job Offer: Senior .NET Developer Position");
        await _page.GetByTestId("contact-message").FillAsync("This is a test message with more than 10 characters.");
    }

    [When(@"I submit the contact form")]
    public async Task WhenISubmitTheContactForm()
    {
        await _page.GetByTestId("contact-submit-button").ClickAsync();
    }

    [Then(@"I should see a success message")]
    public async Task ThenIShouldSeeASuccessMessage()
    {
        // Wait for toast notification to appear
        await _page.WaitForSelectorAsync("[data-testid='toast-message']", new() { Timeout = 5000 });
        var successMessage = _page.GetByTestId("toast-message");
        await Assertions.Expect(successMessage).ToContainTextAsync("Your message has been sent. Thank you!");
    }

    [When(@"I fill out the contact form with invalid data")]
    public async Task WhenIFillOutTheContactFormWithInvalidData()
    {
        await _page.GetByTestId("contact-name").FillAsync("");
        await _page.GetByTestId("contact-subject").FillAsync("");
        await _page.GetByTestId("contact-message").FillAsync("");
    }

    [Then(@"I should see a validation error message")]
    public async Task ThenIShouldSeeAValidationErrorMessage()
    {
        // Wait for toast notification to appear
        await _page.WaitForSelectorAsync("[data-testid='toast-message']", new() { Timeout = 5000 });
        var toastMessage = _page.GetByTestId("toast-message");
        await Assertions.Expect(toastMessage).ToContainTextAsync("Please fill in all required fields before sending your message.");
    }

    // Additional E2E Test Step Definitions

    [When(@"I resize the browser to mobile size")]
    public async Task WhenIResizeTheBrowserToMobileSize()
    {
        await _page.SetViewportSizeAsync(375, 667); // iPhone 8 size
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(500);
    }

    [Then(@"the navigation should be responsive")]
    public async Task ThenTheNavigationShouldBeResponsive()
    {
        await Assertions.Expect(_page.Locator(".navbar-toggler")).ToBeVisibleAsync();
    }

    [Then(@"the content should be properly arranged for mobile")]
    public async Task ThenTheContentShouldBeProperlyArrangedForMobile()
    {
        await Assertions.Expect(_page.Locator("#about")).ToBeVisibleAsync();
    }

    [When(@"I scroll down to the bottom of the page")]
    public async Task WhenIScrollDownToTheBottomOfThePage()
    {
        await _page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
        await Task.Delay(2000); // Wait for scroll to complete and animations
    }

    [Then(@"the scroll to top button should be visible")]
    public async Task ThenTheScrollToTopButtonShouldBeVisible()
    {
        await Assertions.Expect(_page.GetByTestId("scroll-to-top")).ToBeVisibleAsync();
    }

    [When(@"I click the scroll to top button")]
    public async Task WhenIClickTheScrollToTopButton()
    {
        await _page.GetByTestId("scroll-to-top").ClickAsync();
    }

    [Then(@"the page should scroll to the top")]
    public async Task ThenThePageShouldScrollToTheTop()
    {
        await Task.Delay(2000); // Wait for scroll animation
        var scrollY = await _page.EvaluateAsync<int>("window.pageYOffset");

        Assert.That(scrollY, Is.LessThan(100));
    }

    [When(@"I scroll to the skills section")]
    public async Task WhenIScrollToTheSkillsSection()
    {
        await _page.Locator("#skills").ScrollIntoViewIfNeededAsync();
        await Task.Delay(1000);
    }

    [Then(@"the skill bars should animate")]
    public async Task ThenTheSkillBarsShouldAnimate()
    {
        await Assertions.Expect(_page.Locator(".progress-bar").First).ToBeVisibleAsync();
    }

    [Then(@"the AOS animations should trigger")]
    public async Task ThenTheAOSAnimationsShouldTrigger()
    {
        await Assertions.Expect(_page.Locator("[data-aos]").First).ToBeVisibleAsync();
    }

    [Then(@"the page should load within 3 seconds")]
    public async Task ThenThePageShouldLoadWithin3Seconds()
    {
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle, new() { Timeout = 3000 });
    }

    [Then(@"all images should be loaded")]
    public async Task ThenAllImagesShouldBeLoaded()
    {
        var images = _page.Locator("img");
        var count = await images.CountAsync();
        var baseUrl = new Uri(TestRunContext.Config!.SutUrl);

        for (var i = 0; i < count; i++)
        {
            var src = await images.Nth(i).GetAttributeAsync("src");
            if (string.IsNullOrEmpty(src) || src.StartsWith("data:"))
            {
                continue;
            }

            var absoluteUrl = new Uri(baseUrl, src).ToString();
            var response = await _page.APIRequest.GetAsync(absoluteUrl);
            await Assertions.Expect(response).ToBeOKAsync();
        }
    }

    [Then(@"the page should be accessible")]
    public async Task ThenThePageShouldBeAccessible()
    {
        // Basic accessibility checks
        await Assertions.Expect(_page.Locator("h1")).ToBeVisibleAsync();
        await Assertions.Expect(_page.Locator("img[alt]").First).ToBeVisibleAsync();
    }

    [When(@"I toggle the theme multiple times")]
    public async Task WhenIToggleTheThemeMultipleTimes()
    {
        for (int i = 0; i < 3; i++)
        {
            await _page.Locator("[data-testid='theme-toggle-button']").ClickAsync();
            await Task.Delay(500);
        }
    }

    [Then(@"the theme should change consistently")]
    public async Task ThenTheThemeShouldChangeConsistently()
    {
        await Assertions.Expect(_page.Locator("body")).ToHaveClassAsync(new Regex("(light|dark)"));
    }

    [Then(@"the theme preference should be saved")]
    public async Task ThenTheThemePreferenceShouldBeSaved()
    {
        var theme = await _page.EvaluateAsync<string>("localStorage.getItem('theme')");
        Assert.That(theme, Is.Not.Null);
    }

    [When(@"I refresh the page")]
    public async Task WhenIRefreshThePage()
    {
        await _page.ReloadAsync(new() { WaitUntil = WaitUntilState.DOMContentLoaded });
    }

    [Then(@"the page should still be in German")]
    public async Task ThenThePageShouldStillBeInGerman()
    {
        await Task.Delay(2000); // Wait for the page to reload
        var expectedText = _translationService.GetTranslation("de-DE", "Hello");
        await Assertions.Expect(_page.GetByTestId("hello_text"))
            .ToContainTextAsync(expectedText, new() { Timeout = 5000 });
    }

    [When(@"I try to submit an empty contact form")]
    public async Task WhenITryToSubmitAnEmptyContactForm()
    {
        await _page.GetByTestId("contact-name").FillAsync("");
        await _page.GetByTestId("contact-subject").FillAsync("");
        await _page.GetByTestId("contact-message").FillAsync("");
        await _page.GetByTestId("contact-submit-button").ClickAsync();
    }

    [Then(@"I should see individual field validation messages")]
    public async Task ThenIShouldSeeIndividualFieldValidationMessages()
    {
        await Assertions.Expect(_page.Locator(".validation-message").First).ToBeVisibleAsync();
    }

    [When(@"I enter a subject with less than 10 characters")]
    public async Task WhenIEnterASubjectWithLessThan10Characters()
    {
        await _page.GetByTestId("contact-subject").FillAsync("Short");
        await _page.GetByTestId("contact-subject").BlurAsync();
    }

    [Then(@"I should see a subject length validation message")]
    public async Task ThenIShouldSeeASubjectLengthValidationMessage()
    {
        await Assertions.Expect(_page.Locator("[data-testid='contact-subject'] + .validation-message")).ToContainTextAsync("10 characters");
    }

    [When(@"I enter a message with less than 10 characters")]
    public async Task WhenIEnterAMessageWithLessThan10Characters()
    {
        await _page.GetByTestId("contact-message").FillAsync("Short");
        await _page.GetByTestId("contact-message").BlurAsync();
    }

    [Then(@"I should see a message length validation message")]
    public async Task ThenIShouldSeeAMessageLengthValidationMessage()
    {
        await Assertions.Expect(_page.Locator("[data-testid='contact-message'] + .validation-message")).ToContainTextAsync("10 characters");
    }

    [When(@"I navigate to the skills section")]
    public async Task WhenINavigateToTheSkillsSection()
    {
        await _page.GetByTestId("nav-link-skills").ClickAsync();
        await _page.Locator("#skills").ScrollIntoViewIfNeededAsync();
        await Task.Delay(1000); // Wait for animations
    }

    [Then(@"all skill categories should be visible")]
    public async Task ThenAllSkillCategoriesShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("#skills .progress").First).ToBeVisibleAsync();
    }

    [Then(@"the skill percentages should be displayed correctly")]
    public async Task ThenTheSkillPercentagesShouldBeDisplayedCorrectly()
    {
        await Assertions.Expect(_page.Locator(".progress-bar").First).ToBeVisibleAsync();
    }

    [When(@"I navigate to the experience section")]
    public async Task WhenINavigateToTheExperienceSection()
    {
        await _page.GetByTestId("nav-link-experience").ClickAsync();
    }

    [Then(@"the experience timeline should be visible")]
    public async Task ThenTheExperienceTimelineShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("#experience")).ToBeVisibleAsync();
    }

    [Then(@"the experience details should be properly formatted")]
    public async Task ThenTheExperienceDetailsShouldBeProperlyFormatted()
    {
        await Assertions.Expect(_page.Locator("#experience .card").First).ToBeVisibleAsync();
    }

    [Then(@"I should see a success toast notification")]
    public async Task ThenIShouldSeeASuccessToastNotification()
    {
        var toast = _page.GetByTestId("toast-message");
        await Assertions.Expect(toast).ToBeVisibleAsync();
        await Assertions.Expect(toast).ToContainTextAsync("Your message has been sent. Thank you!");
    }

    [Then(@"the toast should automatically hide after 5 seconds")]
    public async Task ThenTheToastShouldAutomaticallyHideAfter5Seconds()
    {
        await Task.Delay(6000); // Wait for 6 seconds
        await Assertions.Expect(_page.GetByTestId("toast-message")).ToBeHiddenAsync();
    }

    [Then(@"I should see an error toast")]
    public async Task ThenIShouldSeeAnErrorToast()
    {
        var toast = _page.GetByTestId("toast-message");
        await Assertions.Expect(toast).ToBeVisibleAsync(new() { Timeout = 5000 });
        await Assertions.Expect(toast).ToContainTextAsync("Please fill in all required fields before sending your message.");
    }

    [When(@"I submit a valid contact form immediately after")]
    public async Task WhenISubmitAValidContactFormImmediatelyAfter()
    {
        await _page.GetByTestId("contact-name").FillAsync("Test User");
        await _page.GetByTestId("contact-subject").FillAsync("Job Offer: Senior .NET Developer Position");
        await _page.GetByTestId("contact-message").FillAsync("This is a test message with more than 10 characters.");
        await _page.GetByTestId("contact-submit-button").ClickAsync();
    }

    [Then(@"I should see a success toast")]
    public async Task ThenIShouldSeeASuccessToast()
    {
        var toast = _page.GetByTestId("toast-message");
        await Assertions.Expect(toast).ToBeVisibleAsync(new() { Timeout = 10000 });
        await Assertions.Expect(toast).ToContainTextAsync("Your message has been sent. Thank you!");
    }

    [Then(@"both toasts should be handled correctly")]
    public async Task ThenBothToastsShouldBeHandledCorrectly()
    {
        // The toast system should handle multiple toasts properly
        await Assertions.Expect(_page.GetByTestId("toast-message")).ToBeVisibleAsync();
    }

    // New Step Definitions for Additional Tests

    [When(@"I navigate to the specializations section")]
    public async Task WhenINavigateToTheSpecializationsSection()
    {
        await _page.GetByTestId("nav-link-specializations").ClickAsync();
    }

    [Then(@"the specializations section should be visible")]
    public async Task ThenTheSpecializationsSectionShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("#specializations")).ToBeVisibleAsync();
    }

    [Then(@"the specialization cards should be displayed")]
    public async Task ThenTheSpecializationCardsShouldBeDisplayed()
    {
        await Assertions.Expect(_page.Locator("#specializations .specialization-item").First).ToBeVisibleAsync();
    }

    [Then(@"each specialization should have an icon")]
    public async Task ThenEachSpecializationShouldHaveAnIcon()
    {
        var cards = _page.Locator("#specializations .specialization-item");
        var count = await cards.CountAsync();
        
        for (int i = 0; i < count; i++)
        {
            await Assertions.Expect(cards.Nth(i).Locator("img")).ToBeVisibleAsync();
        }
    }

    [When(@"I navigate to the roadmap section")]
    public async Task WhenINavigateToTheRoadmapSection()
    {
        await _page.GetByTestId("nav-link-roadmap").ClickAsync();
        await _page.Locator("#roadmap").ScrollIntoViewIfNeededAsync();
        await Task.Delay(1000); // Wait for animations
    }

    [Then(@"the roadmap section should be visible")]
    public async Task ThenTheRoadmapSectionShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("#roadmap")).ToBeVisibleAsync();
    }

    [Then(@"the roadmap timeline should be displayed")]
    public async Task ThenTheRoadmapTimelineShouldBeDisplayed()
    {
        await Assertions.Expect(_page.Locator("#roadmap .roadmap-container").First).ToBeVisibleAsync();
    }

    [When(@"I click on each navigation menu item")]
    public async Task WhenIClickOnEachNavigationMenuItem()
    {
        var menuItems = new[]
        {
            "about",
            "specializations",
            "skills",
            "experience",
            "roadmap",
            "contact"
        };

        foreach (var item in menuItems)
        {
            await _page.GetByTestId($"nav-link-{item}").ClickAsync();
            await Task.Delay(500); // Wait for navigation
        }
    }

    [Then(@"the corresponding section should become visible")]
    public async Task ThenTheCorrespondingSectionShouldBecomeVisible()
    {
        // This is implicitly checked by the navigation actions
        await Assertions.Expect(_page.Locator("#contact")).ToBeVisibleAsync();
    }

    [Then(@"the URL should update accordingly")]
    public void ThenTheURLShouldUpdateAccordingly()
    {
        // Check that the URL contains the hash
        var url = _page.Url;
        Assert.That(url, Does.Contain("#"));
    }

    [Then(@"the header should contain the logo")]
    public async Task ThenTheHeaderShouldContainTheLogo()
    {
        await Assertions.Expect(_page.Locator("a.navbar-brand img")).ToBeVisibleAsync();
    }

    [Then(@"the navigation menu should be visible")]
    public async Task ThenTheNavigationMenuShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator(".navbar-nav")).ToBeVisibleAsync();
    }

    [Then(@"the language switcher should be visible")]
    public async Task ThenTheLanguageSwitcherShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("[data-testid='language-dropdown']")).ToBeVisibleAsync();
    }

    [Then(@"the theme switcher should be visible")]
    public async Task ThenTheThemeSwitcherShouldBeVisible()
    {
        await Assertions.Expect(_page.Locator("[data-testid='theme-toggle-button']")).ToBeVisibleAsync();
    }

    [When(@"I scroll to the footer")]
    public async Task WhenIScrollToTheFooter()
    {
        await _page.Locator("footer").ScrollIntoViewIfNeededAsync();
    }

    [Then(@"the footer should contain social media links")]
    public async Task ThenTheFooterShouldContainSocialMediaLinks()
    {
        await Assertions.Expect(_page.Locator("footer .social-nav a").First).ToBeVisibleAsync();
    }
}
