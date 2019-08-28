$(document).ready(function () {
  'use strict';

  $('#fileToUpload').hide();
  $('#uploadSubmit').hide();
  $('#uploadButton').on('click', function () {
    $('#fileToUpload').click();
  });
  $('#fileToUpload').change(function () {
    $('#uploadSubmit').click();
  });

});