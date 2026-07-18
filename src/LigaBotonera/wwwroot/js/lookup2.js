(() => {
    const init = () => {
        if (Alpine.store('lookupRegistered')) {
            return;
        }
        Alpine.store('lookupRegistered', true);

        Alpine.data('lookupComponent', (componentId) => ({
            lookupText: '',
            lastSearch: '',
            showGrid: false,
            idSuffix: componentId,

            onInput() {
                if (this.lookupText !== this.lastSearch) {
                    document.getElementById('lookupId-' + this.idSuffix).value = '';
                }
                if (this.lookupText.trim().length === 0) {
                    this.showGrid = false;
                    document.getElementById('grid-container-' + this.idSuffix).innerHTML = '';
                }
                this.lastSearch = '';
            },

            onBlur() {
                setTimeout(() => {
                    if (!document.getElementById('lookupId-' + this.idSuffix).value) {
                        this.lookupText = '';
                    }
                    document.getElementById('grid-container-' + this.idSuffix).innerHTML = '';
                    this.showGrid = false;
                }, 200);
            },

            onEnter() {
                const grid = document.getElementById('grid-container-' + this.idSuffix);
                const singleRow = grid.querySelector('[data-single-row="true"]');
                if (singleRow) {
                    const id = singleRow.getAttribute('data-id');
                    const name = singleRow.getAttribute('data-name');
                    this.onSelectedItem(id, name);
                }
            },

            onConfigRequest(event) {
                if (this.lookupText === this.lastSearch) {
                    event.preventDefault();
                }
            },

            onSelectedItem(id, name) {
                this.lookupText = name;
                this.lastSearch = name;
                document.getElementById('lookupId-' + this.idSuffix).value = id;
                document.getElementById('grid-container-' + this.idSuffix).innerHTML = '';
                this.showGrid = false;

                // Dispara o evento customizado
                // this.$el.dispatchEvent(new CustomEvent('lookup-selected', {
                //     detail: { id: id, name: nome },
                //     bubbles: true
                // }));
            }
        }));
    };
    if (window.Alpine) {
        init();
    } else {
        document.addEventListener('alpine:init', init);
    }
})();