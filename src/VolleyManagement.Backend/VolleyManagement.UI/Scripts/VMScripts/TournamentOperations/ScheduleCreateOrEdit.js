$(document).ready(function () {
  'use strict';

  var currNs = VM.addNamespace("tournament.scheduleEdit"),
    me = {},
    divisionDD;
  var HomeTeamId = $(".home-team-select :selected").val();
  var AwayTeamId = $(".away-team-select :selected").val();
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
    var optGroups = $(dropdown).find("optgroup"),
      child,
      options,
      i,
      hasFirstSelected = false;

    for (i = 0; i < optGroups.length; i++) {
      child = optGroups[i];
      child.hidden = child.label !== me.selectedDivision;
      if (child.label === me.selectedDivision) {
        $(child).removeClass("hidden");
      } else {
        $(child).addClass("hidden");
      }
    }

    options = $(dropdown).find("option");
    if (HomeTeamId === AwayTeamId) {
      for (i = 0; i < options.length; i++) {
        child = $(options[i]);
        var parent = child.parent();
        if (parent.is("optgroup")) {
          if (!parent.hasClass("hidden")) {
            $(dropdown).value = child.value;
          }
        } else {
          $(dropdown).val(null);
        }
      }
    }
  }

  me.setSelected = function (child, hasFirstSelected) {
    child.selected = !hasFirstSelected;
    return child.selected;
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
