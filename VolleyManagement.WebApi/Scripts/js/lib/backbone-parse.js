Backbone.Model = Backbone.Model.extend({
    parse: function (resp, options) {
        var out = resp.Id? resp: resp.value;

        out.id = out.Id;
        //console.log(out);

        return (out);
       // return (resp.Id ? resp : resp.value);
    }
});

Backbone.Collection = Backbone.Collection.extend({
    parse: function (resp, options) {
        

        return resp.value;
    }
}); 
