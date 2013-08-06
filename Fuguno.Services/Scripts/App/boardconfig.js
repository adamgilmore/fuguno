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
    var selectedItems = getBoardConfigSetting(localStorageKey)
    if (selectedItems != null)
        selectedItems = selectedItems.split(",");

    _.each(list, function (item) {
        var compiled = _.template($(templateSelector).html());
        var isSelected = false;
        if (selectedItems != null) {
            isSelected = $.inArray(item, selectedItems) > -1;
        }
        
        var html = compiled({ name: item, selected: isSelected });
        $(el).append(html);
    });
}

function storeSelectedListItems(el, localStorageKey) {
    var list = [];
    $(el + " option:selected").each(function () {
        list.push($(this).text());
    });

    $.localStorage.set(localStorageKey, list);
}