﻿/// Model
var BuildInfoModel = Backbone.Model.extend({
    defaults: {
        name: "C",
        definition: "CI",
        startTime: "",
        finishTime: "",
        triggeredBy: "Adam Gilmore"
    }
});

// Collection
var BuildInfoList = Backbone.Collection.extend({
    model: BuildInfoModel
});

// Item View
var BuildInfoView = Backbone.View.extend({
    template: _.template($("#buildInfoTemplate").html()),

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

// List View
var BuildInfoListView = Backbone.View.extend({
    initialize: function () {
        // "_" is the Underscore object and .bindAll is a helper to bind event handlers to an object (this) 
        _.bindAll(this, "render", "addItem", "appendItem");

        // Create a collection and bind the "appendItem" function to the "add" event of the collection
        this.collection = new BuildInfoList();
        this.collection.bind("add", this.appendItem);

        this.render();
    },

    // Creates the <button> and root <ul> then calls appendItem for each model in the collection 
    render: function () {
        // Create local variable to store "this" as it is used in the callback in the .each method
        var self = this;

        _(this.collection.models).each(function (item) {
            self.appendItem(item);
        }, this);
    },

    // Button "click" event handler - adds a new model to the collection - which triggers the "add" event on the collection which is bound to the "appendItem" method
    addItem: function () {
        var item = new BuildInfoModel();
        this.collection.add(item);
    },

    // Creates a new item view using the "item" that was added to the collection. Gets the view to create it's DOM elements and these are then added to the root <ul> (that is a child of the this.el - see render function)
    appendItem: function (item) {
        var itemView = new BuildInfoView({
            model: item
        });

        this.$el.append(itemView.render().el);
    }
});