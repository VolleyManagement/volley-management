$(document).ready(function () {
  'use strict';

  var currNs = VM.addNamespace("tournament.scheduleEdit"),
    me = {},
    divisionDD;

  me.updateDropdownStates = function () {
    var dropDowns = [
      $(".round-select")[0],
      $(".home-team-select")[0],
      $(".away-team-select")[0]
    ];

    dropDowns.forEach(function (dd) {
      me.updateStateForDropDown(dd);
    });
  }

  me.updateStateForDropDown = function (dropdown) {
    var list = $(dropdown).find("optgroup"),
      child;
    for (var i = 0; i < list.length; i++) {
      child = list[i];
      child.hidden = child.label !== me.selectedDivision;
      if (child.label === me.selectedDivision) {
        $(child).removeClass("hidden");
      } else {
        $(child).addClass("hidden");
      }
    }
  }

  me.getDivisionDropdown = function () {
    return $(".division-select")[0];
  }

  me.getSelectedText = function (dropDown) {
    if (dropDown.selectedIndex === -1)
      return null;

    return dropDown.options[dropDown.selectedIndex].text;
  }

  me.onDivisionChange = function (event) {
    me.selectedDivision = me.getSelectedText(event.target);
    me.updateDropdownStates();
  }

  divisionDD = me.getDivisionDropdown();
  me.selectedDivision = me.getSelectedText(divisionDD);

  $(divisionDD).on("change", me.onDivisionChange.bind(me));

  me.updateDropdownStates();
});
