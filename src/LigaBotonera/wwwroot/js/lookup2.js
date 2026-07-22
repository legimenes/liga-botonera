(() => {
    const init = () => {
        if (Alpine.store('lookupRegistered')) {
            return;
        }
        Alpine.store('lookupRegistered', true);

        Alpine.data('lookupComponent', (config) => ({
            lookupValue: config.initialValue || '',
            lastSearch: config.initialValue || '',
            showGrid: false,
            idSuffix: config.componentId,

            onHtmxAfterOnLoad(event) {
                if (event.detail.target.id === 'grid-container-' + this.idSuffix) {
                    this.showGrid = event.detail.target.innerHTML.trim() !== '';
                }
            },

            onInput() {
                if (this.lookupValue !== this.lastSearch) {
                    document.getElementById('lookupId-' + this.idSuffix).value = '';
                    this.dispatchClearEvent();
                }
                if (this.lookupValue.trim().length === 0) {
                    this.showGrid = false;
                    document.getElementById('grid-container-' + this.idSuffix).innerHTML = '';
                }
                this.lastSearch = '';
            },

            onBlur() {
                setTimeout(() => {
                    if (!document.getElementById('lookupId-' + this.idSuffix).value) {
                        this.lookupValue = '';
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
                if (this.lookupValue === this.lastSearch) {
                    event.preventDefault();
                }
            },

            onSelectedItem(id, name, payload) {
                this.lookupValue = name;
                this.lastSearch = name;
                document.getElementById('lookupId-' + this.idSuffix).value = id;

                let dataObj = {};
                if (payload) {
                    try {
                        dataObj = JSON.parse(payload);
                    } catch (e) {
                        console.error("Payload parse error", e);
                    }
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

        Alpine.data('bindLookup', (lookupId, fieldMapping = {}) => {
            const getMapping = (mappingValue) => {
                if (typeof mappingValue === 'object' && mappingValue !== null) {
                    return {
                        property: mappingValue.field,
                        defaultValue: mappingValue.default !== undefined ? mappingValue.default : ''
                    };
                }
                return { property: mappingValue, defaultValue: '' };
            };

            const state = {};
            Object.entries(fieldMapping).forEach(([key, mappingValue]) => {
                state[key] = getMapping(mappingValue).defaultValue;
                //state[key] = null;
            });

            return {
                ...state,

                init() {
                    window.addEventListener('lookup-selected', (e) => {
                        if (e.detail.lookupId !== lookupId) return;
                        Object.entries(fieldMapping).forEach(([localKey, mappingValue]) => {
                            const mapping = getMapping(mappingValue);
                            this[localKey] = e.detail.data[mapping.property] ?? mapping.defaultValue;
                        });
                    });

                    window.addEventListener('lookup-cleared', (e) => {
                        if (e.detail.lookupId !== lookupId) return;

                        Object.entries(fieldMapping).forEach(([localKey, mappingValue]) => {
                            const mapping = getMapping(mappingValue);
                            this[localKey] = mapping.defaultValue;
                        });
                    });
                }
            };
        });
    };
    if (window.Alpine) {
        init();
    } else {
        document.addEventListener('alpine:init', init);
    }
})();