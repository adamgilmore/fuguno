function isBoardConfigured() {
    return $.localStorage.isSet("teamNames");
}

function storeTeamNames(teamNames) {
    $.localStorage.set("teamNames", teamNames);
}

function retrieveTeamNames() {
    return [$.localStorage.get("teamNames")];
}

function bindTeamNamesToTemplate(list, templateSelector, el) {
    var selectedItems = retrieveTeamNames();

    _.each(list, function (item) {
        var compiled = _.template($(templateSelector).html());
        var isSelected = $.inArray(item, selectedItems) > -1;
        var html = compiled({ name: item, selected: isSelected });
        var element = $(el);
        element.append(html);
    });
}