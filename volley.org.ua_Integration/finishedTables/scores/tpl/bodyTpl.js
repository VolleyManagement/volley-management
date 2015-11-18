(function (This) {
    'use strict';
    This.bodyTpl = _.template([
        '<% _.each(teams, function(team) { %>',
            '<tr>',
                '<td class="scoresVerticalHeader greyBackground" teamValue="place"></td>',
                '<td class="scoresVerticalHeader greyBackground" teamValue="name"></td>',

                '<% _.each(teamsId, function(id) {%>',
                    '<td first=<%=team.id%> second=<%=id%>></td>',
                '<%}, this);%>',

                '<td class="scoresVerticalHeader" teamValue="score"></td>',
            '</tr>',
        '<%}, this);%>'
    ].join(''));
})(scores);