const btn = document.querySelector('.navigate-top');

window.addEventListener('scroll', () => {
    if (document.documentElement.scrollTop > 500) {
        btn.style.pointerEvents = 'auto';
        btn.style.opacity = '1';
    }
    else {
        btn.style.pointerEvents = 'none';
        btn.style.opacity = '0';
    }
})