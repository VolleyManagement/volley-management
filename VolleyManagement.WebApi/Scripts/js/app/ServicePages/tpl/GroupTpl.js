var groupTpl = _.template([
    '<div class="panel panel-default col-md-5 col-sm-10 group-container">',
        '<div class="row">',
            '<div class="col-md-6 group-logo-container">',
                //'<img class="group-logo" src="img/<%= itaName %>.jpg">',
                '<img class="group-logo img-circle" src="https://s-media-cache-ak0.pinimg.com/236x/14/47/57/1447579351fff031e683a60441929fdd.jpg">',
            '</div>',
            '<div class="col-md-6 group-info-container">',
                    '<h3 calss="group-name"> <%= name %></h3>',
                    '<ul class="list-unstyled">',
                        '<% _.each (contributors, function (contributor) { %>',
                            '<li><%= contributor %></li>',
                        '<% }); %>',
                    '</ul>',
            '</div>',
        '</div>',
    '</div>'
].join(''));