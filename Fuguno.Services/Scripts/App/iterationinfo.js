/// Model
var IterationInfoModel = Backbone.Model.extend({
    urlRoot: "api/iterationinfo",
    defaults: {
        ConnectionName: "",
        Name: "",
        Path: "",
        StartDate: "",
        EndDate: ""
    },

    url: function () {
        var params = {
            connectionName: this.get("ConnectionName"),
            rootIterationPath: this.get("Path")
        }

        return this.urlRoot + "?" + $.param(params);
    }
});

// View
var IterationInfoView = Backbone.View.extend({
    template: _.template($("#iterationInfoTemplate").html()),

    initialize: function () {
        _.bindAll(this, "render");

        this.model.bind("change", this.render); // bind the "render" function to the model "change" event
    },

    render: function () {
        var data = this.model.toJSON();
        this.$el.html(this.template(data));
        return this;
    },
});

function fetchIterationInfoModel(model) {
    model.fetch({
        success: function (model, response) {
        }
    });
}