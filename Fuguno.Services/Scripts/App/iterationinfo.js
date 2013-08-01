/// Model
var IterationInfoModel = Backbone.Model.extend({
    urlRoot: "api/iterationinfo",
    defaults: {
        Name: "",
        StartDate: "",
        EndDate: ""
    },
});

// View
var IterationInfoView = Backbone.View.extend({
    initialize: function () { 
        _.bindAll(this, "render");

        this.model.bind("change", this.render); // bind the "render" function to the model "change" event
    },

    render: function () {
        var data = this.model.toJSON();
        var compiled = _.template($("#iterationInfoTemplate").html());
        this.$el.html(compiled(data));
        return this;
    },
});
