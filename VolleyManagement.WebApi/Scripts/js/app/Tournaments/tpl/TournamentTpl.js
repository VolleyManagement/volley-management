var tournamentTpl = _.template([
    '<div class="row tournament">',
        '<div class="col-sm-9">',
            '<h4><%= Name %></h4>',
        '</div>',
        '<div class="col-sm-3">',
            '<h3 class="text-right"><span class="label label-default"><%= Season %></span></h3>',
        '</div>',
    '</div>'
].join(''));