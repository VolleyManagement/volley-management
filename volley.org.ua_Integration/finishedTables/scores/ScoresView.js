(function(This) {
    'use strict';
    This.ScoresView = function() {
        var teams = {},
            teamsScores = {},
            date;

        this.setData = function(_data) {
            teams = _data.teams;
            teamsScores = _data.teamsScores;
            date = _data.date;
        };

        this.render = function() {
            var $table = $('<table></table>');

            $table.html(getTableBody());

            $table.addClass('table table-bordered scoresTable');
            return {
                el: $table
            };
        };

        function getTableBody() {
            var $body = $('<tbody></tbody>');

            $body.append(getHeader());
            $body.append(getBody());

            return $body;
        }

        //TODO: for in in templates
        function getHeader() {
            return This.headerTpl({
                date: date,
                teams: teams
            });
        }

        function getBody() {
            var $frag = $(document.createDocumentFragment()),
                teamsId = getTeamsId(teams);

            $frag.html(This.bodyTpl({
                teamsId: teamsId,
                teams: teams
            }));

            addPoints($frag);
            addScores($frag);
            addColors($frag);
            return $frag;
        }

        function getTeamsId(teams) {
            return _.pluck(teams, 'id');
        }

        function addPoints($frag) {
            $frag.children().each(function(i, tr) {
                $(tr).find('td[teamValue="place"]').html(teams[i].place);
                $(tr).find('td[teamValue="name"]').html(teams[i].name);
                $(tr).find('td[teamValue="score"]').html(teams[i].score);
            });
        }

        function addScores($frag) {
            _.each(teamsScores, function(game) {

                (findAndAddScore($frag, {
                    firstTeamId: game.firstTeam,
                    secondTeamId: game.secondTeam,
                    firstScore: game.firstScore,
                    secondScore: game.secondScore
                })) &&
                (findAndAddScore($frag, {
                    firstTeamId: game.secondTeam,
                    secondTeamId: game.firstTeam,
                    firstScore: game.secondScore,
                    secondScore: game.firstScore
                }));
            });
        }

        function findAndAddScore($frag, options) {
            var $td = $frag.find('td[first="' + options.firstTeamId + '"][second="' + options.secondTeamId + '"]'),
                exists = false;

            if ($td) {
                $td.html(options.firstScore + ':' + options.secondScore);
                exists = true;
            }

            return exists;
        }

        function addColors($frag) {
            var colors = {
                    'oneTeam': '#000000',
                    '3:1': '#BFEE38',
                    '3:0': '#499A00',
                    '2:3': '#FFC71F',
                    '3:2': '#FFEB51',
                    '0:3': '#FF681F',
                    '1:3': '#FF841F'
                },
                score;

            $frag.find('tr').children().each(function(index, el) {
                score = $(el).html();

                if (colors[score]) {
                    $(el).css('background-color', colors[score]);
                } else {
                    if (isTheSameTeam($(el))) {
                        $(el).css('background-color', colors['oneTeam']);
                    }
                }
            });

            return $frag;
        }

        function isTheSameTeam($td) {
            return $td.attr('first') === $td.attr('second') && $td.attr('second');
        }
        return this;
    };
})(scores);
