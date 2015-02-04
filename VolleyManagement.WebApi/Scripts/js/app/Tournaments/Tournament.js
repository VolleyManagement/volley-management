"use strict";

(function (This)  {
	This.Tournament = Backbone.Model.extend({
	    urlRoot: '/OData/Tournaments',
	    
	    defaults: function () {
		    var year = (new Date()).getFullYear();

	    	return {
		        Name: '',
		        Description: '',
		        Season: (year+"/"+(year+1)),
		        Scheme: '1',
		        RegulationsLink: ''
	    	};
		}
	});
})(App.Tournaments);