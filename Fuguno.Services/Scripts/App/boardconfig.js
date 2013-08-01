var LocalStorageKey_TeamNames = "teamNames";
var LocalStorageKey_BuildDefinitionNames = "buildDefinitionNames";

function isBoardConfigured() {
    return $.localStorage.isSet(LocalStorageKey_TeamNames);
}

function getBoardConfigSetting(localStorageKey) {
    var setting = $.localStorage.get(localStorageKey);
    return setting;
}

function bindListToTemplate(list, templateSelector, el, localStorageKey) {
    var selectedItems = getBoardConfigSetting(localStorageKey).split(",");

    _.each(list, function (item) {
        var compiled = _.template($(templateSelector).html());
        var isSelected = $.inArray(item, selectedItems) > -1;
        var html = compiled({ name: item, selected: isSelected });
        var element = $(el);
        element.append(html);
    });
}

function storeSelectedListItems(el, localStorageKey) {
    var list = [];
    $(el + " option:selected").each(function () {
        list.push($(this).text());
    });

    $.localStorage.set(localStorageKey, list);
}