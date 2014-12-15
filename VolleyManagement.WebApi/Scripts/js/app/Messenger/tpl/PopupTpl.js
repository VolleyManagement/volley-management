var popupTpl = _.template([
    '<div class="modal-backdrop fade in"></div>',
    '<div class="modal-dialog modal-md">',
        '<div class="modal-content">',
            '<div class="modal-header">',
                '<h4 class="modal-title"><%= message %> </h4>',
            '</div>',
            '<div class="modal-body clearfix">',
                '<div class="btn-toolbar pull-right">',
                    '<button type="button" class="btn btn-danger delete">Удалить</button>',
                    '<button type="button" class="btn btn-success cancel">Отмена</button>',
                '</div>',
            '</div>',
        '</div>',
    '</div>'
].join(''));