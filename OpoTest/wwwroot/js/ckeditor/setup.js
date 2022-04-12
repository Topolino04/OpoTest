CKEditorInterop = (() => {
    var editors = {};
    return {
        init(id, initialData, dotNetReference) {
            window.ClassicEditor
                .create(document.getElementById(id),
                    {
                        toolbar: {
                            items: [
                                'findAndReplace',
                                '|',
                                'undo',
                                'redo',
                                '|',
                                'heading',
                                'fontFamily',
                                'fontSize',
                                'fontColor',
                                'fontBackgroundColor',
                                'fontBackgroundColor',
                                'highlight',
                                '|',
                                'alignment',
                                'imageInsert',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'mediaEmbed',
                                '-',
                                'bold',
                                'italic',
                                'link',
                                'underline',
                                'strikethrough',
                                'specialCharacters',
                                'superscript',
                                'subscript',
                                '|',
                                'bulletedList',
                                'numberedList',
                                'todoList',
                                '|',
                                'outdent',
                                'indent',
                                'horizontalLine'

                            ],
                            shouldNotGroupWhenFull: true
                        },
                        image: {
                            toolbar: [
                                'imageTextAlternative',
                                'imageStyle:inline',
                                'imageStyle:block',
                                'imageStyle:side',
                                'linkImage'
                            ]
                        },
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells',
                                'tableCellProperties'
                            ]
                        },
                        language: 'es',
                    })
                .then(editor => {
                    editors[id] = editor;
                    editor.setData(initialData ?? "");
                    editor.model.document.on('change:data', () => {
                        var data = editor.getData();
                        var el = document.createElement('div');
                        el.innerHTML = data;

                        if (el.innerText.trim() === '')
                            data = null;

                        dotNetReference.invokeMethodAsync('EditorDataChanged', data);
                    });
                })
                .catch(error => console.error(error));
        },
        setData(id, data) {
            editors[id].setData(data);
        },
        destroy(id) {
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
        }
    };
})();