/// Model
var BugJailInfoModel = Backbone.Model.extend({
    defaults: {
        Name: "",
        ImageUrl: ""
    }
});

// Model View
var BugJailInfoView = Backbone.View.extend({
    initialize: function () {
        _.bindAll(this, "render");

        this.model.bind("change", this.render); // bind the "render" function to the model "change" event
    },

    render: function () {
        var data = this.model.toJSON();
        var compiled = _.template($("#bugJailInfoTemplate").html());
        this.$el.html(compiled(data));
        return this;
    },
});

// Collection
var BugJailInfoList = Backbone.Collection.extend({
    model: BugJailInfoModel,
    initialize: function (models, options) {
        this.TeamName = options.TeamName;
    },
    url: function () {
        var params = {
            teamName: this.TeamName
        }

        var url = "api/bugjailinfo?" + $.param(params);
        return url;
    }
});


// Collection View
var BugJailInfoListView = Backbone.View.extend({
    initialize: function () {
        // "_" is the Underscore object and .bindAll is a helper to bind event handlers to an object (this) 
        _.bindAll(this, "render", "appendItem");

        // Create a collection and bind the "appendItem" function to the "add" event of the collection
        this.collection = new BugJailInfoList([], {
            TeamName: this.options.TeamName
        });

        this.collection.bind("add", this.appendItem);

        this.render();
    },

    render: function () {
        // Create local variable to store "this" as it is used in the callback in the .each method
        var self = this;

        this.childViews = [];

        _(this.collection.models).each(function (item) {
            self.appendItem(item);
        }, this);
    },

    // Creates a new item view using the "item" that was added to the collection. Gets the view to create it's DOM elements and these are then added to the root <ul> (that is a child of the this.el - see render function)
    appendItem: function (item) {
        var itemView = new BugJailInfoView({
            model: item
        });

        this.childViews.push(itemView);
        this.$el.append(itemView.render().el);
    },

    fetchCollection: function () {
        var self = this;

        this.collection.fetch({
            success: function () {
                _.each(self.childViews, function (view) {
                    view.remove();
                    view.unbind();
                });

                self.render();
            }
        });
    }
});
