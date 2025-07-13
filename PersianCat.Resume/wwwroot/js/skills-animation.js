// Skills Progress Bar Animation
function initializeSkillsAnimation() {
    const skillsSection = document.querySelector('.skills-section');
    if (!skillsSection) return;

    // Use IntersectionObserver to trigger animation when section comes into view
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                animateSkillsProgressBars();
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.3,
        rootMargin: '0px 0px -50px 0px'
    });

    observer.observe(skillsSection);
}

function animateSkillsProgressBars() {
    const progressBars = document.querySelectorAll('.skills-section .animated-progress');
    
    progressBars.forEach((bar, index) => {
        const width = bar.getAttribute('data-width');
        const delay = parseInt(bar.getAttribute('data-delay') || 0);
        
        // Set the CSS custom property for target width
        bar.style.setProperty('--target-width', width + '%');
        
        // Animate to target width after delay
        setTimeout(() => {
            bar.classList.add('animate');
        }, delay);
    });
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', initializeSkillsAnimation);

// Also initialize on page load as backup
window.addEventListener('load', initializeSkillsAnimation);

// Expose function globally for Blazor integration
window.animateSkillsProgressBars = animateSkillsProgressBars;
window.initializeSkillsAnimation = initializeSkillsAnimation;
