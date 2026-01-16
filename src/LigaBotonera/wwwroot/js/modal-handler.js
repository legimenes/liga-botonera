function openGenericModal(url) {
    const modal = document.getElementById('generic-modal');
    const modalContent = document.getElementById('generic-modal-content');

    // Show loading indicator (optional)
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

function closeGenericModal() {
    const modal = document.getElementById('generic-modal');
    modal.classList.add('hidden');
    const modalContent = document.getElementById('generic-modal-content');
    modalContent.innerHTML = ''; // Clear content when closing
}