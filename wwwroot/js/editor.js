window.richEditor = {
    quillMap: {},

    init: function (id, dotnetRef) {
        const quill = new Quill('#' + id, {
            theme: 'snow',
            placeholder: 'Tulis sesuatu yang indah...',

            modules: {
                toolbar: [
                    [{ font: [] }],
                    [{ header: [1, 2, 3, 4, 5, 6, false] }],

                    ['bold', 'italic', 'underline', 'strike'],
                    [{ script: 'sub' }, { script: 'super' }],

                    [{ color: [] }, { background: [] }],

                    [{ list: 'ordered' }, { list: 'bullet' }],
                    [{ indent: '-1' }, { indent: '+1' }],

                    [{ direction: 'rtl' }],
                    [{ align: [] }],

                    ['blockquote', 'code-block'],

                    ['link', 'image', 'video', 'formula'],

                    ['clean']
                ],

                clipboard: {
                    matchVisual: false
                },

                keyboard: {
                    bindings: {
                        // custom shortcuts here if you want
                    }
                },

                history: {
                    delay: 1000,
                    maxStack: 100,
                    userOnly: true
                },

                syntax: true // requires highlight.js
            }
        });

        this.quillMap[id] = quill;

        quill.on('text-change', function () {
            dotnetRef.invokeMethodAsync('NotifyChange', quill.root.innerHTML);
        });
        quill.root.addEventListener('input', function () {
            dotnetRef.invokeMethodAsync('NotifyChange', quill.root.innerHTML);
        });
    },

    setHtml: function (id, html) {
        const quill = this.quillMap[id];
        if (quill) {
            quill.clipboard.dangerouslyPasteHTML(html || '');
        }
    }
};