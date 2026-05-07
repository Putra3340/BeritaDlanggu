
    const wrapper = document.querySelector('.navbar-search-wrapper');
    const toggle = wrapper.querySelector('.search-toggle');
    const input = wrapper.querySelector('input');
    const clearBtn = wrapper.querySelector('.btn-clear');

// open / close
toggle.addEventListener('click', () => {
        wrapper.classList.toggle('active');
    if (wrapper.classList.contains('active')) {
        setTimeout(() => input.focus(), 150);
    }
});

// typing detection
input.addEventListener('input', () => {
        wrapper.classList.toggle('has-text', input.value.length > 0);
});

// clear input
clearBtn.addEventListener('click', () => {
        input.value = '';
    input.focus();
    wrapper.classList.remove('has-text');
});
document.addEventListener('click', (e) => {
    const wrapper = document.getElementById('navSearch');

    if (!wrapper.contains(e.target)) {
        wrapper.classList.remove('active');
    }
});