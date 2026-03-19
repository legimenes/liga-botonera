(() => {
    const init = () => {
        Alpine.data('lookupComponent', (config) => ({
            searchTerm: config.initialValue || '',
            lastValidTerm: config.initialValue || '',
            dataId: config.initialId || '',
            showGrid: false,

            validate() {
                if (this.searchTerm !== this.lastValidTerm) {
                    this.searchTerm = '';
                    this.lastValidTerm = '';
                    this.dataId = '';
                    window.dispatchEvent(new CustomEvent(`lookupitemselected-${config.id}`, { detail: null }));
                }
            },

            handleSelection(data) {
                data = data?.detail !== undefined ? data.detail : data;

                if (data !== null && data?.detail === undefined) {
                    const cleanData = {};
                    for (let key in data) {
                        if (!(data[key] instanceof HTMLElement)) {
                            cleanData[key] = data[key];
                        }
                    }
                    data = cleanData;
                }

                if (data) {
                    if (data[config.displayProperty] !== undefined) {
                        this.searchTerm = data[config.displayProperty];
                        this.lastValidTerm = this.searchTerm;
                        this.dataId = data[config.dataIdProperty];
                    }
                    this.showGrid = false;
                    htmx.ajax('POST', `?handler=${config.selectedHandler}`, {
                        values: { selectedData: JSON.stringify(data) }
                    });
                }
                else {
                    this.showGrid = false;
                    htmx.ajax('POST', `?handler=${config.selectedHandler}`, {
                        values: { selectedData: JSON.stringify(data) }
                    });
                }
            },

            init() {
                // x-on:lookupitemselected.window
                window.addEventListener(`lookupitemselected-${config.id}`, (e) => {
                    this.handleSelection(e.detail);
                });

                // x-on:htmx:after-request
                this.$el.addEventListener('htmx:afterRequest', (e) => {
                    if (e.detail.successful && e.detail.xhr.responseText.trim().length > 0) {
                        this.showGrid = true;
                    } else {
                        this.showGrid = false;
                    }
                });

                // x-on:keydown.escape.stop
                this.$el.addEventListener('keydown', (e) => {
                    if (e.key === 'Escape') {
                        this.showGrid = false;
                    }
                });

                // x-on:blur
                this.$el.addEventListener('focusout', (e) => {
                    setTimeout(() => {
                        if (!this.$el.contains(document.activeElement)) {
                            this.showGrid = false;
                            this.validate();
                        }
                    }, 200);
                });

                this.$el.addEventListener('htmx:confirm', (e) => {
                    if (e.target.tagName === 'INPUT' && e.target.name === 'searchQuery') {
                        const valor = this.searchTerm.trim();
                        if (!valor || valor.length === 0) {
                            e.preventDefault();
                            this.showGrid = false;
                        }
                    }
                });
            }
        }));
    };

    if (window.Alpine) {
        init();
    }
    else {
        document.addEventListener('alpine:init', init);
    }
})();