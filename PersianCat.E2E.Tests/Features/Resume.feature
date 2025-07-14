Feature: Resume Page E2E Tests

@all
Scenario: Page Load and Initial Display
    Given the resume page is open
    Then the page title should be "PersianCat"
    And the header should be visible
    And the footer should be visible

@all
Scenario Outline: Navigation
    Given the resume page is open
    When I click on the "<link>" link
    Then the "<section>" section should be visible
    Examples:
        | link        | section    |
        | About       | about      |
        | Experience  | experience |
        | Skills      | skills     |
        | GetInTouch  | contact    |

@all
Scenario Outline: Language Change
    Given the resume page is open
    When I change the language to "<language>"
    Then the page should be in <language>
    Examples:
        | language |
        | German   |
        | Persian  |
        | English  |

@all
Scenario Outline: Content Verification
    Given the resume page is open in "<language>"
    Then the content of the page should be correct in "<language>"
    Examples:
        | language |
        | German   |
        | Persian  |
        | English  |

@all
Scenario: Social Media Links
    Given the resume page is open
    Then the social media links should be correct

@all
Scenario: Theme Change
    Given the resume page is open
    When I change the theme to "dark"
    Then the page should have a dark theme
    When I change the theme to "light"
    Then the page should have a light theme

@all
Scenario: Contact Form Submission
    Given the resume page is open
    When I fill out the contact form with valid data
    And I submit the contact form
    Then I should see a success message
    When I fill out the contact form with invalid data
    And I submit the contact form
    Then I should see a validation error message

@all
Scenario: Responsive Design Testing
    Given the resume page is open
    When I resize the browser to mobile size
    Then the navigation should be responsive
    And the content should be properly arranged for mobile

@all
Scenario: Scroll Navigation
    Given the resume page is open
    When I scroll down to the bottom of the page
    Then the scroll to top button should be visible
    When I click the scroll to top button
    Then the page should scroll to the top

@all
Scenario: Animation and AOS Testing
    Given the resume page is open
    When I scroll to the skills section
    Then the skill bars should animate
    And the AOS animations should trigger

@all
Scenario: Performance Testing
    Given the resume page is open
    Then the page should load within 3 seconds
    And all images should be loaded
    And the page should be accessible

@all
Scenario: Theme Toggle Functionality
    Given the resume page is open
    When I toggle the theme multiple times
    Then the theme should change consistently
    And the theme preference should be saved

@all
Scenario: Language Persistence
    Given the resume page is open
    When I change the language to "German"
    And I refresh the page
    Then the page should still be in German

@all
Scenario: Contact Form Validation Messages
    Given the resume page is open
    When I try to submit an empty contact form
    Then I should see individual field validation messages
    When I enter a subject with less than 10 characters
    Then I should see a subject length validation message
    When I enter a message with less than 10 characters
    Then I should see a message length validation message

@all
Scenario: Skills Section Interaction
    Given the resume page is open
    When I navigate to the skills section
    Then all skill categories should be visible
    And the skill percentages should be displayed correctly

@all
Scenario: Experience Section Content
    Given the resume page is open
    When I navigate to the experience section
    Then the experience timeline should be visible
    And the experience details should be properly formatted


@all
Scenario: Specializations Section Content
    Given the resume page is open
    When I navigate to the specializations section
    Then the specializations section should be visible
    And the specialization cards should be displayed
    And each specialization should have an icon

@all
Scenario: Roadmap Section Content
    Given the resume page is open
    When I navigate to the roadmap section
    Then the roadmap section should be visible
    And the roadmap timeline should be displayed

@all
Scenario: Navigation Menu Functionality
    Given the resume page is open
    When I click on each navigation menu item
    Then the corresponding section should become visible
    And the URL should update accordingly

@all
Scenario: Header Components Visibility
    Given the resume page is open
    Then the header should contain the logo
    And the navigation menu should be visible
    And the language switcher should be visible
    And the theme switcher should be visible

@all
Scenario: Footer Information
    Given the resume page is open
    When I scroll to the footer
    Then the footer should contain social media links
