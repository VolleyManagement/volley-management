var tournamentCollectionTpl = _.template([
    '<div class="panel panel-default">',
        '<div class="panel-heading">',
            '<div class="row">',
                '<div class="col-md-9 col-sm-8">',
                    '<h4>Список турниров</h4>',
                '</div>',
                '<div class="col-md-3 col-sm-4">',
                    '<button class="btn btn-success pull-right create">',
                        '<i class="glyphicon glyphicon-plus-sign"></i> Добавить',
                    '</button>',
                '</div>',
            '</div>',
        '</div>',
        '<ul class="list-group">',
        '</ul>',
    '</div>'
].join(''));