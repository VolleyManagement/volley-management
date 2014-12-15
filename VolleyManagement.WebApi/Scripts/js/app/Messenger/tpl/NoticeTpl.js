var noticeTpl = _.template([
    '<div class="alert alert-<%= type %> alert-dismissible">',
        '<i class="glyphicon glyphicon-ok"></i>',
        '<span class="alert-text"><%= message %></span>',
        '<button type="button" class="close">',
        '<span>&times;</span>',
        '</button>',
    '</div>'
].join(''));