(function (This) {
	'use strict';
	This.teamItemTpl = _.template([
'	<tr>',
'		<td class="summarySeparatedTd"><%= place %></td>',
'		<td class="summarySeparatedTd"><%= teamName %></td>',
'		<td class="summarySeparatedTd"><%= points %></td>',
'		<td><%= game.all%></td>',
'		<td><%= game.win%></td>',
'		<td><%= game.lost%></td>',
'		<td><%= game.game30%></td>',
'		<td><%= game.game31%></td>',
'		<td><%= game.game32%></td>',
'		<td><%= game.game23%></td>',
'		<td><%= game.game13%></td>',
'		<td class="summarySeparatedTd"><%= game.game03%></td>',
'		<td><%= sets.win%></td>',
'		<td><%= sets.lost%></td>',
'		<td class="summarySeparatedTd"><%= sets.koef%></td>',
'		<td><%= balls.win%></td>',
'		<td class="summarySeparatedTd"><%= balls.lost%></td>',
'		<td><%= balls.koef%></td>',
'	</tr>'].join(''));
})(summary);