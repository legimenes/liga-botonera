(() => {
    const sidebar = document.getElementById('sidebar');
    const sidebarToggle = document.getElementById('sidebar-toggle');
    const mobileSidebarToggle = document.getElementById('mobile-sidebar-toggle');
    const mainContent = document.getElementById('main-content');
    const overlay = document.getElementById('overlay');
    let sidebarCollapsed = false;

    sidebarToggle.addEventListener('click', () => {
        sidebarCollapsed = !sidebarCollapsed;

        if (sidebarCollapsed) {
            sidebar.style.width = '80px';
            mainContent.style.marginLeft = '80px';
            document.querySelectorAll('.sidebar-text').forEach(el => el.style.display = 'none');
        } else {
            sidebar.style.width = '256px';
            mainContent.style.marginLeft = '256px';
            document.querySelectorAll('.sidebar-text').forEach(el => el.style.display = '');
        }
    });

    mobileSidebarToggle.addEventListener('click', () => {
        sidebar.classList.toggle('open');
        overlay.classList.toggle('hidden');
    });

    overlay.addEventListener('click', () => {
        sidebar.classList.remove('open');
        overlay.classList.add('hidden');
    });

    window.addEventListener('resize', () => {
        if (window.innerWidth >= 768) {
            sidebar.classList.remove('open');
            overlay.classList.add('hidden');

            if (!sidebarCollapsed) {
                sidebar.style.width = '256px';
                mainContent.style.marginLeft = '256px';
                document.querySelectorAll('.sidebar-text').forEach(el => el.style.display = '');
            }
        } else {
            sidebar.style.width = '256px';
            mainContent.style.marginLeft = '0';
        }
    });
})();