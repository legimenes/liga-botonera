(function () {
    window.closeModal = function () {
        const modalOverlay = document.getElementById('modal-overlay');
        if (modalOverlay) {
            modalOverlay.classList.add('opacity-0');
            setTimeout(() => modalOverlay.remove(), 200);
        }
        document.removeEventListener('keydown', handleEscape);
    };

    function handleEscape(e) {
        if (e.key === 'Escape') {
            closeModal();
        }
    }

    //if (type === 'confirm' && onConfirm) {
    //    document.getElementById('modalMessageConfirm')
    //        ?.addEventListener('click', () => {
    //            onConfirm();
    //            closeModal();
    //        });
    //}

    document.addEventListener('keydown', handleEscape);

    setTimeout(() => {
        const btn = document.getElementById('messageModalCloseButton');
        if (btn) btn.focus();
    }, 100);
})();