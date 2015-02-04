Backbone.Model = Backbone.Model.extend({
    parse: function (resp, options) {

        return (resp.Id ? resp : resp.value);
    }
});

Backbone.Collection = Backbone.Collection.extend({
    parse: function (resp, options) {
        return resp.value;
    }
}); 
