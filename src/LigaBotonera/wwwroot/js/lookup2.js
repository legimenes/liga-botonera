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
                    this.dispatchClearEvent();
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
                        this.dispatchClearEvent();
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
                    const payload = singleRow.getAttribute('data-payload');
                    this.onSelectedItem(id, name, payload);
                }
            },

            onConfigRequest(event) {
                if (this.lookupText === this.lastSearch) {
                    event.preventDefault();
                }
            },

            onSelectedItem(id, name, payload) {
                this.lookupText = name;
                this.lastSearch = name;
                document.getElementById('lookupId-' + this.idSuffix).value = id;

                let dataObj = {};
                if (payload) {
                    try { dataObj = JSON.parse(payload); } catch (e) { console.error("Payload parse error", e); }
                }

                window.dispatchEvent(new CustomEvent('lookup-selected', {
                    detail: { lookupId: this.idSuffix, id: id, name: name, data: dataObj },
                    bubbles: true
                }));

                document.getElementById('grid-container-' + this.idSuffix).innerHTML = '';
                this.showGrid = false;
            },

            dispatchClearEvent() {
                this.$el.dispatchEvent(new CustomEvent('lookup-cleared', {
                    detail: { lookupId: this.idSuffix },
                    bubbles: true
                }));
            }
        }));
    };
    if (window.Alpine) {
        init();
    } else {
        document.addEventListener('alpine:init', init);
    }
})();