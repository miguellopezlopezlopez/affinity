// Smooth scrolling for anchor links
document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Fade in animation on scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('visible');
        }
    });
}, observerOptions);

// Observe all fade-in elements
document.querySelectorAll('.fade-in').forEach(el => {
    observer.observe(el);
});

// Header background on scroll
window.addEventListener('scroll', function() {
    const header = document.querySelector('.header');
    if (window.scrollY > 100) {
        header.style.background = 'rgba(255, 255, 255, 0.98)';
        header.style.boxShadow = '0 2px 20px rgba(0, 0, 0, 0.1)';
    } else {
        header.style.background = 'rgba(255, 255, 255, 0.95)';
        header.style.boxShadow = 'none';
    }
});

// Add click handlers for CTA buttons (placeholder)
document.querySelectorAll('.btn-primary').forEach(btn => {
    btn.addEventListener('click', function(e) {
        e.preventDefault();
        // Redirect to registration page
        window.location.href = 'register.html'; // Your registration page
    });
});

document.querySelectorAll('.btn-outline').forEach(btn => {
    btn.addEventListener('click', function(e) {
        e.preventDefault();
        const text = this.textContent.trim();
        if (text === 'Iniciar Sesión') {
            window.location.href = 'login.html'; // Your login page
        } else {
            // Scroll to features section
            document.querySelector('.features-section').scrollIntoView({
                behavior: 'smooth'
            });
        }
    });
});

// Animate stats counter on scroll
let statsAnimated = false;
const statsSection = document.querySelector('.stats');

const statsObserver = new IntersectionObserver(function(entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting && !statsAnimated) {
            statsAnimated = true;
            animateStats();
        }
    });
}, { threshold: 0.5 });

if (statsSection) {
    statsObserver.observe(statsSection);
}

function animateStats() {
    const statNumbers = document.querySelectorAll('.stat-number');
    statNumbers.forEach(stat => {
        const finalText = stat.textContent;
        const isPercentage = finalText.includes('%');
        const isK = finalText.includes('K');
        const numOnly = parseInt(finalText.replace(/[^\d]/g, ''));
        
        let current = 0;
        const increment = numOnly / 50;
        const timer = setInterval(() => {
            current += increment;
            if (current >= numOnly) {
                current = numOnly;
                clearInterval(timer);
            }
            
            let displayValue = Math.floor(current);
            if (isK) {
                displayValue = (displayValue / 1000).toFixed(1) + 'K';
            }
            if (isPercentage) {
                displayValue += '%';
            }
            if (finalText.includes('+')) {
                displayValue += '+';
            }
            
            stat.textContent = displayValue;
        }, 30);
    });
}