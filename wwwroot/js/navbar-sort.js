window.initNavbarSort = (elementId, dotnetHelper) => {

    const el = document.getElementById(elementId);

    Sortable.create(el, {
        animation: 150,
        handle: '.drag-handle',

        onEnd: function (evt) {

            dotnetHelper.invokeMethodAsync(
                'UpdateNavbarOrder',
                evt.oldIndex,
                evt.newIndex
            );
        }
    });
};