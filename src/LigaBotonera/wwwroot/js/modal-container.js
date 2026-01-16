(function () {
    window.openModalContainer = function(url) {
        const modal = document.getElementById('modal-container');
        const modalContent = document.getElementById('modal-container-content');
        modalContent.innerHTML = '<div class="text-center text-slate-600 dark:text-emerald-300">Loading...</div>';
        modal.classList.remove('hidden');
        fetch(url)
            .then(response => response.text())
            .then(html => {
                modalContent.innerHTML = html;
            })
            .catch(error => {
                console.error('Error loading modal content:', error);
                modalContent.innerHTML = '<div class="text-center text-red-500">Failed to load content.</div>';
            });
    }

    window.closeModalContainer = function () {
        const modal = document.getElementById('modal-container');
        modal.classList.add('hidden');
        const modalContent = document.getElementById('modal-container-content');
        modalContent.innerHTML = '';
    }
})();