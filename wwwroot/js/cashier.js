// Cashier page accounting theme enhancements

window.addEventListener('DOMContentLoaded', () => {
    // Add subtle animation to the accounting cards
    const accountingCards = document.querySelectorAll('.accounting-card');
    
    accountingCards.forEach(card => {
        // Add subtle hover effect
        card.addEventListener('mouseenter', () => {
            card.style.transition = 'transform 0.3s ease, box-shadow 0.3s ease';
        });
        
        // Add subtle animation when the page loads
        setTimeout(() => {
            card.style.opacity = '0';
            card.style.transform = 'translateY(20px)';
            card.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
            
            setTimeout(() => {
                card.style.opacity = '1';
                card.style.transform = 'translateY(0)';
            }, 100 * Array.from(accountingCards).indexOf(card));
        }, 300);
    });
    
    // Format currency values with animation
    const amountElements = document.querySelectorAll('.amount, .total-amount');
    amountElements.forEach(el => {
        const originalText = el.textContent;
        if (originalText.includes('$')) {
            // Add subtle highlight effect to amounts
            el.style.transition = 'background-color 0.5s ease';
            el.style.backgroundColor = 'rgba(39, 174, 96, 0.1)';
            
            setTimeout(() => {
                el.style.backgroundColor = 'transparent';
            }, 1500);
        }
    });
    
    // Add subtle effect to the header
    const header = document.querySelector('.cashier-header');
    if (header) {
        header.style.opacity = '0';
        header.style.transform = 'translateY(-10px)';
        header.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
        
        setTimeout(() => {
            header.style.opacity = '1';
            header.style.transform = 'translateY(0)';
        }, 200);
    }
});