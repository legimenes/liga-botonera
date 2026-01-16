(function () {
    const modalOverlay = document.getElementById('modal-overlay');
    const modalTitle = document.getElementById('modal-title');
    const modalContent = document.getElementById('modal-content');
    const modalIconContainer = document.getElementById('modal-icon');
    const modalFooter = document.getElementById('modal-footer');

    window.closeModal = function () {
        if (modalOverlay) {
            modalOverlay.classList.add('hidden');
        }
        document.removeEventListener('keydown', handleEscape);
    };

    function handleEscape(e) {
        if (e.key === 'Escape') {
            window.closeModal();
        }
    }

    window.showMessageModal = function (options) {
        if (!modalOverlay || !modalTitle || !modalContent || !modalIconContainer || !modalFooter) {
            console.error("Message Modal elements not found.");
            return;
        }

        modalTitle.textContent = options.title || 'Message';
        modalContent.textContent = options.message || '';

        modalIconContainer.innerHTML = getIconSvg(options.type);

        modalFooter.innerHTML = '';
        if (options.type === 'Question') {
            const cancelButton = document.createElement('button');
            cancelButton.type = 'button';
            cancelButton.className = 'px-6 py-2 rounded-lg border border-slate-300 dark:border-emerald-600 text-slate-700 dark:text-slate-300 hover:bg-slate-100 dark:hover:bg-emerald-700';
            cancelButton.textContent = options.cancelText || 'Cancelar';
            cancelButton.onclick = window.closeModal;
            modalFooter.appendChild(cancelButton);

            const confirmButton = document.createElement('button');
            confirmButton.type = 'button';
            confirmButton.className = 'px-6 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700';
            confirmButton.textContent = options.confirmText || 'Confirmar';
            confirmButton.onclick = () => {
                if (options.onConfirm) {
                    options.onConfirm();
                }
                window.closeModal();
            };
            modalFooter.appendChild(confirmButton);
        } else {
            const okButton = document.createElement('button');
            okButton.type = 'button';
            okButton.className = 'px-6 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700';
            okButton.textContent = options.confirmText || 'Ok';
            okButton.onclick = window.closeModal;
            modalFooter.appendChild(okButton);
        }

        modalOverlay.classList.remove('hidden');
        document.addEventListener('keydown', handleEscape);

        setTimeout(() => {
            const btn = modalFooter.querySelector('button');
            if (btn) btn.focus();
        }, 100);
    };

    function getIconSvg(type) {
        const types = {
            'Warning': `<svg width="30" height="30" viewBox="0 0 24 24" fill="none" xmlns="http:www.w3.org/2000/svg">
                <path d="M12 2L1 21H23L12 2Z" fill="#FFD700" stroke="#000000" stroke-width="1" stroke-linejoin="round"/>
                <rect x="11" y="9" width="2" height="6" fill="black"/>
                <rect x="11" y="17" width="2" height="2" fill="black"/>
            </svg>`,
            'Success': `<svg width="30" height="30" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="12" r="10" fill="#28A745"/>
                <path d="M8 12.5L11 15.5L16 9.5" stroke="white" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>`,
            'Error': `<svg width="30" height="30" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="12" r="10" fill="#FF0000"/>
                <path d="M15 9L9 15M9 9L15 15" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>`,
            'Question': `<svg width="30" height="30" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="12" r="10" fill="#007BFF"/>
                <path d="M9.09 9C9.3251 8.33167 9.78915 7.76811 10.4 7.40913C11.0108 7.05016 11.7289 6.91894 12.4272 7.03871C13.1255 7.15849 13.7588 7.52152 14.2151 8.06353C14.6713 8.60553 14.9211 9.29152 14.92 10C14.92 12 11.92 13 11.92 13" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"/>
                <circle cx="12" cy="17" r="1" fill="white"/>
            </svg>`,
            'Information': `<svg width="30" height="30" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="12" r="10" fill="#007BFF"/>
                <circle cx="12" cy="7" r="1.2" fill="white"/>
                <rect x="10.8" y="10" width="2.4" height="7" rx="1" fill="white"/>
            </svg>`
        };
        return types[type] || types['Information'];
    }

    if (modalOverlay) {
        modalOverlay.classList.add('hidden');
    }

})();