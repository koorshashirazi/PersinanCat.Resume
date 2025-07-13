// Experience Cards Animation
function initializeExperienceAnimation() {
    const experienceSection = document.querySelector('.experience-section');
    if (!experienceSection) return;

    // Use IntersectionObserver to trigger animation when section comes into view
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                animateExperienceCards();
                observer.unobserve(entry.target);
            }
        });
    }, {
        threshold: 0.2,
        rootMargin: '0px 0px -50px 0px'
    });

    observer.observe(experienceSection);
}

function animateExperienceCards() {
    const experienceCards = document.querySelectorAll('.experience-section .experience-card');
    
    experienceCards.forEach((card, index) => {
        setTimeout(() => {
            // Add pulse animation class
            card.classList.add('animate-pulse');
            
            // Remove pulse class after animation completes
            setTimeout(() => {
                card.classList.remove('animate-pulse');
            }, 600);
        }, index * 100); // Stagger the animations
    });

    // Add the animate class to the section for any CSS-based animations
    setTimeout(() => {
        document.querySelector('.experience-section').classList.add('animate');
    }, 100);
}

// Add hover effects for cards
function addCardHoverEffects() {
    const experienceCards = document.querySelectorAll('.experience-card');
    
    experienceCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-8px) scale(1.02)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', function() {
    initializeExperienceAnimation();
    addCardHoverEffects();
});

// Also initialize on page load as backup
window.addEventListener('load', function() {
    initializeExperienceAnimation();
    addCardHoverEffects();
});

// Expose functions globally for Blazor integration
window.animateExperienceCards = animateExperienceCards;
window.initializeExperienceAnimation = initializeExperienceAnimation;
