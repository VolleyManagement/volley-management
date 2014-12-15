Backbone.Model = Backbone.Model.extend({
    url: function () {
        var base =
           _.result(this, 'urlRoot') ||
           _.result(this.collection, 'url') ||
           urlError();
        
		if (this.isNew()) {
		    return base;
		}
        
		return (base.replace(/([^\/])$/, '$1(') + encodeURIComponent(this.id) + ')');
    }
});	

