'use strict';
$(document).ready(function () {
  (function () {

    var environment = navigator.userAgent;
    var isUserAgree = $('#isUserAgree');
    var UserEnvironment = $('#UserEnvironment');

    isUserAgree.prop('checked', true);
    UserEnvironment.val(environment);

    isUserAgree.change(function () {
      if (this.checked) {
        UserEnvironment.val(environment);
      }
      else {
        UserEnvironment.val('');
      }
    });

    $('#feedbackForm').submit(function () {
      if (!isUserAgree.prop('checked')) {
        UserEnvironment.val('');
      }
    });

    $('#Content').focus();

  })();
});