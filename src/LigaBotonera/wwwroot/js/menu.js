(() => {
    window.toggleSubmenu = (menuId) => {
        const submenu = document.getElementById(`${menuId}-submenu`);
        const arrow = document.getElementById(`${menuId}-arrow`);
        submenu.classList.toggle('open');
        arrow.classList.toggle('rotate-180');
    }
})();