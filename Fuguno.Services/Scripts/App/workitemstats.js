/// Model
var WorkItemStatModel = Backbone.Model.extend({
    urlRoot: "api/workitemstats",
    defaults: {
        ConnectionName: "",
        Action:"",
        WorkItemType: "", 
        State: "", 
        AreaPaths: ""
    },

    url: function () {
        var params = {
            connectionName: this.get("ConnectionName"),
            workItemType: this.get("WorkItemType"),
            state: this.get("State"),
            areaPaths: this.get("AreaPaths")
        }

        return this.urlRoot + "/" + this.get("Action") + "?" + $.param(params);
    }
});

// View
var WorkItemStatView = Backbone.View.extend({
    initialize: function () {
        _.bindAll(this, "render");
        this.model.bind("change", this.render); // bind the "render" function to the model "change" event
    },

    render: function () {
        var data = this.model.get("Data");

        var seriesData = new Array();
        for (var i = 0; i < data.count() ; ++i) {
            seriesData[i] = [data[i].Key, data[i].Count];
        }

        $.plot("#chart", [seriesData], { series: { bars: { show: true, barWidth: 0.6, align: "center" } }, xaxis: { mode: "categories", tickLength: 0 } });
        return this;
    },
});
