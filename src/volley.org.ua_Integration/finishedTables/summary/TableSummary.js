(function(This) {
    This.TableSummary = function() {
        'use strict';
        var data = {};

        this.setData = function(_data) {
            data = _data;
        };

        this.render = function() {
            var table = $('<table></table>');

            table.html(getTableBody());
            table.addClass('table summaryTable');

            return {
                el: table
            };
        };

        function getHeader() {
            return This.summaryHeaderTpl(data);
        }

        function getBody() {
            var frag = $(document.createDocumentFragment());

            _.each(data.teams, function(team) {
                frag.append(This.teamItemTpl(team));
            }, this);

            return frag;
        }

        function getTableBody() {
            var tableBody = $('<tbody></tbody>');

            tableBody.append(getHeader());
            tableBody.append(getBody());

            return tableBody;
        }

        return this;
    };
})(summary);
