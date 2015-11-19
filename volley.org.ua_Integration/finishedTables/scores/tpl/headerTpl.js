(function(This) {
    'use strict';
    This.headerTpl = _.template([
        '<tr>',
        '<td class="greyBackground">',
        '</td>',
        '<td class="scoresHorizontalHeader greyBackground">',
        'На <%= date%>',
        '</td>',
        '<% _.each(teams, function(team) { %>',
        '<td class="scoresVerticalHeader greyBackground">',
        '<%=team.place%>',
        '</td>',
        '<% })%>',
        '<td class="scoresVerticalHeader greyBackground">',
        '	Points',
        '</td>',
        '</tr>',
    ].join(''));
})(scores);
