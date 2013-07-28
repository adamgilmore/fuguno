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
        var ticksData = new Array();
        var ticks = this.model.get("Ticks");
        for (var i = 0; i < ticks.count() ; ++i) {
            ticksData.push([i, ticks[i]]);
        }

        var seriesData = new Array();
        var series = this.model.get("Series");
        for (var i = 0; i < series.count() ; ++i) {
            seriesData.push({ label: series[i].Label, data: [] });
            for (var j = 0; j < series[i].Data.count() ; ++j) {
                seriesData[i].data.push([j, series[i].Data[j]]);
            }
        }

        $.plot(this.$el.selector, seriesData, { series: { stack: true, bars: { show: true, barWidth: 0.9, align: "center" } }, xaxis: { ticks: ticksData } });
        return this;
    },
});
