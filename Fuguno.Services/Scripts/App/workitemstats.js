/// Model
var WorkItemStatsModel = Backbone.Model.extend({
    urlRoot: "api/workitemstats",
    defaults: {
        ConnectionName: "",
        Heading: "",
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
var WorkItemStatsView = Backbone.View.extend({
    initialize: function () {
        this.template = _.template($(this.options.templateId).html());

        _.bindAll(this, "render");
        this.model.bind("change", this.render); // bind the "render" function to the model "change" event
    },

    render: function () {
        var data = this.model.toJSON();
        this.$el.html(this.template(data));
        this.createChart();
        return this;
    },

    createChart: function() {
        var ticksData = new Array();
        var ticks = this.model.get("Ticks");
        for (var i = 0; i < ticks.count() ; ++i) {
            ticksData.push([i, ticks[i]]);
        }

        var seriesData = new Array();
        var series = this.model.get("Series");
        for (var i = 0; i < series.count() ; ++i) {
            seriesData.push({ label: series[i].Label, data: [] });
            var color = this.calculateColor(series[i].Label);
            if (color != null)
                seriesData[i].color = color;

            for (var j = 0; j < series[i].Data.count() ; ++j) {
                seriesData[i].data.push([j, series[i].Data[j]]);
            }
        }

        var placeholder = this.$el.children(".chart");
        $.plot(
            placeholder,
            seriesData, {
                series: {
                    stack: true,
                    bars: {
                            show: true,
                            barWidth: 0.9, align: "center"
                    }
                },
                xaxis: {
                    ticks: ticksData
                },
                grid: {
                    show: true
                }
            });

    },

    calculateColor: function (label) {
        switch (label) {
            case "P0":
                return "black";
            case "P1":
                return "red";
            case "P2":
                return "orange";
            case "P3":
                return "green";
            default:
                return null;
        }
    }
});

