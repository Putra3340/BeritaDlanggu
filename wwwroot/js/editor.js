window.richEditor = {
    quillMap: {},

    init: function (id, dotnetRef) {
        const quill = new Quill('#' + id, {
            theme: 'snow',
            placeholder: 'Tulis sesuatu yang indah...',
            modules: {
                toolbar: [
                    [{ header: [1, 2, 3, false] }],

                    ['bold', 'italic', 'underline', 'strike'],
                    [{ color: [] }, { background: [] }],

                    [{ list: 'ordered' }, { list: 'bullet' }],
                    [{ align: [] }],

                    ['blockquote', 'code-block'],

                    ['link', 'image', 'video'],

                    ['clean']
                ]
            }
        });

        this.quillMap[id] = quill;

        quill.on('text-change', function () {
            dotnetRef.invokeMethodAsync('NotifyChange', quill.root.innerHTML);
        });
    },

    setHtml: function (html) {
        const quill = Object.values(this.quillMap)[0];
        if (quill) quill.root.innerHTML = html || '';
    }
};