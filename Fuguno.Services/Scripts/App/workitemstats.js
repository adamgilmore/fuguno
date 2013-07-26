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

function fetchWorkItemStatModel(model) {
    model.fetch({
        success: function (model, response) {
            renderWorkItemStatChart(model);
        }
    });
}

function renderWorkItemStatChart(model) {
    var data = model.get("Data");

    var seriesData = new Array();
    for (var i = 0; i < data.count() ; ++i) {
        seriesData[i] = [data[i].Key, data[i].Count];
    }

    $.plot("#chart", [seriesData], { series: { bars: { show: true, barWidth: 0.6, align: "center" } }, xaxis: { mode: "categories", tickLength: 0 } });
}


//function fetchWorkItemStatModel(url, connectionName, workItemType, state, areaPaths, success) {
//    var params = {
//        connectionName: connectionName,
//        workItemType: workItemType,
//        state: state,
//        areaPaths: areaPaths
//    }

//    $.ajax({
//        url: url + "?" + $.param(params),
//        success: success
//    });
//}